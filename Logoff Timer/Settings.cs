using Microsoft.Win32;

namespace Logoff_Timer
{
    internal class Settings
    {
        private const string RegistryPath = @"SOFTWARE\PCTimerOff";

        public static int InactivityTime
        {
            get => GetSetting("InactivityTime", 60);
            set => SaveSetting("InactivityTime", value);
        }

        public static int WarningTime
        {
            get => GetSetting("WarningTime", 5);
            set => SaveSetting("WarningTime", value);
        }

        private static int GetSetting(string keyName, int defaultValue)
        {
            using RegistryKey? key = Registry.LocalMachine.OpenSubKey(RegistryPath);
            return key?.GetValue(keyName) is int value ? value : defaultValue;
        }

        private static void SaveSetting(string keyName, int value)
        {
            using RegistryKey key = Registry.LocalMachine.CreateSubKey(RegistryPath);
            key.SetValue(keyName, value, RegistryValueKind.DWord);
        }
    }
}
