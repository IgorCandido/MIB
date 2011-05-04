using System;
using System.IO;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using Interfaces.ClientInformationTypes;
using Interfaces.Contracts;
using Interfaces.ContractsFromClients;
using Interfaces.Factories;
using Interfaces.Services;
using Interfaces.Tcp;
using Interfaces.Tcp.ClientObjects;

namespace TcpClient
{
    class Program
    {
        /// <summary>
        /// Mains the specified args.
        /// </summary>
        /// <param name="args">The args.</param>
        static void Main(string[] args)
        {

            //IReceiver receiver = ProxyFactory.GetProxy<IReceiver>("tcpReceiver");

            TcpClientInformation info = new TcpClientInformation() {EmitterType = "TCP", Hostname = "localhost", Port = 66};

            TcpListener tcpListener = new TcpListener(66);

            tcpListener.Start();

            tcpListener.BeginAcceptTcpClient( DoIt , tcpListener);

            SubscriptionContract subscription = new SubscriptionContract()
                                                    {
                                                        ClientInformation = TcpClientInformationAdapter.SerializeToClientInformation(info),
                                                        ContentDescription = @"<description><color>black</color></description>"
                                                    };

            Console.WriteLine("Press enter to make the following subscription: {0}", subscription);

            Console.ReadKey();

            MemoryStream subscriptionStream = new MemoryStream();
            
            BinaryFormatter formatter = new BinaryFormatter();

            formatter.Serialize(subscriptionStream, subscription);

            TcpCommand command = new TcpCommand() { Command = Commands.Subscription, Payload = subscriptionStream.GetBuffer() };

            System.Net.Sockets.TcpClient client = new System.Net.Sockets.TcpClient("localhost", 70);

            formatter.Serialize(client.GetStream(), command);

            client.Close();

            Console.WriteLine("Press enter to make send information");

            Console.ReadKey();
            
            client = new System.Net.Sockets.TcpClient();

            client.Connect("localhost",70);

            MemoryStream stream = new MemoryStream();

            BinaryWriter writer = new BinaryWriter(stream);

            writer.Write("teste");
            writer.Flush();
            writer.Close();

            //receiver.Receive(new EventContract() {topic = "teste" , data = stream.GetBuffer()});

            EventContract eventObj = new EventContract() { ContentDescription = @"<description><color>black</color></description>", 
                                                           Data = stream.GetBuffer() }; 

            stream = new MemoryStream();

            formatter.Serialize(stream, eventObj);

            command = new TcpCommand() {Command = Commands.Event, Payload = stream.GetBuffer()};

            formatter.Serialize(client.GetStream(), command);

            Console.ReadKey();

        }


        /// <summary>
        /// Does it.
        /// </summary>
        /// <param name="ar">The ar.</param>
        public static void DoIt(IAsyncResult ar)
        {
                                                      
            TcpListener list = (TcpListener) ar.AsyncState;

            System.Net.Sockets.TcpClient client = list.EndAcceptTcpClient(ar);

            NetworkStream myNetworkStream = client.GetStream();

            BinaryReader reader = new BinaryReader(myNetworkStream);

            String message = reader.ReadString();

            Console.WriteLine("Os dados recebidos foram: " + message );

                                                      
        }
    }
}
