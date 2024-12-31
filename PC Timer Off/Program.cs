using Cassia;

namespace PC_Timer_Off
{
    internal static class Program
    {
        private static readonly int SHUTDOWN_TIME = Settings.ShutdownTime;

        [STAThread]
        static void Main()
        {
            // Aguarda até que todas as sessões ativas sejam encerradas
            while (GetSessionCount() > 0)
            {
                Console.WriteLine($"\nHá {GetSessionCount()} usuários logados neste computador:");

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
        /// Retorna as sessões ativas no computador (ou seja, os usuários logados)
        /// </summary>
        static List<ITerminalServicesSession> GetSessions()
        {
            var manager = new TerminalServicesManager();

            using var server = manager.GetLocalServer();

            server.Open();

            // Todas as sessões ativas no computador
            var allSessions = server.GetSessions();

            // Filtra as sessões que não estão nulas
            var notNullSessions = allSessions.Where(session => session.UserAccount != null).ToList();

            return notNullSessions;
        }

        /// <summary>
        /// Retorna a quantidade de sessões ativas no computador
        /// </summary>
        static int GetSessionCount()
        {
            return GetSessions().Count;
        }
    }
}