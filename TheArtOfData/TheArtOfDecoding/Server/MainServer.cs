using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TheArtOfDecoding.Server
{
    class MainServer
    {
        #region "Fields"

        private Thread _thread;
        private bool stopThread;
        private int cancelCounter;
        private bool isRunning;
        private int port;

        private TcpListener listener;

        private List<Client> clients;

        #endregion

        #region "Constructors"

        public MainServer(int port)
        {
            instance = this;
            _thread = new Thread(RunServer);
            stopThread = false;
            cancelCounter = 0;
            this.port = port;
            this.clients = new List<Client>();
        }

        #endregion

        #region "Singleton"

        private static MainServer instance;

        public static MainServer INSTANCE
        {
            get
            {
                return instance;
            }
        }

        #endregion  

        #region "Properties"

        public bool Running
        {
            get { return isRunning; }
        }

        #endregion

        #region "Methods"

        private void RunServer()
        {
            Log(LogType.INFO, "Starting the server . . .");
            Log(LogType.INFO, "Port: " + port);
            //listener = new TcpListener(new IPEndPoint(new IPAddress(new byte[] { 127, 0, 0, 1 }), port));
            listener = new TcpListener(IPAddress.Any, port);
            Log(LogType.INFO, "Start listening for clients . . .");
            listener.Start();
            listener.Server.Blocking = true;

            while (!stopThread)
            {
                try
                {
                    Log(LogType.INFO, "Waiting for client to accept . . .");
                    TcpClient clnt = listener.AcceptTcpClient();
                    if (clnt == null)
                        continue;

                    Client client = new Client(clnt);
                    clients.Add(client);
                    client.Start();
                }
                catch (SocketException ex)
                {
                    Log(LogType.WARN, "The listener stopped listening for new clients");
                    if(ex.ErrorCode != 10004)
                    {
                        Log(LogType.ERROR, ex.Message + " (" + ex.ErrorCode + ")");
                        Log(LogType.ERROR, ex.StackTrace);
                    }
                }
            }

            while (clients.Count > 0)
            {
                Thread.Sleep(1);
            }
            isRunning = false;

            Log(LogType.INFO, "Server stopped successfully");
        }

        public void Start()
        {
            _thread.Start();
            isRunning = true;
        }

        public void Stop()
        {
            if (cancelCounter == 1)
            {
                Log(LogType.INFO, "Stopping the server thread . . .");
                stopThread = true;

                Log(LogType.INFO, "Stopping the listener . . .");
                Log(LogType.WARN, "The server will finish the running tasks. Press Ctrl+C again to terminate immediately.");
                listener.Server.Close();
            }
            else if (cancelCounter == 2)
            {
                Log(LogType.INFO, "Cleaning the que . . .");
                while (clients.Count > 0)
                {
                    clients[0].Terminate();
                }
            }
            else
            {
                Log(LogType.INFO, "Waiting for the que to be empty . . .");
            }
        }

        public void RemoveClient(Client client)
        {
            clients.Remove(client);
        }

        public void Stop(object sender, ConsoleCancelEventArgs e)
        {
            e.Cancel = true;
            cancelCounter++;

            Stop();
        }

        public static void Log(LogType type, string message)
        {
            Console.Write("[");
            if (type == LogType.ERROR)
                Console.ForegroundColor = ConsoleColor.Red;
            else if (type == LogType.INFO)
                Console.ForegroundColor = ConsoleColor.Cyan;
            else if (type == LogType.WARN)
                Console.ForegroundColor = ConsoleColor.Yellow;

            Console.Write(Enum.GetName(typeof(LogType), type));

            Console.ResetColor();
            Console.WriteLine("]: " + message);
        }

        #endregion

        #region "Abstract/Virtual Methods"



        #endregion

        #region "Inherited Methods"



        #endregion

        #region "Static Methods"



        #endregion

        #region "Operators"



        #endregion
    }
}
