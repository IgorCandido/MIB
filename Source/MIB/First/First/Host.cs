using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.ServiceModel;
using System.Text;
using First.Interfaces;
using First_Interfaces;
using First_Interfaces.ServiceInterfaces;

namespace First
{
    class Host
    {

        static IBlackbox blackbox = new BlackBox.Blackbox();

        static void Main(string[] args)
        {
            Console.Title = "First - Broker";
            Console.WriteLine("Begin");

            ServiceHost svc = new ServiceHost(typeof(BlackBox.Blackbox));

            svc.Open();

            Console.WriteLine("Press enter to conclude");

            Console.ReadKey();

        }
    }
}
