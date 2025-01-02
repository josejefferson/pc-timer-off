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
            // Aguarda at� que todas as sess�es ativas sejam encerradas
            while (true)
            {
                Debug.WriteLine($"\nH� {GetSessionCount()} usu�rios logados neste computador:");

                // Exibe os usu�rios logados
                foreach (var session in GetSessions())
                {
                    Debug.WriteLine($" - {session.UserAccount}");
                }

                // Se n�o houver sess�es ativas, inicia o timer de desligamento
                if (GetSessionCount() == 0)
                {
                    Debug.WriteLine($"Desligando o computador em {SHUTDOWN_TIME} minutos...");

                    // Tempo que o computador est� sem sess�es ativas
                    long timeStart = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

                    while (true)
                    {
                        long currentTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

                        // Se algum usu�rio fizer login, cancela o desligamento
                        if (GetSessionCount() > 0)
                        {
                            Debug.WriteLine($"Usu�rio fez login, cancelando o desligamento...");
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
