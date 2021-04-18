using System.Collections.Generic;
using Core.Utils;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Modules.Table.Demos.UniversalTable
{
    public class UniversalTableDemo : MonoBehaviour
    {
        public UniversalTableCellExample CellPrefab;
        public Core.UniversalTable.UniversalTable UniversalTable;

        public Text Desc;
    
        private List<Data> _data = new List<Data>();
    
        void Start()
        {
            // example list data
            for (var i = 0; i < 1000; i++)
                _data.Add(new Data() { someText = "Cell Data Index " + i.ToString() });
        
        
            // use
            UniversalTable.Bind(_data, CellPrefab, (data, view) =>
            {
                view.Fill(data);
                view.Button.BindClick(() => { Desc.text = $"Current picked {data.someText}"; }).AddTo(view.Collector);
            });
        }


        public class Data
        {
            public string someText;
        }
    }
}
