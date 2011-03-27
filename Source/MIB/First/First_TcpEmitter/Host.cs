using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace First_TcpEmitter
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
