using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CommonLibrary.Native;
using System.Data.SqlTypes;

namespace CommonLibrary.Forms
{
    public partial class NullableDateTimeSelector : UserControl
    {
        public DateTime? Date
        {
            get
            {
                DateTime? dateTime;

                if (TextBoxMain.Text.IsNullOrEmptyTrim())
                {
                    dateTime = null;
                }
                else
                {
                    dateTime = DateTimePickerMain.Value;
                }

                return dateTime;
            }
            set
            {
                DateTimePickerMain.Value = value.HasValue ? value.Value : SqlDateTime.MinValue.Value;

                if (DateTimePickerMain.Value == SqlDateTime.MinValue.Value)
                {
                    TextBoxMain.Text = string.Empty;
                }
                else
                {
                    TextBoxMain.Text = DateTimePickerMain.Text;
                }
            }
        }

        public NullableDateTimeSelector()
        {
            InitializeComponent();
        }

        private void TextBoxMain_Leave(object sender, EventArgs e)
        {
            if (TextBoxMain.Text.IsNullOrEmptyTrim())
            {
                Date = null;
            }
            else
            {
                DateTime date;

                if (DateTime.TryParse(TextBoxMain.Text, out date))
                {
                    Date = date;
                }
                else
                {
                    Date = null;
                }
            }
        }
    }
}
