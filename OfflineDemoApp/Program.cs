using Klarna.Entities;
using Klarna.Offline;
using Klarna.Offline.Entities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OfflineDemoApp
{
    class Program
    {

        static void Main(string[] args)
        {
            KlarnaIntegration k = new KlarnaIntegration();
            Console.WriteLine("#################");
            Console.WriteLine("Klarna Offline Demo app");
            Console.WriteLine("Pick your scenario");
            Console.WriteLine("1. Offline with polling");
            Console.WriteLine("2. Offline with push to status_Url");
            string selection = Console.ReadLine();
            switch (selection)
            {
                case "1":
                    k.StartOrderWithPolling();
                    break;
                case "2":
                    k.StartOrderWithStatusUrl();
                    break;
                default:
                    Console.WriteLine("Incorrect selection");
                    break;
            }

        }

    }
}
