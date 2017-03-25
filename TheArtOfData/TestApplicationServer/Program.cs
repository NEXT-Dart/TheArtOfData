using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace TestApplicationServer
{
    class Program
    {
        static void Main(string[] args)
        {
            bool completed = false;
            using (TcpClient client = new TcpClient("kdullens.com", 666))
            //using (TcpClient client = new TcpClient("localhost", 666))
            {
                if (client.Connected)
                {
                    NetworkStream stream = client.GetStream();

                    byte[] data = File.ReadAllBytes(@"D:\Developments\Git\TheArtOfData\Test images\helloWorldScaled.jpg");
                    client.GetStream().Write(BitConverter.GetBytes((long)data.Length), 0, 8);
                    client.GetStream().Write(data, 0, data.Length);

                    while (!completed)
                    {
                        if (client.Available > 0)
                        {
                            int status = stream.ReadByte();
                            switch (status)
                            {
                                case 1:
                                    Console.WriteLine("Data received");
                                    break;
                                case 5:
                                    Console.WriteLine("Data decoded");
                                    List<byte> decodedData = new List<byte>();
                                    while (client.Available > 0 || decodedData.Count == 0)
                                    {
                                        byte[] buffer = new byte[client.Available];
                                        stream.Read(buffer, 0, buffer.Length);
                                        decodedData.AddRange(buffer);
                                    }
                                    completed = true;
                                    Console.WriteLine("DATA: " + Encoding.ASCII.GetString(decodedData.ToArray()));
                                    break;
                            }
                        }
                    }
                }
            }

            Console.WriteLine("===== Done =====");
            Console.ReadKey();
        }
    }
}
