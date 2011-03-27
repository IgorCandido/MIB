using System;
using System.ServiceModel;

namespace Second.TcpReceiver
{
    class Host
    {
        static void Main(string[] args)
        {
            Console.Title = "First - TcpReceiver";
            Console.WriteLine("Begin");

            ServiceHost svc = new ServiceHost(typeof(TcpReceiver));

            svc.Open();

            Console.WriteLine("Press enter to conclude");

            Console.ReadKey();

        }
    }
}
