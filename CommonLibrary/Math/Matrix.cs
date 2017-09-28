using System;

namespace CommonLibrary.Math
{
    public class Matrix
    {
        private double[,] _Elements;

        public Matrix(int rows, int columns)
        {
            _Elements = new double[rows, columns];
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

        public double this[int row, int column]
        {
            get
            {
                return _Elements[row, column];
            }
            set
            {
                _Elements[row, column] = value;
            }
        }

        #region Operators

        public static Matrix operator -(Matrix a, Matrix b)
        {
            if (a.Rows != b.Rows && a.Columns != b.Columns)
            {
                throw new ArithmeticException("Rows and/or Columns are not equal");
            }

            Matrix result = new Matrix(a.Rows, a.Columns);

            for (int row = 0; row < a.Rows; row++)
            {
                for (int col = 0; col < a.Columns; col++)
                {
                    result[row, col] = a[row, col] - b[row, col];
                }
            }

            return result;
        }

        public static Matrix operator *(Matrix a, Matrix b)
        {
            if (a.Columns != b.Rows)
            {
                throw new ArithmeticException("Invalid sizes for multiplication");
            }

            Matrix result = new Matrix(a.Rows, b.Columns);

            for (int r = 0; r < result.Rows; r++)
            {
                for (int c = 0; c < result.Columns; c++)
                {
                    result[r, c] = 0;
                    for (int z = 0; z < a.Columns; z++)
                    {
                        result[r, c] += a[r, z] * b[z, c];
                    }
                }
            }
            return result;
        }

        public static Matrix operator *(double scalar, Matrix matrix)
        {
            {
                Matrix result = new Matrix(matrix.Rows, matrix.Columns);

                for (int r = 0; r < result.Rows; r++)
                {
                    for (int c = 0; c < result.Columns; c++)
                    {
                        result[r, c] = scalar * matrix[r, c];
                    }
                }
                return result;
            }
        }

        public static Matrix operator *(Matrix mat, double scalar)
        {
            return scalar * mat;
        }

        public static Matrix operator +(Matrix a, Matrix b)
        {
            if (a.Rows != b.Rows && a.Columns != b.Columns)
            {
                throw new ArithmeticException("Rows and/or Columns are not equal");
            }

            Matrix result = new Matrix(a.Rows, a.Columns);

            for (int row = 0; row < a.Rows; row++)
            {
                for (int col = 0; col < a.Columns; col++)
                {
                    result[row, col] = a[row, col] + b[row, col];
                }
            }

            return result;
        }

        #endregion Operators
    }
}