namespace CommonLibrary.Math
{
    public class Matrix<T>
    {
        private T[][] _Data;

        public Matrix(int rows, int columns)
        {
            Rows = rows;
            Columns = columns;
        }

        public int Columns
        {
            get;
            private set;
        }

        public int Rows
        {
            get;
            private set;
        }

        public T this[int row, int column]
        {
            get
            {
                return _Data[row][column];
            }
            set
            {
                _Data[row][column] = value;
            }
        }
    }
}