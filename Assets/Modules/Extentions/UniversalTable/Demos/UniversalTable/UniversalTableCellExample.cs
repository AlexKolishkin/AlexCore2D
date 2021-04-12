using Core.UniversalTable;
using UnityEngine.UI;

namespace Modules.Table.Demos.UniversalTable
{
	public class UniversalTableCellExample : UniversalTableCellView<UniversalTableDemo.Data>
    {
        public Text SomeText;
        public Button Button;
        public override void Fill(UniversalTableDemo.Data data)
        {
            base.Fill(data);
            SomeText.text = data.someText;
        }
    }
}
