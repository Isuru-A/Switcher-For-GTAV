using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml;
using System.Windows.Forms;
using System.Diagnostics;

namespace Switcher
{
    class SwitcherCore
    {

        private static string locationUnmodded = @"";
        public static string LocationUnmodded
        {
            get { return locationUnmodded; }
            set { locationUnmodded = value; }

        }

        private static string locationModded = @"";
        public static string LocationModded
        {
            get { return locationModded; }
            set { locationModded = value; }
        }

        private static string manifestLocation = @"";
        public static string ManifestLocation
        {
            get { return manifestLocation; }
            set { manifestLocation = value; }

        }

        private static string launcherInstalledLocation = @"";
        public static string LauncherInstalledLocation
        {
            get { return launcherInstalledLocation; }
            set { launcherInstalledLocation = value; }
        }

        private static string launcherSelect = "";
        public static string LauncherSelect
        {
            get { return launcherSelect; }
            set { launcherSelect = value; }
        }

        private static string launcherLocation = @"";
        public static string LauncherLocation
        {
            get { return launcherLocation; }
            set { launcherLocation = value; }
        }
        
        //Changes Manifest Locations From Clean Copy To Modded Copy
        public static void switchInstall_Epic(string action)
        {
            if (System.IO.File.Exists(manifestLocation))
            {
                StreamReader sr = new StreamReader(manifestLocation);
                String manifestContent = sr.ReadToEnd();
                sr.Close();
                StreamReader sr2 = new StreamReader(launcherInstalledLocation);
                string lInstallContent = sr2.ReadToEnd();
                sr2.Close();

                string manifestContent_New = ".";
                string lInstalledContent_New = ".";

                if (action == "switchToClean")
                {
                    manifestContent_New = manifestContent.Replace(locationModded, locationUnmodded);
                    lInstalledContent_New = lInstallContent.Replace(locationModded, locationUnmodded);
                }
                else if (action == "switchToModded")
                {
                    manifestContent_New = manifestContent.Replace(locationUnmodded, locationModded);
                    lInstalledContent_New = lInstallContent.Replace(locationUnmodded, locationModded);
                }
                
                StreamWriter sw = new StreamWriter(manifestLocation);
                sw.Write(manifestContent_New);
                sw.Close();
                StreamWriter sw2 = new StreamWriter(launcherInstalledLocation);
                sw2.Write(lInstalledContent_New);
                sw2.Close();
            }
            else
            {
                MessageBox.Show("Unable to locate files", "Something Went Wrong");
            }
        }

        public static void switchInstall_Steam(string action)
        {

        }
    }
}
