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
using System.Diagnostics;

namespace Switcher
{
    /// <summary>
    /// Interaction logic for ToggleSwitch.xaml
    /// </summary>
     partial class ToggleSwitch : UserControl
    {

        public Thickness switchOffPos = new Thickness(-66, 0, 0, 0);
        public Thickness switchOnPos = new Thickness(0, 0, -66, 0);
        public SolidColorBrush switchOffFill = new SolidColorBrush(Color.FromRgb(100, 100, 100));
        public SolidColorBrush switchOnFill = new SolidColorBrush(Color.FromRgb(0, 255, 121));

        private bool switchOn = false;

        public bool SwitchOn
        {
            get { return switchOn; }
            set { switchOn = value; }
        }

        public ToggleSwitch()
        {
            InitializeComponent();

            switchRail.Fill = switchOffFill;
            switchSlider.Margin = switchOffPos;
            switchOn = false;
        }

        private void switchSlider_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!switchOn)
            {
                switchRail.Fill = switchOnFill;
                switchSlider.Margin = switchOnPos;
                switchOn = true;
            }
            else
            {
                switchRail.Fill = switchOffFill;
                switchSlider.Margin = switchOffPos;
                switchOn = false;
            }

        }

        private void switchRail_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!switchOn)
            {
                switchRail.Fill = switchOnFill;
                switchSlider.Margin = switchOnPos;
                switchOn = true;
            }
            else
            {
                switchRail.Fill = switchOffFill;
                switchSlider.Margin = switchOffPos;
                switchOn = false;
            }
        }
    }
}
