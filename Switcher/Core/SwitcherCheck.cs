using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;

namespace Switcher
{
    class SwitcherCheck
    {

        private static bool switcherPresence = false;
        public static bool SwitcherPresence
        {
            get { return switcherPresence; }
            set { switcherPresence = value; }
        }

        private static bool syncError = false;
        public static bool SyncError
        {
            get { return syncError; }
            set { syncError = value; }
        }

        private static bool manifestModded = false;
        public static bool ManifestModded
        {
            get { return manifestModded; }
            set { manifestModded = value; }
        }
        private static bool lInstalledModded = false;
        public static bool LInstalledModded
        {
            get { return lInstalledModded; }
            set { lInstalledModded = value; }
        }

        //Checks If Current Install Is Modded Or Clean
        public static void switcherModCheck_Epic(string text)
        {
            if (System.IO.File.Exists(SwitcherCore.ManifestLocation))
            {
                StreamReader sr = new StreamReader(SwitcherCore.ManifestLocation);
                String manifestLine = sr.ReadLine();

                while (manifestLine != null)
                {
                    if (manifestLine.Contains(text))
                    {
                        manifestModded = true;
                    }

                    manifestLine = sr.ReadLine();
                }

                sr.Close();

                StreamReader sr2 = new StreamReader(SwitcherCore.LauncherInstalledLocation);
                string lInstalledLine = sr2.ReadLine();

                while (lInstalledLine != null)
                {
                    if (lInstalledLine.Contains(text))
                    {
                        lInstalledModded = true;
                    }

                    lInstalledLine = sr2.ReadLine();
                }

                sr2.Close();

                if (!(manifestModded) && !(lInstalledModded))
                {
                    switcherPresence = false;
                }
                else if ((manifestModded) && (lInstalledModded))
                {
                    switcherPresence = true;
                }
                else
                {
                    syncError = true;
                }
            }

        }

    }

}

