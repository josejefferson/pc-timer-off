using System.Diagnostics;

namespace Logoff_Timer
{
    internal static class Program
    {
        private static readonly int INACTIVITY_TIME = Settings.InactivityTime;
        private static readonly int WARNING_TIME = Settings.WarningTime;

        [STAThread]
        static void Main()
        {
            while (true)
            {
                // Obtém o tempo de inatividade do usuário
                uint time = Helpers.GetIdleTimeInSeconds();
                Debug.WriteLine($"O usuário está inativo há {time} segundos.");

                // Se o tempo de inatividade for maior ou igual ao tempo limite, exibe o popup de aviso e faz o logoff
                if (time >= (INACTIVITY_TIME * 60))
                {
                    Logoff();
                    break;
                }

                // Aguarda 1 segundo
                Thread.Sleep(1000);
            }
        }

        /// <summary>
        /// Exibe um popup de aviso de inatividade para o usuário e faz o logoff
        /// </summary>
        static void Logoff()
        {
            // Exibe o popup de aviso em uma nova thread
            Task.Run(WarnLogoff);

            // Aguarda o tempo de aviso
            Thread.Sleep(WARNING_TIME * 1000 * 60);

            // Faz o logoff
            Debug.WriteLine("Fazendo logoff");
            Helpers.Logoff();
        }

        /// <summary>
        /// Exibe um popup de aviso de inatividade para o usuário
        /// </summary>
        static void WarnLogoff()
        {
            MessageBox.Show($"Você está há mais de {INACTIVITY_TIME} minutos inativo. O seu usuário será desconectado em {WARNING_TIME} minutos. Salve o seu trabalho.", "Aviso de inatividade", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }
}