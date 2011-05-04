using System;
using System.ServiceModel;

namespace TcpEmitter
{
    class Host
    {
        static void Main(string[] args)
        {

            Console.Title = "Second - TcpEmitter";
            Console.WriteLine("Begin");

            ServiceHost svc = new ServiceHost(typeof(global::TcpEmitter.TcpEmitter));

            svc.Open();

            Console.WriteLine("Press enter to conclude");

            Console.ReadKey();

        }
    }
}
