using MyTeiris.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using TetrisEngine;

namespace MyTeiris.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {


        public const int MAIN_ROWS = 21;
        public const int MAIN_COLS = 12;

        Block mainBlock = null;
        Block nextBlock = null;

        Move move = new();

        Matrix matrix = new Matrix(MAIN_ROWS,MAIN_COLS);

        int scoreLine;
        int scoreHighLine;

        public bool isPlaying = false;

        System.Windows.Threading.DispatcherTimer dispatcherTimer;

        BindableTwoDArray<int> nextPan = new BindableTwoDArray<int>(4, 4);
        BindableTwoDArray<int> blockPan = new BindableTwoDArray<int>(MAIN_ROWS, MAIN_COLS);

        public BindableTwoDArray<int> NextPan { get => nextPan; set => nextPan = value; }
        public BindableTwoDArray<int> BlockPan { get => blockPan; set => blockPan = value; }
        public int ScoreLine { get => scoreLine; set { scoreLine = value; NotifyPropertyChanged("ScoreLine"); } }
        public int ScoreHighLine { get => scoreHighLine; set => scoreHighLine = value; }

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
                if (!move.ValidMove(ref mainBlock, 1, 0,ref matrix))
                {
                    AfterMove();
                }


                redraw();
            }
            else
            {
                gameover();
            }
        }

        public void AfterMove()
        {
            matrix.stackBlock(mainBlock);
            matrix.isFilledLine();

            if (matrix.Filled.Count > 0)
            {
                ScoreLine += matrix.deleteLine();

            }

            if (matrix.Filled.Count == 0)
            {
                Block cloneNextBlock = nextBlock.Clone();
                cloneNextBlock.Y = 4;
                if (move.ValidCheck(ref cloneNextBlock, ref matrix))
                {
                    getNextBlock();
                }
                else
                {
                    gameover();
                }
            }



        }

        public void pauseGame()
        {

        }

        private async void gameover()
        {
            await Task.Run(() =>
            {
                StopTimer();
                isPlaying = false;
                for (int i = MAIN_ROWS - 1; i >= 0; i--)
                {
                    for (int j = 0; j < MAIN_COLS; j++)
                    {
                        matrix.Array[i][j] = 8;
                    }
                    DrawBoard();
                    Thread.Sleep(100);
                }
            });
        }

        private void initGame()
        {
            matrix.resetMatrix();
            ScoreLine = 0;
            getNextBlock();
            redraw();
        }

        private void DrawBlock(Block block, BindableTwoDArray<int> pan)
        {
            for (int i = 0; i < block.Shape.GetLength(0); i++)
            {
                for (int j = 0; j < block.Shape.GetLength(1); j++)
                {
                    if (block.Shape[i, j] > 0)
                    {
                        pan[i + block.X, j + block.Y] = block.Shape[i, j];
                    }
                }
            }
            pan.NotifyBlockChange();
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
                        matrix.Array[i][j] = -1;
                    }
                    else
                    {
                        blockPan[i, j] = Constants.blockBackground;
                        if (matrix.Array[i][j] > 0)
                        {
                            blockPan[i, j] = matrix.Array[i][j];
                        }

                    }
                }
            }

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    nextPan[i, j] = 0;
                }
            }
            nextPan.NotifyBlockChange();
            blockPan.NotifyBlockChange();
        }

        public void redraw()
        {
            DrawBoard();
            DrawBlock(mainBlock, blockPan);
            DrawBlock(nextBlock, nextPan);
        }

        public void getNextBlock()
        {
            mainBlock = nextBlock ?? new Block();
            mainBlock.Y = 4;
            nextBlock = new Block();
            nextBlock.Y = 1;
            nextBlock.X = 1;
            if (nextBlock.Shape[1, 1] == 5)
                nextBlock.X = 1;
            if (nextBlock.Shape[1, 1] == 4)
            {
                nextBlock.X = 0;
                nextBlock.Y = 0;
            }
        }

        internal void MoveLeft()
        {
            move.ValidMove(ref mainBlock, 0, -1, ref matrix);
            redraw();
        }

        internal void MoveRight()
        {
            move.ValidMove(ref mainBlock, 0, 1, ref matrix);
            redraw();
        }

        internal void MoveDrop()
        {
            while (move.ValidMove(ref mainBlock, 1, 0, ref matrix)) ;
            AfterMove();
            StopTimer();
            StartTimer();
            redraw();
        }

        internal void MoveDown()
        {
            move.ValidMove(ref mainBlock, 1, 0, ref matrix);
            StopTimer();
            StartTimer();
            redraw();
        }

        internal void MoveRotate()
        {
            move.ValidRotate(ref mainBlock,ref matrix); ;
            redraw();
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