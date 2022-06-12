using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace np_sync_sockets
{
    class Program
    {
        static int port = 8080; // порт для приема входящих запросов
        static void Main(string[] args)
        {
            // получаем адреса для запуска сокета
            IPAddress iPAddress = IPAddress.Parse("127.0.0.1");//Dns.GetHostEntry("localhost").AddressList[1]; //localhost
            IPEndPoint ipPoint = new IPEndPoint(iPAddress, port);

            // создаем сокет
            //Socket listenSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            TcpListener listener = new TcpListener(ipPoint);

            try
            {
                // связываем сокет с локальной точкой, по которой будем принимать данные
                //listenSocket.Bind(ipPoint);

                // начинаем прослушивание
                //listenSocket.Listen(10);
                listener.Start(10);

                Console.WriteLine("Server started! Waiting for connection...");

                while (true)
                {
                    //Socket handler = listenSocket.Accept();
                    TcpClient client = listener.AcceptTcpClient();

                    // handler.Receive(); - get data from client
                    // handler.Send();    - sent data to client

                    NetworkStream ns = client.GetStream();

                    // получаем сообщение
                    //StringBuilder builder = new StringBuilder();
                    //int bytes = 0; // количество полученных байтов
                    //byte[] data = new byte[256]; // буфер для получаемых данных

                    //do
                    //{
                    //    bytes = handler.Receive(data);
                    //    builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                    //}
                    //while (handler.Available > 0);

                    StreamReader streamReader = new StreamReader(ns);
                    string message = streamReader.ReadLine();

                    //Console.WriteLine($"{handler.RemoteEndPoint} - {builder.ToString()} at {DateTime.Now.ToShortTimeString()}");
                    Console.WriteLine($"{client.Client.RemoteEndPoint} - {message} at {DateTime.Now.ToShortTimeString()}");
                    
                    // отправляем ответ
                    string response = "Message was send!";
                    //data = Encoding.Unicode.GetBytes(response);
                    //handler.Send(data);

                    StreamWriter streamWriter = new StreamWriter(ns);
                    streamWriter.WriteLine(response);

                    streamWriter.Close();
                    streamReader.Close();
                    ns.Close();
                    client.Close();
                }
                // закрываем сокет
                //handler.Shutdown(SocketShutdown.Both);
                //handler.Close();
                listener?.Stop();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
