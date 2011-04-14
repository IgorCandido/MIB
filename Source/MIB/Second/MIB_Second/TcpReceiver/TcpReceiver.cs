using System;
using System.Configuration;
using System.IO;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.ServiceModel;
using Interfaces.Contracts;
using Interfaces.ContractsFromClients;
using Interfaces.ContractsInnerRepresentations;
using Interfaces.Factories;
using Interfaces.Services;
using Interfaces.Tcp;
using Interfaces.Tcp.ClientObjects;

namespace TcpReceiver
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Single, InstanceContextMode = InstanceContextMode.PerCall)]
    public class TcpReceiver : IReceiver
    {

        private static IBlackbox _blackbox;

        private TcpListener _listener;

        public TcpReceiver()
        {

            _blackbox = ProxyFactory.GetProxy<IBlackbox>("blackbox");

            InitiateListenerThread();

        }

        public TcpReceiver(IBlackbox blackbox, int portNumber)
        {

            _blackbox = blackbox;

            InitiateListenerThread(portNumber);

        }

        private void InitiateListenerThread(int? port = null)
        {

            int portNumber = port ?? int.Parse(ConfigurationManager.AppSettings["PortNumber"]);

            _listener = new TcpListener(portNumber);

            _listener.Start();

            _listener.BeginAcceptTcpClient(ConnectionMade, _listener);

        }
       
        #region Implementation of IReceiver

        [OperationBehavior(TransactionAutoComplete = true, TransactionScopeRequired = false)]
        public void Receive(EventContract eventContract)
        {
            
            _blackbox.Process(new Event(eventContract));

        }


        [OperationBehavior(TransactionAutoComplete = true, TransactionScopeRequired = false)]
        public void Subscribe(SubscriptionContract subscriptionContract)
        {
            
            _blackbox.Process(new Subscription(subscriptionContract));

        }

        #endregion

        private static void ConnectionMade(IAsyncResult ar)
        {

            TcpListener listener = (TcpListener) (ar.AsyncState);

            TcpClient connection = listener.EndAcceptTcpClient(ar);

            listener.BeginAcceptTcpClient(ConnectionMade, listener);

            BinaryFormatter formatter = new BinaryFormatter();

            TcpCommand command;

            if( ( command = formatter.Deserialize(connection.GetStream()) as TcpCommand ) != null )
            {

                MemoryStream deserializationStream = new MemoryStream(command.Payload);

                switch (command.Command)
                {

                    case Commands.Event:

                        EventContract eventObj;

                        if( ( eventObj = formatter.Deserialize(deserializationStream) as EventContract ) != null )
                        {
                            
                            _blackbox.Process(new Event(eventObj));

                        }

                        break;

                    case Commands.Subscription:

                        SubscriptionContract subscription;

                        if ((subscription = formatter.Deserialize(deserializationStream) as SubscriptionContract) != null)
                        {

                            _blackbox.Process(new Subscription(subscription));

                        }

                        break;

                }
                
            }

        }
    }
}
