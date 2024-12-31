using Cassia;

namespace PC_Timer_Off
{
    internal static class Program
    {
        private static readonly int SHUTDOWN_TIME = Settings.ShutdownTime;

        [STAThread]
        static void Main()
        {
            // Aguarda at� que todas as sess�es ativas sejam encerradas
            while (GetSessionCount() > 0)
            {
                Console.WriteLine($"\nH� {GetSessionCount()} usu�rios logados neste computador:");

                foreach (var session in GetSessions())
                {
                    Console.WriteLine($" - {session.UserAccount}");
                }

                Thread.Sleep(10000);
            }

            Console.WriteLine($"Desligando o computador em {SHUTDOWN_TIME} segundos...");

            // Desliga o computador
            System.Diagnostics.Process.Start("shutdown", $"/s /t {SHUTDOWN_TIME}");
        }

        /// <summary>
        /// Retorna as sess�es ativas no computador (ou seja, os usu�rios logados)
        /// </summary>
        static List<ITerminalServicesSession> GetSessions()
        {
            var manager = new TerminalServicesManager();

            using var server = manager.GetLocalServer();

            server.Open();

            // Todas as sess�es ativas no computador
            var allSessions = server.GetSessions();

            // Filtra as sess�es que n�o est�o nulas
            var notNullSessions = allSessions.Where(session => session.UserAccount != null).ToList();

            return notNullSessions;
        }

        /// <summary>
        /// Retorna a quantidade de sess�es ativas no computador
        /// </summary>
        static int GetSessionCount()
        {
            return GetSessions().Count;
        }
    }
}