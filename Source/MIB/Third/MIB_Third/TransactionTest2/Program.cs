using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Transactions;
using System.Xml.Serialization;

namespace TransactionTest2
{
    class Program
    {
        private static TransactionScope tran;

        static void Main(string[] args)
        {

            tran = new TransactionScope(TransactionScopeOption.Required);



            Console.WriteLine(Transaction.Current.TransactionInformation.LocalIdentifier);
            Console.WriteLine(Transaction.Current.TransactionInformation.DistributedIdentifier);

            TcpClient tcp = new TcpClient("localhost",60);

            Transaction.Current.TransactionCompleted += new TransactionCompletedEventHandler(target);

            byte[] propagation = TransactionInterop.GetTransmitterPropagationToken(Transaction.Current);

            BinaryWriter serverStream = new BinaryWriter(tcp.GetStream());

            serverStream.Write(propagation.Length);

            serverStream.Write(propagation);

            while (true)
            {
                Console.WriteLine(Transaction.Current.TransactionInformation.LocalIdentifier);
                Console.WriteLine(Transaction.Current.TransactionInformation.DistributedIdentifier);
                Console.WriteLine(Transaction.Current.TransactionInformation.Status);
                Thread.Sleep(200);

               
            }

        }

        private static void target(object sender, TransactionEventArgs e)
        {

            Console.WriteLine(e.Transaction.TransactionInformation.Status);

        }
    }
}
