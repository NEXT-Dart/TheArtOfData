using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TheArtOfDecoding.Server;

namespace TheArtOfDecodingServer.Server
{
    class ThreadQue
    {
        #region "Fields"

        private List<Thread> threads;
        private List<Client> clients;
        private int maxThreads;
        private bool _stop;

        private Thread monitorThread;

        #endregion

        #region "Constructors"

        private ThreadQue()
        {
            threads = new List<Thread>();
            clients = new List<Client>();
            maxThreads = 1;
            _stop = false;

            StartMonitor();
        }

        #endregion

        #region "Singleton"

        private static ThreadQue instance;

        public static ThreadQue INST
        {
            get
            {
                if (instance == null)
                    instance = new ThreadQue();
                return instance;
            }
        }

        #endregion

        #region "Properties"


        #endregion

        #region "Methods"

        private void StartMonitor()
        {
            monitorThread = new Thread(monitorThreads);
            monitorThread.Start();
        }

        private void monitorThreads()
        {
            while (!_stop)
            {
                // Remove clients from the list that are done
                Client[] done;
                lock (clients)
                {
                    done = clients.Where(x => x.Status == ClientStatus.Done).ToArray();

                    foreach (Client clientDone in done)
                    {
                        clients.Remove(clientDone);
                    }
                }

                if (threads.Count < maxThreads)
                {
                    Thread thread = new Thread(RunThread);
                    thread.Start();
                    threads.Add(thread);
                }
            }
        }

        private void RunThread()
        {
            while (!_stop)
            {
                Client client;
                lock (clients)
                {
                    client = clients.Where(c => c.Status == ClientStatus.Waiting).FirstOrDefault();
                    if (client == null)
                    {
                        Thread.Sleep(1);
                        continue;
                    }

                    client.Status = ClientStatus.Executing;
                }

                client.Start();

                client.Status = ClientStatus.Done;
            }
        }

        #endregion

        #region "Abstract/Virtual Methods"



        #endregion

        #region "Inherited Methods"



        #endregion

        #region "Static Methods"

        public static void AddClient(Client client)
        {
            client.Status = ClientStatus.Waiting;
            lock (INST.clients)
            {
                INST.clients.Add(client);
            }
        }

        public static void Stop()
        {
            INST._stop = true;
        }

        #endregion

        #region "Operators"



        #endregion  
    }
}
