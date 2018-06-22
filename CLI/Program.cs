using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FritzBoxSoap;
using System.Threading;

namespace CLI
{
    class Program
    {
        static void Main(string[] args)
        {
            FritzBoxSoap.FritzBoxSoap s = new FritzBoxSoap.FritzBoxSoap("192.168.178.1","-");
            while(true)
            {
                Thread.Sleep(1000);
                Console.WriteLine(s.getPercentageUsageDownStream() + "%");
            
            }
        }
    }
}
