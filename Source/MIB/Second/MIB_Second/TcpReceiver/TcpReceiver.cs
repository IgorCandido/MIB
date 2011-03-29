using System;
using System.Configuration;
using System.IO;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.ServiceModel;
using Interfaces.Contracts;
using Interfaces.Factories;
using Interfaces.Services;
using Interfaces.Tcp;
using Interfaces.Tcp.ClientObjects;

namespace TcpReceiver
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Single, InstanceContextMode = InstanceContextMode.PerCall)]
    public class TcpReceiver : IReceiver
    {

        private static readonly IBlackbox Blackbox = 
            ProxyFactory.GetProxy<IBlackbox>("blackbox");

        private readonly TcpListener _listener = new TcpListener(int.Parse(ConfigurationManager.AppSettings["PortNumber"]));

        public TcpReceiver()
        {

            _listener.Start();

            _listener.BeginAcceptTcpClient(ConnectionMade, _listener);

        }
       
        #region Implementation of IReceiver

        [OperationBehavior(TransactionAutoComplete = true, TransactionScopeRequired = false)]
        public void Receive(EventContract eventContract)
        {
            
            Blackbox.Receive(eventContract.Topic, eventContract.Data);

        }


        [OperationBehavior(TransactionAutoComplete = true, TransactionScopeRequired = false)]
        public void Subscribe(SubscriptionContract subscriptionContract)
        {
            
            Blackbox.Subscribe(subscriptionContract.Topic, subscriptionContract.ClientInformation);

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
                            
                            Blackbox.Receive(eventObj.Topic, eventObj.Data);

                        }

                        break;

                    case Commands.Subscription:

                        SubscriptionContract subscription;

                        if ((subscription = formatter.Deserialize(deserializationStream) as SubscriptionContract) != null)
                        {

                            Blackbox.Subscribe(subscription.Topic, subscription.ClientInformation);

                        }

                        break;

                }
                
            }

        }
    }
}
