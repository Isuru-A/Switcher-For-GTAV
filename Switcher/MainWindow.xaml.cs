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
using System.Windows.Shapes;
using System.Diagnostics;
using System.IO;

namespace Switcher
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private SolidColorBrush textColorModded = new SolidColorBrush(Color.FromRgb(200, 0, 0));
        private SolidColorBrush textColorUnmodded = new SolidColorBrush(Color.FromRgb(0, 200, 0));
        private SolidColorBrush textColorWarning = new SolidColorBrush(Color.FromRgb(255, 200, 0));
        private SolidColorBrush textColorDefault = new SolidColorBrush(Color.FromRgb(0, 199, 136));

        private string textModded = "Modded";
        private string textUnmodded = "Not Modded";

        public MainWindow()
        {
            InitializeComponent();

            //Creates A Switcher Directory in Local Appdata
            var localAppDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            var localPath = System.IO.Path.Combine(localAppDataPath, @"Switcher");
            if (!System.IO.Directory.Exists(localPath))
            {
                System.IO.Directory.CreateDirectory(localPath);
            }

            //Initiates Other Needed Functions
            try
            {
                SwitcherSettings.readLauncherSettings();
                SwitcherSettings.readSettings();
                SwitcherCheck.switcherModCheck_Epic(SwitcherCore.LocationModded);
                updateStatus();
            }
            catch (Exception)
            {
                //Console.WriteLine("Something Went Wrong");
                MessageBox.Show("An unknown issue occured, and Switcher was not able to resolve it!", "Something Went Wrong");
                throw;
            }
            
            //Updates UI With User Settings and Preferences
            if (SwitcherCore.LauncherSelect == "EpicGames")
            {
                LauncherStatus.Text = "Epic Games";
            }
            else if (SwitcherCore.LauncherSelect == "Steam")
            {
                LauncherStatus.Text = "Steam";
            }

            //Checks If Current Install Is Modded Or Clean And Adjusts InstallSwitch's Properties Accordingly
            if (!SwitcherCheck.SwitcherPresence)
            {
                InstallSwitch.switchRail.Fill = InstallSwitch.switchOffFill;
                InstallSwitch.switchSlider.Margin = InstallSwitch.switchOffPos;
                InstallSwitch.SwitchOn = false;
            }
            else if(SwitcherCheck.SwitcherPresence)
            {
                InstallSwitch.switchRail.Fill = InstallSwitch.switchOnFill;
                InstallSwitch.switchSlider.Margin = InstallSwitch.switchOnPos;
                InstallSwitch.SwitchOn = true;
            }

            //Checks If Current Install Is Modded Or Clean And Adjusts Status Text Accordingly
            if (!SwitcherCheck.SwitcherPresence)
            {
                textInstallStatus.Text = textUnmodded;
                textInstallStatus.Foreground = textColorUnmodded;
                InstallStatusContainer.Stroke = textColorUnmodded;
            }
            else
            {
                textInstallStatus.Text = textModded;
                textInstallStatus.Foreground = textColorModded;
                InstallStatusContainer.Stroke = textColorModded;
            }

        }

        //Updates Status Output With Relevant Information
        private void updateStatus()
        {
            var localAppDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            var localPath = System.IO.Path.Combine(localAppDataPath, @"Switcher");

            if (!(System.IO.File.Exists(System.IO.Path.Combine(localPath, "settings.xml"))) || !(System.IO.File.Exists(System.IO.Path.Combine(localPath, "launcher.xml"))))
            {
                SwitcherStatusTitle.Text = "Settings File(s) Not Found";
                SwitcherStatusTitle.Foreground = textColorModded;
                SwitcherStatusBody.Text = "Please Setup Switcher Using Settings (...)";
                SwitcherStatusBody.Foreground = textColorDefault;
            }
            else
            {
                if (SwitcherCore.LauncherSelect == "EpicGames")
                {
                    if (!(System.IO.File.Exists(SwitcherCore.ManifestLocation)))
                    {
                        SwitcherStatusTitle.Text = "Unable To Locate Files";
                        SwitcherStatusTitle.Foreground = textColorModded;
                        SwitcherStatusBody.Text = "Please Define Install Paths In Settings Before Using Switcher";
                        SwitcherStatusBody.Foreground = textColorDefault;
                    }
                    else if (SwitcherCheck.SyncError)
                    {
                        SwitcherStatusTitle.Text = "Manifest Is Not Synced With Launcher";
                        SwitcherStatusTitle.Foreground = textColorWarning;
                        SwitcherStatusBody.Text = "Please Resync Manifest And Launcher In Settings Before Using Switcher";
                        SwitcherStatusBody.Foreground = textColorDefault;
                    }
                    else if (!SwitcherCheck.SyncError && (System.IO.File.Exists(SwitcherCore.ManifestLocation)) && (System.IO.File.Exists(SwitcherCore.LauncherInstalledLocation)))
                    {
                        SwitcherStatusTitle.Text = "Switcher Is Ready To Use";
                        SwitcherStatusTitle.Foreground = textColorUnmodded;
                        SwitcherStatusBody.Text = "Restart launcher after changing installs";
                        SwitcherStatusBody.Foreground = textColorDefault;
                    }
                    else
                    {
                        SwitcherStatusTitle.Text = "Something Went Wrong";
                        SwitcherStatusTitle.Foreground = textColorModded;
                        SwitcherStatusBody.Text = "Switcher Does Not Know What Went Wrong. Please Try Reinstalling Switcher Or Restarting Launcher";
                        SwitcherStatusBody.Foreground = textColorDefault;
                    }
                }
                else if (SwitcherCore.LauncherSelect == "Steam")
                {
                    SwitcherStatusTitle.Text = "Unsupported Launcher";
                    SwitcherStatusTitle.Foreground = textColorModded;
                    SwitcherStatusBody.Text = "Steam is currently not supported by switcher";
                    SwitcherStatusBody.Foreground = textColorDefault;
                }
            }

        }

        private void ControlSet_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        private void SettingsButton_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            SwitcherSettings switcherSettings = new SwitcherSettings();
            switcherSettings.Show();
            this.Close();
        }

        private void InstallSwitch_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //Checks InstallSwitch Status After Each Click And Execeutes Actions Accordingly
            if (SwitcherCore.LauncherSelect == "EpicGames")
            {
                if ((!SwitcherCheck.SyncError))
                {
                    if (!InstallSwitch.SwitchOn)
                    {
                        textInstallStatus.Text = textUnmodded;
                        textInstallStatus.Foreground = textColorUnmodded;
                        InstallStatusContainer.Stroke = textColorUnmodded;

                        SwitcherCore.switchInstall_Epic("switchToClean");
                    }
                    else
                    {
                        textInstallStatus.Text = textModded;
                        textInstallStatus.Foreground = textColorModded;
                        InstallStatusContainer.Stroke = textColorModded;

                        SwitcherCore.switchInstall_Epic("switchToModded");
                    }
                }
                else
                {
                    MessageBox.Show("Manifest and Launcher not synced!", "Something Went Wrong");
                }
            }
            else if (SwitcherCore.LauncherSelect == "Steam")
            {
                if (!InstallSwitch.SwitchOn)
                {
                    textInstallStatus.Text = textUnmodded;
                    textInstallStatus.Foreground = textColorUnmodded;
                    InstallStatusContainer.Stroke = textColorUnmodded;
                }
                else
                {
                    textInstallStatus.Text = textModded;
                    textInstallStatus.Foreground = textColorModded;
                    InstallStatusContainer.Stroke = textColorModded;
                }
            }
            
        }

        private void RestartLauncher_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //Restarts Selected Launcher
            string launcher = "name";

            if (System.IO.File.Exists(SwitcherCore.LauncherLocation))
            {
                if (SwitcherCore.LauncherSelect == "EpicGames")
                {
                    launcher = "EpicGamesLauncher";
                }
                else if (SwitcherCore.LauncherSelect == "Steam")
                {
                    launcher = "Steam";
                }

                Process[] processes = Process.GetProcessesByName(launcher);

                foreach (var process in processes)
                {
                    process.Kill();
                    process.WaitForExit();
                }

                Process.Start(SwitcherCore.LauncherLocation);
            }
            else
            {
                MessageBox.Show("Unable to find launcher. Please ensure correct launcher path is defined in settings", "Something Went Wrong");
            }
            
        }

        private void Header_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
    } 
}
