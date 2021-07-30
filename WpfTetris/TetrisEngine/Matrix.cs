using System.Collections.Generic;

namespace TetrisEngine
{
    public class Matrix
    {

        List<List<int>> array = new();
        List<int> filled = new List<int>();

        int col;
        int row;

        public List<int> Filled { get => filled; set => filled = value; }
        public List<List<int>> Array { get => array; set => array = value; }
        public int Col { get => col; set => col = value; }
        public int Row { get => row; set => row = value; }

        public Matrix(int mainRow, int mainCol)
        {
            col = mainCol;
            row = mainRow;

            for (int i = 0; i < Row; i++)
            {
                array.Add(new List<int>());
                for (int j = 0; j < Col; j++)
                {
                    array[i].Add(0);
                }
            }
        }

        public void resetMatrix()
        {
            array.Clear();
            for (int i = 0; i < Row; i++)
            {
                array.Add(new List<int>());
                for (int j = 0; j < Col; j++)
                {
                    array[i].Add(0);
                }
            }
        }

        public int deleteLine()
        {
            int i = 0;
            for (i = 0; i < Filled.Count; i++)
            {
                Array.RemoveAt(Filled[i]);
                Array.Insert(0, new List<int>());
                for (int k = 0; k < Col; k++)
                    Array[0].Add(0);
            }

            Filled.Clear();

            return i;
        }

        public void isFilledLine()
        {
            for (int i = 0; i < (Array.Count - 1); i++)
            {
                bool isFill = true;
                for (int j = 0; j < Array[0].Count; j++)
                {
                    if (Array[i][j] == 0)
                    {
                        isFill = false;
                    }
                }
                if (isFill == true)
                {
                    Filled.Add(i);
                }
            }
        }

        public void stackBlock(Block block)
        {
            for (int i = 0; i < block.Shape.GetLength(0); i++)
            {
                for (int j = 0; j < block.Shape.GetLength(1); j++)
                {
                    if (block.Shape[i, j] > 0)
                    {
                        Array[i + block.X][j + block.Y] = block.Shape[i, j];
                    }
                }
            }
        }

    }
}
