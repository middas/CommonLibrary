﻿using System;
using System.Windows.Forms;
using CommonLibrary.Native;

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
                DateTimePickerMain.Value = value.HasValue ? value.Value : DateTime.Now;

                if (!value.HasValue)
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

        private void DateTimePickerMain_CloseUp(object sender, EventArgs e)
        {
            if (!DesignMode)
            {
                Date = DateTimePickerMain.Value;
            }
        }
    }
}