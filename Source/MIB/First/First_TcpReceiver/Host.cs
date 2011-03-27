using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace First_TcpReceiver
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
