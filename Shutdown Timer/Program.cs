using System.Diagnostics;
using Cassia;

namespace Shutdown_Timer
{

    internal static class Program
    {
        private static readonly int SHUTDOWN_TIME = Settings.ShutdownTime;

        [STAThread]
        static void Main()
        {
            // Aguarda até que todas as sessões ativas sejam encerradas
            while (true)
            {
                Debug.WriteLine($"\nHá {GetSessionCount()} usuários logados neste computador:");

                // Exibe os usuários logados
                foreach (var session in GetSessions())
                {
                    Debug.WriteLine($" - {session.UserAccount}");
                }

                // Se não houver sessões ativas, inicia o timer de desligamento
                if (GetSessionCount() == 0)
                {
                    Debug.WriteLine($"Desligando o computador em {SHUTDOWN_TIME} minutos...");

                    // Tempo que o computador está sem sessões ativas
                    long timeStart = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

                    while (true)
                    {
                        long currentTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

                        // Se algum usuário fizer login, cancela o desligamento
                        if (GetSessionCount() > 0)
                        {
                            Debug.WriteLine($"Usuário fez login, cancelando o desligamento...");
                            break;
                        }

                        // Se o tempo de desligamento for atingido, desliga o computador
                        if ((currentTime - timeStart) >= (SHUTDOWN_TIME * 60))
                        {
                            Debug.WriteLine($"Desligando o computador agora...");

                            // Desliga o computador
                            System.Diagnostics.Process.Start("shutdown", $"/s /t {SHUTDOWN_TIME}");
                            return;
                        }

                        Thread.Sleep(10000);
                    }
                }

                Thread.Sleep(10000);
            }
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
