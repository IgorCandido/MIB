using System;

namespace TcpReceiver
{
    class Host
    {
        static void Main(string[] args)
        {
            Console.Title = "Second - TcpReceiver";
            Console.WriteLine("Begin");

            //ServiceHost svc = new ServiceHost(typeof(global::TcpReceiver.TcpReceiver));

            //svc.Open();

            //Console.WriteLine("Press enter to conclude");

            //Console.ReadKey();

            TcpReceiver receiver = new TcpReceiver();

            Console.WriteLine("Press enter to conclude");

            Console.ReadKey();

        }
    }
}
