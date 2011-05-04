using System;
using System.ServiceModel;
using Interfaces.Services;

namespace Blackbox
{
    class Host
    {

        static IBlackbox blackbox = new Blackbox();

        static void Main(string[] args)
        {
            //Console.Title = "Second - Broker";
            //Console.WriteLine("Begin");

            //ServiceHost svc = new ServiceHost(typeof(Blackbox));

            //svc.Open();

            //Console.WriteLine("Press enter to conclude");

            //Console.ReadKey();

        }
    }
}
