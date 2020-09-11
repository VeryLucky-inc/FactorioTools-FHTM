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

namespace ForTests
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var _Colors = typeof(Colors)
                           .GetProperties()
                           .Select((c, i) => new
                           {
                               Color = (Color)c.GetValue(null),
                               Name = c.Name,
                               Index = i,
                               ColSpan = ColSpan(i),
                               RowSpan = RowSpan(i)
                           });

            DataContext = _Colors;

        }
        private object RowSpan(int i)
        {
            if (i == 0)
                return 2;
            if (i == 2)
                return 3;
            if (i == 7)
                return 2;
            if (i == 14)
                return 2;
            return 1;
        }

        private object ColSpan(int i)
        {
            if (i == 0)
                return 2;
            if (i == 6)
                return 3;
            if (i == 14)
                return 2;
            return 1;
        }

    }
}
