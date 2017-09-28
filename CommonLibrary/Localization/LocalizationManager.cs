using CommonLibrary.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace CommonLibrary.Localization
{
    public class LocalizationManager
    {
        private static LocalizationManager _Current;
        private string Language;
        private List<LocalizationString> Strings;

        private LocalizationManager(List<LocalizationString> strings, string language)
        {
            Strings = strings;
            Language = language;
        }

        public static LocalizationManager Current
        {
            get
            {
                if (_Current == null)
                {
                    throw new Exception("Not initialized");
                }

                return _Current;
            }
        }

        public static void Initialize(List<LocalizationString> strings, string language)
        {
            _Current = new LocalizationManager(strings, language);
        }

        public void LocalizeControl(Control control)
        {
            control.Text = LocalizeString(control.Tag as string) ?? control.Text;

            LocalizeControls(control.Controls);
        }

        public void LocalizeForm(Form form)
        {
            form.Text = LocalizeString(form.Tag as string) ?? form.Text;

            LocalizeControls(form.Controls);
        }

        public string LocalizeString(Enum eVal)
        {
            string value = null;

            var attributes = eVal.GetType().GetMember(eVal.ToString()).First().GetCustomAttributes(typeof(LocalizeValueAttribute), false);

            if (attributes != null && attributes.Length > 0)
            {
                LocalizeValueAttribute att = attributes[0] as LocalizeValueAttribute;

                if (att != null)
                {
                    value = LocalizeString(att.Key);
                }
            }

            return value;
        }

        public string LocalizeString(string key)
        {
            string value = null;

            if (!key.IsNullOrEmptyTrim())
            {
                value = string.Empty;

                // the key can be comma separated
                string[] keyValues = key.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                foreach (string sepKey in keyValues)
                {
                    LocalizationString str = Strings.SingleOrDefault(s => s.Key.EqualsIgnoreCase(sepKey));

                    if (str != null)
                    {
                        value += str.Value;
                    }
                    else
                    {
                        value += sepKey;
                    }
                }
            }

            return value;
        }

        private void LocalizeControls(Control.ControlCollection controlCollection)
        {
            if (controlCollection != null)
            {
                foreach (Control control in controlCollection)
                {
                    if (control is ToolStrip)
                    {
                        LocalizeMenuItems(((ToolStrip)control).Items);
                    }
                    else
                    {
                        LocalizeControls(control.Controls);

                        if (control is ILocalizable)
                        {
                            // only localize a user control once
                            if (!((ILocalizable)control).Localized)
                            {
                                control.Text = LocalizeString(control.Tag as string) ?? control.Text;
                            }
                        }
                        else
                        {
                            control.Text = LocalizeString(control.Tag as string) ?? control.Text;
                        }
                    }
                }
            }
        }

        private void LocalizeMenuItems(ToolStripItemCollection toolStripItemCollection)
        {
            foreach (ToolStripItem item in toolStripItemCollection)
            {
                if (item is ToolStripDropDownItem)
                {
                    LocalizeMenuItems(((ToolStripMenuItem)item).DropDownItems);
                }
                else if (item is ToolStripMenuItem)
                {
                    LocalizeMenuItems(((ToolStripDropDownItem)item).DropDownItems);
                }

                item.Text = LocalizeString(item.Tag as string);
            }
        }
    }
}