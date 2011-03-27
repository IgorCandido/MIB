using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using First.BlackBox;
using First.Interfaces;
using First.TCP_Receiver_Emitter;
using First_Interfaces;
using First_Interfaces.ClientNodeInformationTypes;
using First_Interfaces.ServiceInterfaces;

namespace First_Client
{
    class Program
    {
        static void Main(string[] args)
        {

            IReceiver receiver = ProxyFactory.GetProxy<IReceiver>("tcpReceiver");

            TcpClientInformation info = new TcpClientInformation() {emitterType = "TCP", hostname = "localhost", port = 66};

            MemoryStream stream = new MemoryStream();

            BinaryWriter writer = new BinaryWriter(stream);
            
            TcpListener tcpListener = new TcpListener(66);

            tcpListener.Start();

            tcpListener.BeginAcceptTcpClient( DoIt , tcpListener);

            SubscriptionContract subscription = new SubscriptionContract()
                                                    {
                                                        ClientInformation = TcpClientInformationAdapter.SerializeToClientInformation(info), 
                                                        Topic = "teste"
                                                    };

            Console.WriteLine("Press enter to make the following subscription: {0}", subscription);

            Console.ReadKey();

            receiver.Subscribe(subscription);



            Console.WriteLine("Press enter to make send information");

            Console.ReadKey();

            writer.Write("teste");
            writer.Flush();
            writer.Close();

            receiver.Receive(new EventContract() {topic = "teste" , data = stream.GetBuffer()});

        }


        public static void DoIt(IAsyncResult ar)
        {
                                                      
            TcpListener list = (TcpListener) ar.AsyncState;

            TcpClient client = list.EndAcceptTcpClient(ar);

            int numberOfBytesRead;

            NetworkStream myNetworkStream = client.GetStream();

            BinaryReader reader = new BinaryReader(myNetworkStream);

            String message = reader.ReadString();

            Console.WriteLine("Os dados recebidos foram: " + message );

                                                      
        }
    }
}
