using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Formular.Helpers
{
    public static class SettingsManager
    {
        private static string pushBulletToken = string.Empty;
        private static string settingsFile = "settings.txt";
        public static string PushBulletToken
        {
            get
            {
                if (string.IsNullOrWhiteSpace(pushBulletToken) && File.Exists(settingsFile))
                    pushBulletToken = File.ReadAllText(settingsFile);

                return pushBulletToken;
            }
            set
            {
                File.WriteAllText(settingsFile, value);
            }
        }
    }
}
