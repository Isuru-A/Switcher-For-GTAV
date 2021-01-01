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
using System.IO;
using WinForms = System.Windows.Forms;
using System.Xml;

namespace Switcher
{
    /// <summary>
    /// Interaction logic for SwitcherSettings.xaml
    /// </summary>
    public partial class SwitcherSettings : Window
    {
        private static string inputUnmodded = "path";
        private static string inputModded = "path";
        private static string inputLauncher = "EpicGames";
        private static string inputLauncherPath = "path";

        SolidColorBrush textColorValid = new SolidColorBrush(Color.FromRgb(0, 200, 0));
        SolidColorBrush textColorInvalid = new SolidColorBrush(Color.FromRgb(200, 0, 0));
        //SolidColorBrush textColorWarning = new SolidColorBrush(Color.FromRgb(255, 200, 0));
        private string textValid = "Valid Path";
        private string textInvalid = "Invalid Path, Unable To Tocate 'GTA5.exe'";
        //private string textWarning = "Clean Path And Modded Path Are The Same";

        public SwitcherSettings()
        {
            InitializeComponent();

            //Updates File Path View With Currently Selected Paths
            readSettings();
            TextCleanPath.Text = inputUnmodded;
            TextModdedPath.Text = inputModded;
            TextLauncherPath.Text = inputLauncherPath;
            validatePath(inputUnmodded, PathStatusClean);
            validatePath(inputModded, PathStatusModded);

            if (SwitcherCore.LauncherSelect == "EpicGames")
            {
                EpicButton.Button.Fill = EpicButton.buttonOnFill;
                EpicButton.ButtonOn = true;
            }
            else if (SwitcherCore.LauncherSelect == "Steam")
            {
                SteamButton.Button.Fill = SteamButton.buttonOnFill;
                SteamButton.ButtonOn = true;
            }
        }

        //Opens MainWindow If SwitcherSettings Closes
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            e.Cancel = false;
        }

        //Writes Launcher Settings To Launcher.xml
        private void writeLauncherSettings()
        {
            var localAppDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            var launcherSettingsPath = System.IO.Path.Combine(localAppDataPath, @"Switcher/launcher.xml");

            using (XmlWriter settingsWriter = XmlWriter.Create(launcherSettingsPath))
            {
                settingsWriter.WriteStartDocument();
                settingsWriter.WriteStartElement("Settings");
                settingsWriter.WriteElementString("Launcher", inputLauncher);
                settingsWriter.WriteElementString("LauncherPath", inputLauncherPath);
                settingsWriter.WriteEndElement();
                settingsWriter.WriteEndDocument();
            }
        }

        //Creates Settings.XML And Writes User Defined Settings To Settings.xml
        private void writeSettings()
        {
            var localAppDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            var programDataPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
            var switcherSettingsPath = System.IO.Path.Combine(localAppDataPath, @"Switcher/settings.xml");
            var launcherSettingsPath = System.IO.Path.Combine(localAppDataPath, @"Switcher/launcher.xml");
            var manifestsPath = System.IO.Path.Combine(programDataPath, @"Epic\EpicGamesLauncher\Data\Manifests");
            var launcherInstalledPath = System.IO.Path.Combine(programDataPath, @"Epic\UnrealEngineLauncher\LauncherInstalled.dat");
            var egstorePath = System.IO.Path.Combine(inputUnmodded, ".egstore");
            var egstorePath2 = System.IO.Path.Combine(inputModded, ".egstore");

            if (inputLauncher == "EpicGames")
            {
                string[] manifests = System.IO.Directory.GetFiles(egstorePath, "*.manifest");
                foreach (string manifest in manifests)
                {
                    string manifestID = System.IO.Path.GetFileNameWithoutExtension(manifest) + ".item";
                    string manifestPath = System.IO.Path.Combine(manifestsPath, manifestID);

                    using (XmlWriter settingsWriter = XmlWriter.Create(switcherSettingsPath))
                    {
                        settingsWriter.WriteStartDocument();
                        settingsWriter.WriteStartElement("Settings");

                        settingsWriter.WriteStartElement("Preferences");
                        settingsWriter.WriteElementString("Launcher", inputLauncher);
                        settingsWriter.WriteEndElement();

                        settingsWriter.WriteStartElement("InstallPaths");

                        settingsWriter.WriteStartElement("Switcher");
                        settingsWriter.WriteElementString("Unmodded", inputUnmodded.Replace(@"\", @"\\"));
                        settingsWriter.WriteElementString("Modded", inputModded.Replace(@"\", @"\\"));
                        settingsWriter.WriteEndElement();

                        settingsWriter.WriteStartElement("Windows");
                        settingsWriter.WriteElementString("Unmodded", inputUnmodded);
                        settingsWriter.WriteElementString("Modded", inputModded);
                        settingsWriter.WriteElementString("Launcher", inputLauncherPath);
                        settingsWriter.WriteElementString("Manifest", manifestPath);
                        settingsWriter.WriteElementString("LauncherInstalled", launcherInstalledPath);
                        settingsWriter.WriteEndElement();

                        settingsWriter.WriteEndElement();

                        settingsWriter.WriteEndElement();
                        settingsWriter.WriteEndDocument();
                    }
                }
            }
            else if (inputLauncher == "Steam")
            {
                using (XmlWriter settingsWriter = XmlWriter.Create(switcherSettingsPath))
                {
                    settingsWriter.WriteStartDocument();
                    settingsWriter.WriteStartElement("Settings");

                    settingsWriter.WriteStartElement("Preferences");
                    settingsWriter.WriteElementString("Launcher", inputLauncher);
                    settingsWriter.WriteEndElement();

                    settingsWriter.WriteStartElement("InstallPaths");

                    settingsWriter.WriteElementString("Unmodded", inputUnmodded);
                    settingsWriter.WriteElementString("Modded", inputModded);
                    settingsWriter.WriteElementString("Launcher", inputLauncherPath);

                    settingsWriter.WriteEndElement();

                    settingsWriter.WriteEndElement();
                    settingsWriter.WriteEndDocument();
                }
            }
        }

        //Reads All Settings From Settings.xml
        public static void readLauncherSettings()
        {
            var localAppDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            var launcherSettingsPath = System.IO.Path.Combine(localAppDataPath, @"Switcher/launcher.xml");

            if (System.IO.File.Exists(launcherSettingsPath))
            {
                XmlDocument launcherXML = new XmlDocument();
                launcherXML.Load(launcherSettingsPath);

                XmlNodeList launcherSettings = launcherXML.SelectNodes("Settings");
                foreach (XmlNode setting in launcherSettings)
                {
                    SwitcherCore.LauncherSelect = setting["Launcher"].InnerText;
                    SwitcherCore.LauncherLocation = setting["LauncherPath"].InnerText;
                    inputLauncher = setting["Launcher"].InnerText;
                    inputLauncherPath = setting["LauncherPath"].InnerText;
                }
            }
        }

        //Reads Settings From Settings.xml
        public static void readSettings()
        {
            if (SwitcherCore.LauncherSelect == "EpicGames")
            {
                var localAppDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                var switcherSettingsPath = System.IO.Path.Combine(localAppDataPath, @"Switcher/settings.xml");

                if (System.IO.File.Exists(switcherSettingsPath))
                {

                    XmlDocument settingsXML = new XmlDocument();
                    settingsXML.Load(switcherSettingsPath);

                    XmlNodeList switcherSettings = settingsXML.SelectNodes("Settings/InstallPaths/Switcher");
                    foreach (XmlNode path in switcherSettings)
                    {
                        SwitcherCore.LocationUnmodded = path["Unmodded"].InnerText;
                        SwitcherCore.LocationModded = path["Modded"].InnerText;
                    }

                    XmlNodeList windowsSettings = settingsXML.SelectNodes("Settings/InstallPaths/Windows");
                    foreach (XmlNode path in windowsSettings)
                    {
                        inputUnmodded = path["Unmodded"].InnerText;
                        inputModded = path["Modded"].InnerText;
                        SwitcherCore.ManifestLocation = path["Manifest"].InnerText;
                        SwitcherCore.LauncherInstalledLocation = path["LauncherInstalled"].InnerText;
                    }

                    XmlNodeList preferences = settingsXML.SelectNodes("Settings/Preferences");
                    foreach (XmlNode preference in preferences)
                    {

                    }

                }
            }
            else if (SwitcherCore.LauncherSelect == "Steam")
            {
                var localAppDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                var switcherSettingsPath = System.IO.Path.Combine(localAppDataPath, @"Switcher/settings.xml");

                if (System.IO.File.Exists(switcherSettingsPath))
                {
                    XmlDocument settingsXML = new XmlDocument();
                    settingsXML.Load(switcherSettingsPath);

                    XmlNodeList installPaths = settingsXML.SelectNodes("Settings/InstallPaths");
                    foreach (XmlNode path in installPaths)
                    {
                        SwitcherCore.LocationUnmodded = path["Unmodded"].InnerText;
                        SwitcherCore.LocationModded = path["Modded"].InnerText;
                        inputUnmodded = path["Unmodded"].InnerText;
                        inputModded = path["Modded"].InnerText;
                    }
                }
            }
        }

        //Validates User Selected Paths
        private bool validatePath(string path, TextBox pathStatus)
        {
            string validFile = System.IO.Path.Combine(path, "GTA5.exe");
            bool validPath = false;

            if (!System.IO.File.Exists(validFile))
            {
                pathStatus.Foreground = textColorInvalid;
                pathStatus.Text = textInvalid;
                validPath = false;
            }
            else
            {
                pathStatus.Foreground = textColorValid;
                pathStatus.Text = textValid;
                validPath = true;
            }

            return validPath;
        }

        //Checks If .egstore Directory Is Presend
        private bool egstoreCheck()
        {
            var egstorePath = System.IO.Path.Combine(inputUnmodded, ".egstore");
            var egstorePath2 = System.IO.Path.Combine(inputModded, ".egstore");
            bool egstorePresent = false;

            if (System.IO.Directory.Exists(egstorePath) && (System.IO.Directory.Exists(egstorePath2)))
            {
                egstorePresent = true;
            }

            return egstorePresent;
        }

        //Ammends Paths In Settings To A Format Accepted By Launcher (REDUNDANT)
        private void ammendSettings()
        {
            var localAppDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            var switcherSettingsPath = System.IO.Path.Combine(localAppDataPath, @"Switcher/settings.xml");

            StreamReader sr = new StreamReader(switcherSettingsPath);
            string SettingsXML_Current = sr.ReadToEnd();
            sr.Close();

            string settingsXML_New = SettingsXML_Current.Replace(@"\", @"\\");

            StreamWriter sw = new StreamWriter(switcherSettingsPath);
            sw.Write(settingsXML_New);
            sw.Close();
        }

        //Syncs Manifest With Launcher
        private void resyncManifest()
        {
            if ((ResyncButton.ButtonOn) && (SwitcherCheck.SyncError))
            {
                SwitcherCore.switchInstall_Epic("switchToClean");
                MessageBox.Show("Manifest Synced, Please restart switcher for changes to take effect.", "Success!");
            }
        }

        //Updates Launcher Used Based On User Preference
        private bool updateLauncher()
        {
            bool launcherSet = false;

            if ((EpicButton.ButtonOn))
            {
                inputLauncher = "EpicGames";
                launcherSet = true;
            }
            else if ((SteamButton.ButtonOn))
            {
                inputLauncher = "Steam";
                launcherSet = true;
            }

            return launcherSet;
        }

        //Collectes Install Path Of Clean Install
        private void BrowseClean_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WinForms.FolderBrowserDialog browseClean = new WinForms.FolderBrowserDialog();
            browseClean.ShowNewFolderButton = false;
            browseClean.SelectedPath = System.AppDomain.CurrentDomain.BaseDirectory;
            WinForms.DialogResult result = browseClean.ShowDialog();
            TextCleanPath.Text= browseClean.SelectedPath;
            inputUnmodded = browseClean.SelectedPath;
            //inputUnmoddedAppended = browseClean.SelectedPath.Replace(@"/", @"//");

            validatePath(inputUnmodded, PathStatusClean);
        }

        //Collects Install Path Of Modded Install
        private void BrowseModded_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WinForms.FolderBrowserDialog browseModded = new WinForms.FolderBrowserDialog();
            browseModded.ShowNewFolderButton = false;
            browseModded.SelectedPath = System.AppDomain.CurrentDomain.BaseDirectory;
            WinForms.DialogResult result = browseModded.ShowDialog();
            TextModdedPath.Text = browseModded.SelectedPath;
            inputModded = browseModded.SelectedPath;
            //inputModdedAppended = browseModded.SelectedPath.Replace(@"/", @"//");

            validatePath(inputModded, PathStatusModded);
        }

        //Collects Launcher Install Path If Different From Default
        private void BrowseLauncher_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WinForms.FolderBrowserDialog browseLauncher = new WinForms.FolderBrowserDialog();
            browseLauncher.ShowNewFolderButton = false;
            browseLauncher.SelectedPath = System.AppDomain.CurrentDomain.BaseDirectory;
            WinForms.DialogResult result = browseLauncher.ShowDialog();
            TextLauncherPath.Text = browseLauncher.SelectedPath;
            inputLauncherPath = browseLauncher.SelectedPath;
        }

        //Validates Paths And Updates Settings
        private void ApplyButton_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if ((inputUnmodded == inputModded))
            {
                MessageBox.Show("Clean Installation Path and Modded Installation Path cannot be the same!", "Something Went Wrong");
            }
            else if (!(validatePath(inputUnmodded, PathStatusClean)) || !(validatePath(inputModded, PathStatusModded)))
            {
                MessageBox.Show("Selected paths are invalid!", "Something Went Wrong");
            }
            else if (!updateLauncher())
            {
                MessageBox.Show("Invalid Launcher", "Something Went Wrong");
            }
            else if (inputLauncher == "EpicGames" && (!egstoreCheck()))
            {
                MessageBox.Show("Selected installs are not compatible with the Epic Games Launcher!", "Something Went Wrong");
            }
            else
            {
                writeLauncherSettings();
                writeSettings();
                readLauncherSettings();
                readSettings();
                resyncManifest();

                this.Close();
            }
        }

        private void EpicButton_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var programFiles86 = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86);
            var defaultEpicPath = System.IO.Path.Combine(programFiles86, @"Epic Games\Launcher\Portal\Binaries\Win32\EpicGamesLauncher.exe");
            var defaultSteamPath = System.IO.Path.Combine(programFiles86, @"Steam\steam.exe");

            TextLauncherPath.Text = defaultEpicPath;
            inputLauncherPath = defaultEpicPath;

            EpicButton.Button.Fill = EpicButton.buttonOnFill;
            EpicButton.ButtonOn = true;

            SteamButton.Button.Fill = SteamButton.buttonOffFill;
            SteamButton.ButtonOn = false;
        }

        private void SteamButton_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var programFiles86 = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86);
            var defaultEpicPath = System.IO.Path.Combine(programFiles86, @"Epic Games\Launcher\Portal\Binaries\Win32\EpicGamesLauncher.exe");
            var defaultSteamPath = System.IO.Path.Combine(programFiles86, @"Steam\steam.exe");

            TextLauncherPath.Text = defaultSteamPath;
            inputLauncherPath = defaultSteamPath;

            SteamButton.Button.Fill = SteamButton.buttonOnFill;
            SteamButton.ButtonOn = true;

            EpicButton.Button.Fill = EpicButton.buttonOffFill;
            EpicButton.ButtonOn = false;
        }

        private void ResyncButton_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (SwitcherCore.LauncherSelect == "Steam")
            {
                ResyncButton.Button.Fill = ResyncButton.buttonOffFill;
                ResyncButton.ButtonOn = false;
            }
        }
    }
}