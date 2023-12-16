using SharedData;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;

namespace np_sync_sockets
{
    class Program
    {
        // порт та адреса для приймання вхідних підключень
        static int port = 8080;
        static string address = "127.0.0.1"; // localhost
        static void Main(string[] args)
        {
            Console.WindowWidth = 70;

            // створення кінцевої точки для запуску сервера
            IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse(address), port);

            // створюємо сокет на вказаній кінцевій точці
            TcpListener listener = new TcpListener(ipPoint);

            // запуск приймання підключень на сервер
            listener.Start(10);

            while (true)
            {
                Console.WriteLine("Server started! Waiting for connection...");
                TcpClient client = listener.AcceptTcpClient(); // wait until connection

                //client.SendBufferSize = 0;

                try
                {
                    while (client.Connected)
                    {
                        NetworkStream ns = client.GetStream();

                        // ns.Write() - send data to client
                        // ns.Read()  - get data from the client

                        // отримуємо переданий об'єкт та десеріалізуємо його
                        BinaryFormatter formatter = new BinaryFormatter();
                        var request = (Request)formatter.Deserialize(ns);

                        Console.WriteLine($"Request data: {request.A} {request.B} from {client.Client.RemoteEndPoint}");

                        // відправляємо відповідь
                        double result = 0;
                        switch (request.Operation)
                        {
                            case OperationType.Add: result = request.A + request.B; break;
                            case OperationType.Sub: result = request.A - request.B; break;
                            case OperationType.Mult: result = request.A * request.B; break;
                            case OperationType.Div: result = request.A / request.B; break;
                        }

                        string response = $"Result = {result}";
                        Console.WriteLine(response);

                        StreamWriter sw = new StreamWriter(ns); // розмір буфера за замовчуванням: 1KB
                        sw.WriteLine(response);
                        sw.Flush(); // clear buffer
                    }

                    // закриваємо сокет
                    client.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            listener.Stop();
        }
    }
}