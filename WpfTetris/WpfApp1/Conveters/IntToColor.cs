using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;
using MyTeiris.Models;

namespace MyTeiris.Conveters
{
    class IntToColor : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int type = (int)value;

            switch (type)
            {
                case Constants.blockBackground:
                    if(parameter == null)
                        return new SolidColorBrush(Colors.Transparent);
                    return new SolidColorBrush(Colors.Gray);
                case Constants.blockWall:
                    return new SolidColorBrush(Colors.Ivory);
                case Constants.block_1:
                    return new SolidColorBrush(Colors.Red);
                case Constants.block_2:
                    return new SolidColorBrush(Colors.Orange);
                case Constants.block_3:
                    return new SolidColorBrush(Colors.Yellow);
                case Constants.block_4:
                    return new SolidColorBrush(Colors.Green);
                case Constants.block_5:
                    return new SolidColorBrush(Colors.Blue);
                case Constants.block_6:
                    return new SolidColorBrush(Colors.Magenta);
                case Constants.block_7:
                    return new SolidColorBrush(Colors.DeepPink);
                case Constants.gameover:
                    return new SolidColorBrush(Colors.Red);
            }
            return new SolidColorBrush(Colors.Gray);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
    