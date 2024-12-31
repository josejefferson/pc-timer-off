using Microsoft.Win32;

namespace PC_Timer_Off
{
    internal class Settings
    {
        private const string RegistryPath = @"SOFTWARE\PCTimerOff";

        public static int ShutdownTime
        {
            get => GetSetting("ShutdownTime", 10);
            set => SaveSetting("ShutdownTime", value);
        }

        private static int GetSetting(string keyName, int defaultValue)
        {
            using RegistryKey key = Registry.CurrentUser.CreateSubKey(RegistryPath);
            return key?.GetValue(keyName) is int value ? value : defaultValue;
        }

        private static void SaveSetting(string keyName, int value)
        {
            using RegistryKey key = Registry.CurrentUser.CreateSubKey(RegistryPath);
            key.SetValue(keyName, value, RegistryValueKind.DWord);
        }
    }
}
