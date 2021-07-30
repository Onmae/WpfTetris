using System;
using System.ComponentModel;
using System.Windows.Data;

namespace TetrisEngine
{
    public class Block : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        int[][,] tetrisBlock = new int[][,]
        {
                new int[,]{{0, 1, 1}, {1,1,0},{0,0,0} },
                new int[,]{{2,0,0},{2,2,2},{0,0,0} },
                new int[,]{{0,3,0},{3,3,3},{0,0,0} },
                new int[,]{{0,4,0,0},{0,4,0,0 },{0,4,0,0 },{0,4,0,0} },
                new int[,]{{5,5 },{5,5 } },
                new int[,]{{0,0,6},{6,6,6 },{0,0,0, } },
                new int[,]{{7,7,0},{0,7,7 },{0,0,0 } }
        };

        int x = 0;
        int y = 0;
        int[,] shape;

        public Block()
        {
            shape = RandomBlock();
        }

        public int X { get => x; set => x = value; }
        public int Y { get => y; set => y = value; }
        public int[,] Shape { get => shape; set => shape = value; }

        public Block Clone()
        {
            Block newBlock = new Block();
            newBlock.x = this.x;
            newBlock.y = this.y;
            newBlock.shape = (int[,])this.shape.Clone();

            return newBlock;
        }

        public void Notify() => NotifyPropertyChanged(Binding.IndexerName);

        private void NotifyPropertyChanged(string propertyName)
        {

            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private int[,] RandomBlock()
        {
            return tetrisBlock[new Random().Next(0, 7)].Clone() as int[,];
        }
    }
}