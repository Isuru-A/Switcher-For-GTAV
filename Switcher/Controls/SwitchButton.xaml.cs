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
    /// Interaction logic for SwitchButton.xaml
    /// </summary>
    public partial class SwitchButton : UserControl
    {
        private bool buttonOn = false;
        public bool ButtonOn
        {
            get { return buttonOn; }
            set { buttonOn = value; }
        }

        public SwitchButton()
        {
            InitializeComponent();
            Button.Fill = buttonOffFill;
            buttonOn = false;
        }

        public SolidColorBrush buttonOnFill = new SolidColorBrush(Color.FromRgb(0, 200, 0));
        public SolidColorBrush buttonOffFill = new SolidColorBrush(Color.FromRgb(21, 21, 21));

        private void Button_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!buttonOn)
            {
                Button.Fill = buttonOnFill;
                buttonOn = true;
            }
            else
            {
                Button.Fill = buttonOffFill;
                buttonOn = false;
            }
        }
    }
}
