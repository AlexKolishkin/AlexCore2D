using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using Random = UnityEngine.Random;

public static class CollectionUtilities
{
    public static bool IsDefaultOrNull<T>(T obj)
    {
        return obj == null || EqualityComparer<T>.Default.Equals(obj, default(T));
    }

    public static float RoundTo(this float val, int digits)
    {
        return (float) Math.Round(val, digits);
    }

    public static float RoundTo6Dec(this float val)
    {
        return Mathf.Round(val * 1000000f) * 0.000001f;
    }

    public static T MoveFirstToLast<T>(this List<T> list)
    {
        var item = list[0];
        list.RemoveAt(0);
        list.Add(item);
        return item;
    }

    public static string ReplaceLastOccurrence(this string source, string find, string replace)
    {
        var place = source.LastIndexOf(find, StringComparison.Ordinal);

        if (place == -1)
            return source;

        var result = source.Remove(place, find.Length).Insert(place, replace);
        return result;
    }

    public static int ToIntFromTextSafe(this string text, int defaultReturn)
    {
        int res;
        return int.TryParse(text, out res) ? res : defaultReturn;
    }

    public static int CharToInt(this char c)
    {
        return c - '0';
    }

    #region List Management

    public static List<string> TryAddIfNewAndNotAmpty(this List<string> lst, string text)
    {
        if (!text.IsNullOrEmpty() && lst.IndexOf(text) == -1)
            lst.Add(text);

        return lst;
    }

    public static T TryTake<T>(this List<T> list, int index)
    {
        if (list.IsNullOrEmpty() || list.Count <= index)
            return default(T);

        var ret = list[index];

        list.RemoveAt(index);

        return ret;
    }

    public static string GetUniqueName<T>(this string s, List<T> list)
    {
        var match = true;
        var index = 1;
        var mod = s;


        while (match)
        {
            match = false;

            foreach (var l in list)
                if (l.ToString().SameAs(mod))
                {
                    match = true;
                    break;
                }

            if (match)
            {
                mod = s + index;
                index++;
            }
        }

        return mod;
    }

    public static int TotalCount(this List<int>[] lists)
    {
        return lists.Sum(e => e.Count);
    }

    public static T GetRandom<T>(this List<T> list)
    {
        return list.Count == 0 ? default(T) : list[Random.Range(0, list.Count)];
    }

    public static void ForceSet<T, G>(this List<T> list, int index, G val) where G : T
    {
        if (list == null || index < 0) return;

        while (list.Count <= index)
            list.Add(default(T));

        list[index] = val;
    }

    public static bool AddIfNew<T>(this List<T> list, T val)
    {
        if (list.Contains(val)) return false;

        list.Add(val);
        return true;
    }

    public static bool TryRemoveTill<T>(this List<T> list, int maxCountLeft)
    {
        if (list == null || list.Count <= maxCountLeft) return false;

        list.RemoveRange(maxCountLeft, list.Count - maxCountLeft);
        return true;
    }

    public static T TryGetLast<T>(this IList<T> list)
    {
        if (list == null || list.Count == 0)
            return default(T);

        return list[list.Count - 1];
    }

    public static T TryGet<T>(this List<T> list, int index)
    {
        if (list == null || index < 0 || index >= list.Count)
            return default(T);
        return list[index];
    }

    public static object TryGetObj(this IList list, int index)
    {
        if (list == null || index < 0 || index >= list.Count)
            return null;
        var el = list[index];
        return el;
    }

    public static T TryGet<T>(this List<T> list, int index, T defaultValue)
    {
        if (list == null || index < 0 || index >= list.Count)
            return defaultValue;

        return list[index];
    }

    public static int TryGetIndex<T>(this List<T> list, T obj)
    {
        var ind = -1;
        if (list != null && obj != null)
            ind = list.IndexOf(obj);

        return ind;
    }

    public static int TryGetIndexOrAdd<T>(this List<T> list, T obj)
    {
        var ind = -1;
        if (list == null || obj == null) return ind;

        ind = list.IndexOf(obj);

        if (ind != -1) return ind;

        list.Add(obj);
        ind = list.Count - 1;
        return ind;
    }

    public static void Move<T>(this List<T> list, int oldIndex, int newIndex)
    {
        if (oldIndex == newIndex) return;

        var item = list[oldIndex];
        list.RemoveAt(oldIndex);
        list.Insert(newIndex, item);
    }

    public static void SetFirst<T>(this List<T> list, T value)
    {
        for (var i = 0; i < list.Count; i++)
            if (list[i].Equals(value))
            {
                list.Move(i, 0);
                return;
            }

        if (list.Count > 0)
            list.Insert(0, value);
        else list.Add(value);
    }

    public static List<T> RemoveLast<T>(this List<T> list, int count)
    {
        var len = list.Count;

        count = Mathf.Min(count, len);

        var from = len - count;

        var range = list.GetRange(from, count);

        list.RemoveRange(from, count);

        return range;
    }

    public static T RemoveLast<T>(this List<T> list)
    {
        var index = list.Count - 1;

        var last = list[index];

        list.RemoveAt(index);

        return last;
    }

    public static T Last<T>(this List<T> list)
    {
        return list.Count > 0 ? list[list.Count - 1] : default(T);
    }

    public static void Swap<T>(this List<T> list, int indexOfFirst)
    {
        var tmp = list[indexOfFirst];
        list[indexOfFirst] = list[indexOfFirst + 1];
        list[indexOfFirst + 1] = tmp;
    }

    public static void Swap<T>(this IList<T> list, int indexA, int indexB)
    {
        var tmp = list[indexA];
        list[indexA] = list[indexB];
        list[indexB] = tmp;
    }

    public static bool IsNullOrEmpty(this IList list)
    {
        return list == null || list.Count == 0;
    }

    public static List<T> NullIfEmpty<T>(this List<T> list)
    {
        return list == null || list.Count == 0 ? null : list;
    }

    public static string CountToString(this IList lst)
    {
        return lst == null ? "NULL" : lst.Count.ToString();
    }

    #endregion

    #region Array Management

    public static T TryGetLast<T>(this T[] array)
    {
        if (array.IsNullOrEmpty())
            return default(T);

        return array[array.Length - 1];
    }

    public static T TryGet<T>(this T[] array, int index)
    {
        if (array == null || array.Length <= index || index < 0)
            return default(T);

        return array[index];
    }

    public static T TryGet<T>(this T[] array, int index, T defaultValue)
    {
        if (array == null || array.Length <= index || index < 0)
            return defaultValue;

        return array[index];
    }

    public static T[] GetCopy<T>(this T[] args)
    {
        var temp = new T[args.Length];
        args.CopyTo(temp, 0);
        return temp;
    }

    public static void Swap<T>(ref T[] array, int a, int b)
    {
        if (array == null || a >= array.Length || b >= array.Length || a == b) return;

        var tmp = array[a];
        array[a] = array[b];
        array[b] = tmp;
    }

    public static T[] Resize<T>(this T[] args, int to)
    {
        var temp = new T[to];
        if (args != null)
            Array.Copy(args, 0, temp, 0, Mathf.Min(to, args.Length));

        return temp;
    }

    public static T[] ExpandBy<T>(this T[] args, int add)
    {
        T[] temp;
        if (args != null)
        {
            temp = new T[args.Length + add];
            args.CopyTo(temp, 0);
        }
        else
        {
            temp = new T[add];
        }

        return temp;
    }

    public static void Remove<T>(ref T[] args, int ind)
    {
        var temp = new T[args.Length - 1];
        Array.Copy(args, 0, temp, 0, ind);
        var count = args.Length - ind - 1;
        Array.Copy(args, ind + 1, temp, ind, count);
        args = temp;
    }

    public static void AddAndInit<T>(ref T[] args, int add)
    {
        T[] temp;
        if (args != null)
        {
            temp = new T[args.Length + add];
            args.CopyTo(temp, 0);
        }
        else
        {
            temp = new T[add];
        }

        args = temp;
        for (var i = args.Length - add; i < args.Length; i++)
            args[i] = Activator.CreateInstance<T>();
    }

    public static T AddAndInit<T>(ref T[] args) where T : new()
    {
        T[] temp;
        if (args != null)
        {
            temp = new T[args.Length + 1];
            args.CopyTo(temp, 0);
        }
        else
        {
            temp = new T[1];
        }

        args = temp;
        var tmp = new T();
        args[temp.Length - 1] = tmp;
        return tmp;
    }

    public static void InsertAfterAndInit<T>(ref T[] args, int ind) where T : new()
    {
        if (args != null && args.Length > 0)
        {
            var temp = new T[args.Length + 1];
            Array.Copy(args, 0, temp, 0, ind + 1);
            if (ind < args.Length - 1)
            {
                var count = args.Length - ind - 1;
                Array.Copy(args, ind + 1, temp, ind + 2, count);
            }

            args = temp;
            args[ind + 1] = new T();
        }
        else
        {
            args = new T[ind + 1];
            for (var i = 0; i < ind + 1; i++)
                args[i] = new T();
        }
    }

    #endregion

    #region Dictionaries

    public static TValue GetOrCreate<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key) where TValue : new()
    {
        TValue val;

        if (dict.TryGetValue(key, out val)) return val;

        val = new TValue();
        dict.Add(key, val);

        return val;
    }

    public static T TryGet<T>(this Dictionary<string, T> dic, string tag)
    {
        T value;
        dic.TryGetValue(tag, out value);
        return value;
    }

    public static T TryGet<T, G>(this Dictionary<G, T> dic, G tag)
    {
        T value;
        dic.TryGetValue(tag, out value);
        return value;
    }

    public static bool TryChangeKey(this Dictionary<int, string> dic, int before, int now)
    {
        string value;
        if (dic.TryGetValue(now, out value) || !dic.TryGetValue(before, out value)) return false;

        dic.Remove(before);
        dic.Add(now, value);
        return true;
    }

    public static bool IsNullOrEmpty<T, TG>(this Dictionary<T, TG> dic)
    {
        return dic == null || dic.Count == 0;
    }

    #endregion

    #region String Editing

    public static string FirstLine(this string str)
    {
        return new StringReader(str).ReadLine();
    }

    public static string ToPegiStringType(this Type type)
    {
        return type.ToString().SimplifyTypeName();
    }

    public static string SimplifyTypeName(this string name)
    {
        var ind = Mathf.Max(name.LastIndexOf(".", StringComparison.Ordinal),
            name.LastIndexOf("+", StringComparison.Ordinal));
        return ind == -1 || ind > name.Length - 5 ? name : name.Substring(ind + 1);
    }

    public static string SimplifyDirectory(this string name)
    {
        var ind = name.LastIndexOf("/", StringComparison.Ordinal);
        return ind == -1 || ind > name.Length - 2 ? name : name.Substring(ind + 1);
    }

    public static string ToElipsisString(this string text, int maxLength)
    {
        if (text == null)
            return "null";

        var index = text.IndexOf(Environment.NewLine);

        if (index > 10)
            text = text.Substring(0, index);

        if (text.Length < maxLength + 3)
            return text;

        return text.Substring(0, maxLength) + "…";
    }

    public static bool SameAs(this string s, string other)
    {
        return s?.Equals(other) ?? other == null;
    }

    public static bool IsSubstringOf(this string text, string biggerText, RegexOptions opt = RegexOptions.IgnoreCase)
    {
        return Regex.IsMatch(biggerText, text, opt);
    }

    public static bool AreSubstringsOf(this string search, string name, RegexOptions opt = RegexOptions.IgnoreCase)
    {
        if (search.Length == 0)
            return true;

        if (!search.Contains(" ")) return search.IsSubstringOf(name);

        var segments = search.Split(' ');

        return segments.All(t => t.IsSubstringOf(name, opt));
    }

    public static string RemoveAssetsPart(this string s)
    {
        var ind = s.IndexOf("Assets", StringComparison.Ordinal);

        if (ind == 0 || ind == 1) return s.Substring(6 + ind);

        return ind > 1 ? s.Substring(0, ind) : s;
    }

    public static string RemoveFirst(this string name, int index)
    {
        return name.Substring(index, name.Length - index);
    }

    public static bool IsIncludedIn(this string sub, string big)
    {
        return Regex.IsMatch(big, sub, RegexOptions.IgnoreCase);
    }

    public static int FindMostSimilarFrom(this string s, string[] t)
    {
        var mostSimilar = -1;
        var distance = 999;
        for (var i = 0; i < t.Length; i++)
        {
            var newDistance = s.LevenshteinDistance(t[i]);
            if (newDistance >= distance) continue;
            mostSimilar = i;
            distance = newDistance;
        }

        return mostSimilar;
    }

    private static int LevenshteinDistance(this string s, string t)
    {
        if (s == null || t == null)
        {
            Debug.Log("Compared string is null: " + (s == null) + " " + (t == null));
            return 999;
        }

        if (s.Equals(t))
            return 0;

        var n = s.Length;
        var m = t.Length;
        var d = new int[n + 1, m + 1];

        // Step 1
        if (n == 0)
            return m;


        if (m == 0)
            return n;


        // Step 2
        for (var i = 0; i <= n; d[i, 0] = i++)
        {
        }

        for (var j = 0; j <= m; d[0, j] = j++)
        {
        }

        // Step 3
        for (var i = 1; i <= n; i++)
        for (var j = 1; j <= m; j++)
        {
            var cost = t[j - 1] == s[i - 1] ? 0 : 1;

            d[i, j] = Math.Min(
                Math.Min(d[i - 1, j] + 1, d[i, j - 1] + 1),
                d[i - 1, j - 1] + cost);
        }

        return d[n, m];
    }

    public static bool IsNullOrEmpty(this string s)
    {
        return string.IsNullOrEmpty(s);
    }

    #endregion
}