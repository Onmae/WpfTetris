using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using WpfApp1.Models;

namespace WpfApp1.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {

        const int MAIN_ROWS = 21;
        const int MAIN_COLS = 12;

        Block mainBlock = null;
        Block nextBlock = null;

        int[,] boardMatrix = new int[MAIN_ROWS, MAIN_COLS];

        int ScoreLine;
        int ScoreHighLine;

        public bool isPlaying = false;


        System.Windows.Threading.DispatcherTimer dispatcherTimer;

        int[,] MATRIX = new int[MAIN_ROWS + 1, MAIN_COLS + 2];

        BindableTwoDArray<int> nextPan = new BindableTwoDArray<int>(4, 4);
        BindableTwoDArray<int> blockPan = new BindableTwoDArray<int>(MAIN_ROWS+1, MAIN_COLS+2);
        List<int> filled = new List<int>();

        public int ScoreLine1 { get => ScoreLine; set => ScoreLine = value; }



        public int ScoreHighLine1 { get => ScoreHighLine; set => ScoreHighLine = value; }
        public BindableTwoDArray<int> NextPan { get => nextPan; set => nextPan = value; }
        public BindableTwoDArray<int> BlockPan { get => blockPan; set => blockPan = value; }

        public MainWindowViewModel()
        {
            dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);

        }

        internal void startGame()
        {
            if (isPlaying == false)
            {
                initGame();
                isPlaying = true;
                StartTimer();
            }
            else
            {
                gameover();
            }
        }
        private void StartTimer()
        {

            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 1000);
            dispatcherTimer.Start();
        }

        private void StopTimer()
        {
            dispatcherTimer.Stop();
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            if (isPlaying)
            {
                if (!ValidMove(ref mainBlock, 1, 0))
                {
                    AfterMove();
                }
                DrawBoard();
                DrawBlock(mainBlock);
            }
            else
            {
                gameover();
            }
        }

        private void AfterMove()
        {
            stackBlock(mainBlock);
            isFilledLine();

            getNextBlock();

        }

        private void deleteLine()
        {

        }

        private void isFilledLine()
        {
            for(int i=0; i< MATRIX.GetLength(0); i++)
            {
                bool isFill = true;
                for(int j=0; j< MATRIX.GetLength(1); j++)
                {
                    if(MATRIX[i,j] <= 0)
                    {
                        isFill = false;
                    }

                    if(isFill == true)
                    {
                        filled.Add(j);
                    }
                }
            }
        }

        private void stackBlock(Block block)
        {
                for (int i = 0; i < block.Shape.GetLength(0); i++)
                {
                    for (int j = 0; j < block.Shape.GetLength(1); j++)
                    {
                        if (block.Shape[i, j] > 0)
                        {
                            MATRIX[i+block.X,j+block.Y] = block.Shape[i, j];
                        }
                    }
                }
        }

        private void gameover()
        {
            throw new NotImplementedException();
        }

        private void initGame()
        {
            getNextBlock();
            DrawBoard();
            DrawBlock(mainBlock);
        }
        private void DrawBlock(Block block)
        {
            for(int i=0; i<block.Shape.GetLength(0); i++)
            {
                for(int j=0; j<block.Shape.GetLength(1); j++)
                {
                    if(block.Shape[i,j] > 0)
                    {

                        blockPan[i + block.X, j + block.Y] = block.Shape[i, j];
                    }
                }
            }
            blockPan.NotifyBlockChange();
        }

        private void DrawBoard()
        {

            for (int i = 0; i < 21; i++)
            {
                for (int j = 0; j < 12; j++)
                {
                    if (j == 0 || j == 11 || i == 20)
                    {
                        blockPan[i, j] = Constants.blockWall;
                    }
                    else
                    {
                        blockPan[i, j] = Constants.blockBackground;
                        if (MATRIX[i,j] > 0) {
                            blockPan[i, j] = MATRIX[i, j];
            }

                    }
                }
            }
            blockPan.NotifyBlockChange();
        }


        public void move(ref Block block, int x, int y)
        {
            block.X += x;
            block.Y += y;
        }

        public void rotate(ref Block block)
        {
            for(int i=0; i<block.Shape.GetLength(0); i++)
            {
                for(int j=0; j<i; j++)
                {
                    int temp = block.Shape[j, i];
                    block.Shape[j, i] = block.Shape[i, j];
                    block.Shape[i, j] = temp;
                }
            }

            for(int i=0; i<block.Shape.GetLength(0); i++)
            {
                for(int j=0; j <= block.Shape.GetLength(1)/2; j++)
                {
                    int temp = block.Shape[i, j];
                    block.Shape[i, j] = block.Shape[i, block.Shape.GetLength(1) -j -1];
                    block.Shape[i, block.Shape.GetLength(1) - j -1] = temp;

                }
            }

        }


        public bool ValidMove(ref Block block,int x,int y)
        {
            Block copyBlock = (Block)block.Clone();
            move(ref copyBlock, x, y);
            if (ValidCheck(ref copyBlock))
            {
                move(ref block, x, y);
                DrawBoard();
                DrawBlock(block);
                return true;
            }else
            {
                return false;
            }

        }

        public bool ValidCheck(ref Block block)
        {
            bool isValid = true;

            for(int i=0; i<block.Shape.GetLength(0); i++)
            {
                for(int j=0; j< block.Shape.GetLength(1); j++)
                {
                    if(block.Shape[i,j] > 0) {
                        if(block.X+i < 0 || block.Y+j < 1 ||
                            block.X+i > MAIN_ROWS-2 || block.Y+j >= MAIN_COLS - 1 ||
                            MATRIX[block.X+i,block.Y+j] > 0)
                        {
                            isValid = false;
                        }
                    }
                }
            }
            return isValid;
        }

        public bool ValidRotate(ref Block block)
        {
            Block copyBlock = (Block)block.Clone();
            rotate(ref copyBlock);
            if (ValidCheck(ref copyBlock))
            {
                rotate(ref block);
                DrawBoard();
                DrawBlock(block);
                return true;
            }
            else
            {
                return false;
            }

        }

        internal void MoveLeft()
        {
            ValidMove(ref mainBlock, 0, -1);
        }

        internal void MoveRight()
        {
            ValidMove(ref mainBlock, 0, 1);
        }

        internal void MoveDrop()
        {
            while(ValidMove(ref mainBlock, 1, 0));
            AfterMove();
            DrawBoard();
            DrawBlock(mainBlock);
        }

        internal void MoveDown()
        {
            ValidMove(ref mainBlock, 1, 0);
        }

        internal void MoveRotate()
        {
            ValidRotate(ref mainBlock);
        }

        public static int[,] RandomBlock()
        {
            int[][,] tetrisBlock = new int[][,]
            {
                new int[,]{{0,1,1},{1,1,0},{0,0,0} },
                new int[,]{{2,0,0},{2,2,2},{0,0,0} },
                new int[,]{{0,3,0},{3,3,3},{0,0,0} },
                new int[,]{{0,0,4,0},{0,0,4,0 },{0,0,4,0 },{0,0,4,0} },
                new int[,]{{5,5 },{5,5 } },
                new int[,]{{0,0,6},{6,6,6 },{0,0,0, } },
                new int[,]{{7,7,0},{0,7,7 },{0,0,0 } }
            };

            return tetrisBlock[new Random().Next(0, 7)];
        }

        public class Block : INotifyPropertyChanged, ICloneable
        {
            int x=0;
            int y=0;
            int[,] shape = RandomBlock();

            public int X { get => x; set => x = value; }
            public int Y { get => y; set => y = value; }
            public int[,] Shape { get => shape; set => shape = value; }

            public object Clone()
            {
                Block block = new Block();
                block.x = this.x;
                block.y = this.y;
                block.shape = this.shape;

                return block;

            }

            public event PropertyChangedEventHandler PropertyChanged;

            public void Notify() => NotifyPropertyChanged(Binding.IndexerName);

            private void NotifyPropertyChanged(string propertyName)
            {
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
                }
            }


        }

        public void getNextBlock()
        {
            mainBlock = nextBlock ?? new Block();
            mainBlock.Y = 4;
            nextBlock = new Block();
        }

        #region notifyproperty
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion
    }
}
