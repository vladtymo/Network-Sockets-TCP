using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace sync_client
{
    class Program
    {
        // адрес и порт сервера, к которому будем подключаться
        static int port = 8080; // порт сервера
        static string address = "127.0.0.1"; // адрес сервера
        static void Main(string[] args)
        {
            IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse(address), port);

            //Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            TcpClient client = null;

            // подключаемся к удаленному хосту
            //socket.Connect(ipPoint);

            string message = string.Empty;
            while (message != "end")
            {
                try
                {
                    client = new TcpClient(address, port);
                    //client.Connect(ipPoint);

                    Console.Write("Enter a message:");
                    message = Console.ReadLine();

                    NetworkStream ns = client.GetStream();

                    StreamWriter streamWriter = new StreamWriter(ns);
                    streamWriter.WriteLine(message);

                    streamWriter.Flush(); // send all buffered data and clear buffer

                    //byte[] data = Encoding.Unicode.GetBytes(message);
                    //socket.Send(data);

                    // получаем ответ
                    //data = new byte[256]; // буфер для ответа
                    //StringBuilder builder = new StringBuilder();
                    //int bytes = 0; // количество полученных байт

                    //do
                    //{
                    //    bytes = socket.Receive(data, data.Length, 0);
                    //    builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                    //}
                    //while (socket.Available > 0);
                    // string response = builder.ToString();

                    //NetworkStream ns = client.GetStream();

                    StreamReader streamReader = new StreamReader(ns);
                    string response = streamReader.ReadLine();
                    Console.WriteLine("server response: " + response);

                    streamReader.Close();
                    streamWriter.Close();
                    ns.Close();

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                finally
                {
                    // закрываем сокет
                    //socket.Shutdown(SocketShutdown.Both);
                    //socket.Disconnect(true);
                    //socket.Close();
                    client?.Close();
                }
            }
        }
    }
}
