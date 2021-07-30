using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MyTeiris.ViewModels;

namespace MyTeiris.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            DataContext = new MainWindowViewModel();
            InitializeComponent();
        }

        private void Start_Click(object sender, RoutedEventArgs e)
        {
            ((MainWindowViewModel)DataContext).startGame();
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            MainWindowViewModel vm = (MainWindowViewModel)DataContext;
            if (!vm.isPlaying) return;
            switch (e.Key)
            {
                case Key.Left:
                    vm.MoveLeft();
                    break;
                case Key.Right:
                    vm.MoveRight();
                    break;
                case Key.Space:
                    vm.MoveDrop();
                    break;
                case Key.Down:
                    vm.MoveDown();
                    break;
                case Key.Up:
                    vm.MoveRotate();
                    break;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ((MainWindowViewModel)DataContext).pauseGame();
        }
    }
}
