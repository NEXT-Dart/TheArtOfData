using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using TheArtOfDecoding.Server;

namespace TheArtOfDecoding
{
    static class Program
    {
        [DllImport("kernel32.dll")]
        static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        const int SW_HIDE = 0;
        const int SW_SHOW = 5;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            bool startServer = true;
            int serverPort = 666;
            string[] argv = Environment.GetCommandLineArgs();
            if (argv.Length > 0)
            {
                for (int i = 0; i < argv.Length; i++)
                {
                    if (argv[i] == "--server" || argv[i] == "-s")
                    {
                        startServer = true;
                    }
                    else if (argv[i].StartsWith("--port") || argv[i].StartsWith("-p"))
                    {
                        string[] portinfo = argv[i].Split(new char[] { '=', ':' });
                        if (portinfo.Length > 1)
                            serverPort = Convert.ToInt32(portinfo[1]);
                    }
                    else if (argv[i] == "--help" || argv[i] == "-h")
                    {
                        Console.WriteLine("Help for the The Art of Decoding application");
                        Console.WriteLine();
                        Console.WriteLine("  -h, --help\t\tShows this help message");
                        Console.WriteLine("  -p:<port>,\n    --port:<port>\tSpecifies the port for this server to connect on");
                        return;
                    }
                }
            }

            if (startServer)
            {
                MainServer server = new MainServer(serverPort);
                Console.CancelKeyPress += new ConsoleCancelEventHandler(server.Stop);
                server.Start();

                while(server.Running)
                {
                    Thread.Sleep(1);
                }
            }
        }
    }
}
