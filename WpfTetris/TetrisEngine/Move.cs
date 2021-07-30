using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TetrisEngine
{
    public class Move
    {
        public void move(ref Block block, int x, int y)
        {
            block.X += x;
            block.Y += y;
        }

        public void rotate(ref Block block)
        {
            for (int i = 0; i < block.Shape.GetLength(0); i++)
            {
                for (int j = 0; j < i; j++)
                {
                    int temp = block.Shape[j, i];
                    block.Shape[j, i] = block.Shape[i, j];
                    block.Shape[i, j] = temp;
                }
            }

            for (int i = 0; i < block.Shape.GetLength(0); i++)
            {
                for (int j = 0; j <= block.Shape.GetLength(1) / 2; j++)
                {
                    int temp = block.Shape[i, j];
                    block.Shape[i, j] = block.Shape[i, block.Shape.GetLength(1) - j - 1];
                    block.Shape[i, block.Shape.GetLength(1) - j - 1] = temp;

                }
            }

        }

        public bool ValidMove(ref Block block, int x, int y, ref Matrix matrix)
        {
            Block copyBlock = block.Clone();
            move(ref copyBlock, x, y);
            if (ValidCheck(ref copyBlock, ref matrix))
            {
                move(ref block, x, y);
                return true;
            }
            else
            {
                return false;
            }

        }

        public bool ValidCheck(ref Block block, ref Matrix matrix)
        {
            bool isValid = true;

            for (int i = 0; i < block.Shape.GetLength(0); i++)
            {
                for (int j = 0; j < block.Shape.GetLength(1); j++)
                {
                    if (block.Shape[i, j] > 0)
                    {
                        if (block.X + i < 0 || block.Y + j < 1 ||
                            block.X + i > matrix.Row - 2 || block.Y + j >= matrix.Col - 1 ||
                            matrix.Array[block.X + i][block.Y + j] > 0)
                        {
                            isValid = false;
                        }
                    }
                }
            }
            return isValid;
        }

        public bool ValidRotate(ref Block block, ref Matrix matrix)
        {
            Block copyBlock = block.Clone();
            rotate(ref copyBlock);
            if (ValidCheck(ref copyBlock, ref matrix))
            {
                rotate(ref block);
                return true;
            }
            else
            {
                return false;
            }

        }

    }
}
