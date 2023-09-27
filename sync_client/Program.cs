using SharedData;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;

namespace sync_client
{
    class Program
    {
        // адрес та порт сервера, до якого відбувається підключення
        static int port = 8080;              // порт сервера
        static string address = "127.0.0.1"; // адреса сервера
        static void Main(string[] args)
        {
            IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse(address), port);

            TcpClient client = new TcpClient();

            // підключення до віддаленого хоста
            client.Connect(ipPoint);

            try
            {
                Request request = new Request();
                do
                {
                    // введення даних для відправки
                    Console.Write("Enter A:");
                    request.A = double.Parse(Console.ReadLine());
                    Console.Write("Enter B:");
                    request.B = double.Parse(Console.ReadLine());
                    Console.Write("Enter Operation (1-4):");
                    request.Operation = (OperationType)Enum.Parse(typeof(OperationType), Console.ReadLine());

                    // отримуємо потік для обміну повідомленнями
                    NetworkStream ns = client.GetStream();

                    // ns.Write() - send data to server
                    // ns.Read()  - get data from the server

                    // серіалізація об'єкта та відправка його
                    BinaryFormatter formatter = new BinaryFormatter();
                    formatter.Serialize(ns, request);

                    // отримуємо відповідь
                    //StreamReader sr = new StreamReader(ns);
                    //string response = sr.ReadLine();

                    //Console.WriteLine("Server response: " + response);
                } while (request.A != 0 || request.B != 0);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                // закриваємо підключення
                client.Close();
            }
        }
    }
}
