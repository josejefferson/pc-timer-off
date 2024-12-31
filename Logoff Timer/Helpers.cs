using System.Runtime.InteropServices;

namespace Logoff_Timer
{
    internal class Helpers
    {
        [StructLayout(LayoutKind.Sequential)]
        private struct LASTINPUTINFO
        {
            public uint cbSize;
            public uint dwTime;
        }

        [DllImport("user32.dll")]
        private static extern bool GetLastInputInfo(ref LASTINPUTINFO plii);

        /// <summary>
        /// Retorna o tempo de inatividade do usuário em segundos
        /// </summary>
        /// <exception cref="InvalidOperationException">Erro ao obter informações de inatividade</exception>
        public static uint GetIdleTimeInSeconds()
        {
            LASTINPUTINFO lastInputInfo = new()
            {
                cbSize = (uint)Marshal.SizeOf(typeof(LASTINPUTINFO))
            };

            if (GetLastInputInfo(ref lastInputInfo))
            {
                uint tickCount = (uint)Environment.TickCount;
                return (tickCount - lastInputInfo.dwTime) / 1000;
            }

            throw new InvalidOperationException("Falha ao obter informações de inatividade do usuário.");
        }

        [DllImport("user32.dll", SetLastError = true)]
        static extern bool ExitWindowsEx(uint uFlags, uint dwReason);

        /// <summary>
        /// Faz o logoff do usuário
        /// </summary>
        public static void Logoff()
        {
            ExitWindowsEx(0 | 0x00000004, 0);
        }
    }
}
