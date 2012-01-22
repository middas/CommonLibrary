using System.Collections;
using System.Windows.Forms;

namespace CommonLibrary.Utilities
{
    public class PopulateControl
    {
        public static void PopulateComboBox(ref ComboBox comboBox, IEnumerable data, string displayMember, string valueMember)
        {
            comboBox.DataSource = data;
            comboBox.DisplayMember = displayMember;
            comboBox.ValueMember = valueMember;
        }
    }
}
