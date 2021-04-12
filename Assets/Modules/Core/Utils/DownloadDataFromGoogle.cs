using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using Core.Resource;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class DownloadDataFromGoogle
{
    private static readonly string baseDataUrl = "";

    private static readonly string urlEnding = "&single=true&output=csv";
    private static readonly string finalPath = $"{Path.Combine(Application.persistentDataPath, "Data/")}";
    private static readonly string cvsPath = "/Raw/";
    private static readonly string cvsEnd = ".csv";


    public static Dictionary<string, string> JsonData = new Dictionary<string, string>();

    public static List<DownloadData> Datas = new List<DownloadData>
    {
        new DownloadData(ResourceService.KLocalizationFile, "0"),
    };
    
    
#if UNITY_EDITOR
    [MenuItem("ImportExcel/DownloadFromNet")]
#endif
    public static IEnumerator DownLoadAndParseAll()
    {
        ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
        JsonData.Clear();
        
        var folder = Application.persistentDataPath + cvsPath;
        
        CreateFolder(folder);
        CreateFolder(finalPath);
        
        foreach (var data in Datas)
        {
            var path = folder + data.fileName + cvsEnd;
            Download(baseDataUrl + data.urlID + urlEnding, path).Join();
            var result = ToJson(new CsvReader(path));
            var resultPath = finalPath + data.fileName + ".json";
            Debug.Log(resultPath);

            if (File.Exists(resultPath))
            {
                File.Delete(resultPath);
            }
            
            File.WriteAllText(resultPath, result);
            JsonData.Add(data.fileName, result);
        }

        Debug.Log("Downloaded");
		yield break;
    }

    private static Thread Download(string address, string path)
    {
        var thread = new Thread(() =>
        {
            var client = new WebClient();
            client.DownloadFile(address, path);
        });
        thread.Start();
        thread.Join();
        return thread;
    }

    private static void CreateFolder(string path)
    {
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
    }
    

    public static string ToJson(CsvReader csv)
    {
        var result = new StringBuilder();

        result.Append("[");

        for (var rowIndex = 0; rowIndex < csv.rows.Count; rowIndex++)
        {
            var row = csv.rows[rowIndex];

            if (rowIndex != 0 && rowIndex != csv.rows.Count)
                result.Append(",");

            result.Append("{");
            for (var index = 0; index < row.Keys.Count; index++)
            {
                var rowKey = row.Keys[index];

                result.Append("\"");
                result.Append(rowKey);
                result.Append("\"");
                result.Append(":");
                if (!row[rowKey].Contains('['))
                    result.Append("\"");
                result.Append(row[rowKey]);
                if (!row[rowKey].Contains('['))
                    result.Append("\"");

                if (index < row.Keys.Count - 1)
                    result.Append(",");
            }

            result.Append("}");
        }

        result.Append("]");
        return result.ToString();
    }
}


public class Row
{
    public OrderedDictionary data;
    public int index = -1;

    public Row next;
    public Row prev;
    public List<Row> table;

    public Row(OrderedDictionary fill)
    {
        data = fill;
    }

    public int Length => data.Count;
    public IList<string> Keys => data.Keys.Cast<string>().ToList();

    public string this[string i] => data[i] as string;

    public string this[int i] => data[i] as string;

    public int Count => data.Count;

    public bool has(string key)
    {
        return data.Contains(key) && data[key] as string != "";
    }

    public string AtOrDefault(string key, string def = "")
    {
        return has(key) ? this[key] : def;
    }

    public T EnumOrDefault<T>(string key, T def = default(T))
    {
        if (has(key))
            return (T) Enum.Parse(typeof(T), this[key]);
        return def;
    }

    public string GetLast(string Key)
    {
        if (has(Key))
        {
            return this[Key];
        }

        var currentRow = this;
        while (currentRow.prev != null)
        {
            currentRow = currentRow.prev;

            if (currentRow.has(Key))
                return currentRow[Key];
        }

        throw new FormatException(Key + " not found; at[" + index + "]");
    }
}

public class CsvCell
{
    private readonly List<Row> table;
    private readonly int x;
    private readonly int y;

    public CsvCell(int _x, int _y, List<Row> _table)
    {
        x = _x;
        y = _y;
        table = _table;
    }

    public bool IsAttribute()
    {
        return table[x].Keys[y].StartsWith("Attr", StringComparison.InvariantCultureIgnoreCase);
    }

    public CsvCell GetOffset(int _x, int _y)
    {
        return new CsvCell(x + _x, y + _y, table);
    }

    public bool hasValue()
    {
        return table != null && x < table.Count && y < table[x].Count && table[x][y] != "";
    }

    public string GetValue()
    {
        try
        {
            if (x < table.Count && y < table[x].Count) return table[x][y];
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }

        return "";
    }
}

public class CsvReader : IEnumerable<Row>
{
    private readonly List<string> columns;
    public List<Row> rows = new List<Row>();


    public CsvReader(string fileName)
    {
        using (var r = new StreamReader(fileName))
        {
            string line;
            line = r.ReadLine();

            rows.Clear();

            if (line != null)
            {
                columns = line.Split(',').Select(v => v.Trim()).ToList();

                Debug.Log(fileName + " Header: " + line);
                var rowIndex = 0;
                while ((line = r.ReadLine()) != null)
                {
                    var rawCells = new List<string>();

                    try
                    {
                        var good = true;
                        var accumulatedCell = "";
                        for (var i = 0; i < line.Length; i++)
                            if (line[i] == ',' && good)
                            {
                                rawCells.Add(accumulatedCell.Trim());
                                accumulatedCell = "";
                            }
                            else if (line.Length > i + 1 && line[i] == '"' && line[i + 1] == '"')
                            {
                                accumulatedCell += '"';
                                i++;
                            }
                            else if (line[i] == '"')
                            {
                                good = !good;
                            }
                            else
                            {
                                accumulatedCell += line[i];
                            }

                        rawCells.Add(accumulatedCell);
                    }
                    catch (Exception e)
                    {
                        Debug.LogError(e.Message + e.StackTrace + ", at row " + rowIndex);
                    }

                    var cells = new OrderedDictionary();

                    try
                    {
                        var indexCounter = 0;
                        foreach (var cell in rawCells)
                        {
                            if (indexCounter < columns.Count)
                                cells[columns[indexCounter]] = cell;
                            indexCounter++;
                        }

                        rows.Add(new Row(cells));
                    }
                    catch (Exception e)
                    {
                        Debug.LogError(e.Message + e.StackTrace + ", at row " + rowIndex);
                    }

                    rowIndex++;
                }

                for (var i = 0; i < rows.Count; i++)
                {
                    if (i > 0)
                    {
                        rows[i].prev = rows[i - 1];
                        rows[i - 1].next = rows[i];
                    }

                    rows[i].index = i;
                    rows[i].table = rows;
                }
            }
        }
    }

    public int rowCount => rows.Count;

    public int colCount => columns.Count;

    public IEnumerator<Row> GetEnumerator()
    {
        return rows.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public Row atRow(int rowIndex)
    {
        return rows[rowIndex];
    }

    public Row atRow(string key, string value)
    {
        return rows.FirstOrDefault(val => { return val.has(key) && val[key] == value; });
    }

    public Row AtRealRow(int rowIndex)
    {
        return atRow(rowIndex - 2);
    }

    public List<string> GetColumns()
    {
        return columns;
    }


    public List<string> Column(string name)
    {
        return rows.Select(row => row[name]).ToList();
    }
}

public class DownloadData
{
    public string fileName;
    public string urlID;

    public DownloadData(string fileName, string urlId)
    {
        this.fileName = fileName;
        urlID = urlId;
    }
}