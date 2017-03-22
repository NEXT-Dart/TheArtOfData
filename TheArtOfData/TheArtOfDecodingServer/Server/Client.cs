using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TheArtOfDecoding.Server
{
    class Client
    {
        #region "Fields"

        private TcpClient client;
        private Thread _thread;
        private bool _stopThread;
        private int id;
        private Image image;

        private static int clientId;

        #endregion

        #region "Constructors"

        public Client(TcpClient client)
        {
            this.client = client;
            this.id = clientId;
            _stopThread = false;
            clientId++;
            _thread = new Thread(RunThread);
        }

        #endregion

        #region "Properties"

        public Image Image
        {
            get { return image; }
        }

        #endregion

        #region "Methods"

        public void Start()
        {
            _thread.Start();

            Log(LogType.INFO, "Connection opened");
        }

        private void RunThread()
        {
            NetworkStream stream = client.GetStream();

            // Get the amount of data we need to receive
            long dataLength = GetDataLength(stream);
            Log(LogType.INFO, "Receiving " + dataLength + " bytes of data");

            // Get the data
            byte[] data = GetData(stream, dataLength);
            Log(LogType.INFO, "Data received");

            // Send the status to the client
            stream.WriteByte(1);

            Log(LogType.INFO, "Reading image");
            MemoryStream memStream = new MemoryStream(data);
            image = Image.FromStream(memStream);
            ImageParser ip = new ImageParser(image);
            Log(LogType.INFO, "Parsing image");
            Image result = ip.Run();

            try
            {
                Log(LogType.INFO, "Decoding data");
                ImageDataReader reader = new ImageDataReader(result);
                byte[] readedData = reader.GetData();
                stream.WriteByte(5);
                stream.Write(readedData, 0, readedData.Length);
            }
            catch (Exception ex)
            {
                Log(LogType.ERROR, ex.Message + "\n" + ex.StackTrace);
            }

            Log(LogType.INFO, "Client decoding is done");
            Terminate();
        }

        public long GetDataLength(NetworkStream stream)
        {
            while (client.Available < 8)
            {
                Thread.Sleep(1);

                if (_stopThread) break;
            }

            byte[] b_dataLength = new byte[8];
            stream.Read(b_dataLength, 0, 8);

            return BitConverter.ToInt64(b_dataLength, 0);
        }

        private byte[] GetData(NetworkStream stream, long dataLength)
        {
            List<byte> data = new List<byte>();
            while (data.Count < dataLength)
            {
                byte[] newData = new byte[client.Available];
                stream.Read(newData, 0, newData.Length);
                data.AddRange(newData);

                if (_stopThread) break;
            }
            return data.ToArray();
        }

        public void Terminate()
        {
            _stopThread = true;
            MainServer.INSTANCE.RemoveClient(this);
        }

        private void Log(LogType type, string message)
        {
            MainServer.Log(type, "Client#" + id + " -> " + message);
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
