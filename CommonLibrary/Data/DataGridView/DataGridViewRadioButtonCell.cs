using System;
using System.Drawing;
using System.Windows.Forms;

namespace CommonLibrary.Data.DataGridView
{
    public class DataGridViewRadioButtonCell :
        DataGridViewComboBoxCell, IDataGridViewEditingCell
    {
        #region IDataGridViewEditingCell Members

        public object EditingCellFormattedValue
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public bool EditingCellValueChanged
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public object GetEditingCellFormattedValue(DataGridViewDataErrorContexts context)
        {
            throw new NotImplementedException();
        }

        public void PrepareEditingCellForEdit(bool selectAll)
        {
            throw new NotImplementedException();
        }

        #endregion IDataGridViewEditingCell Members

        public override Type EditType
        {
            get
            {
                return null;
            }
        }

        public override object Clone()
        {
            DataGridViewRadioButtonCell cell = base.Clone() as DataGridViewRadioButtonCell;

            return cell;
        }

        protected override bool ContentClickUnsharesRow(DataGridViewCellEventArgs e)
        {
            Point ptCurrentCell = this.DataGridView.CurrentCellAddress;

            return ptCurrentCell.X == this.ColumnIndex &&
                ptCurrentCell.Y == e.RowIndex &&
                this.DataGridView.IsCurrentCellInEditMode;
        }
    }
}