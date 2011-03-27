using System;
using System.ServiceModel;
using Second.BlackBox;
using Second.Interfaces.ServiceInterfaces;

namespace Second
{
    class Host
    {

        static IBlackbox blackbox = new Blackbox();

        static void Main(string[] args)
        {
            Console.Title = "First - Broker";
            Console.WriteLine("Begin");

            ServiceHost svc = new ServiceHost(typeof(Blackbox));

            svc.Open();

            Console.WriteLine("Press enter to conclude");

            Console.ReadKey();

        }
    }
}
