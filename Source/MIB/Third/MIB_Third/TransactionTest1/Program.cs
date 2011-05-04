using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Transactions;

namespace TransactionTest1
{
    class Program
    {
        static void Main(string[] args)
        {

            //TcpListener listener = new TcpListener(60);

            //listener.Start();

            //TcpClient client = listener.AcceptTcpClient();

            //BinaryReader clientStream = new BinaryReader(client.GetStream());

            //int dimension = clientStream.ReadInt32();

            //Transaction tran = TransactionInterop.GetTransactionFromTransmitterPropagationToken(clientStream.ReadBytes(dimension));

            //Console.WriteLine(tran.TransactionInformation.LocalIdentifier);
            //Console.WriteLine(tran.TransactionInformation.DistributedIdentifier);

            //TransactionScope trans = new TransactionScope(tran);

            //trans.Complete();

        }
    }
}
