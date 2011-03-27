using System;
using System.ServiceModel;

namespace Second.TcpEmitter
{
    class Host
    {
        static void Main(string[] args)
        {

            Console.Title = "First - TcpEmitter";
            Console.WriteLine("Begin");

            ServiceHost svc = new ServiceHost(typeof(TcpEmitter));

            svc.Open();

            Console.WriteLine("Press enter to conclude");

            Console.ReadKey();

        }
    }
}
