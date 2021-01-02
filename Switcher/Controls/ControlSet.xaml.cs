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

namespace Switcher
{
    /// <summary>
    /// Interaction logic for ControlSet.xaml
    /// </summary>
    public partial class ControlSet : UserControl
    {

        SolidColorBrush fillNormal = new SolidColorBrush(Color.FromRgb(200, 0, 0));
        SolidColorBrush fillHover = new SolidColorBrush(Color.FromRgb(150, 0, 0));

        public ControlSet()
        {
            InitializeComponent();
            CloseCircle.Fill = fillNormal;
        }

        private void CloseCircle_MouseEnter(object sender, MouseEventArgs e)
        {
            CloseCircle.Fill = fillHover;
        }

        private void CloseCircle_MouseLeave(object sender, MouseEventArgs e)
        {
            CloseCircle.Fill = fillNormal;
        }
    }
}
