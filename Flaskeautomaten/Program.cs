using System;
using System.Collections.Generic;
using System.Threading;

namespace Flaskeautomaten
{
    class Program
    {
        static Buffer buffer1 = new Buffer();
        static Buffer bufferSoda = new Buffer();
        static Buffer bufferBeer = new Buffer();
        static void Main(string[] args)
        {
            Thread producer = new Thread(Producer);
            Thread splitter = new Thread(Splitter);
            Thread conSoda = new Thread(Consumer);
            Thread conBeer = new Thread(Consumer);
            producer.Name = "Producer";
            splitter.Name = "Splitter";
            conSoda.Name = "Soda Consumer";
            conBeer.Name = "Beer Consumer";

            producer.Start();
            splitter.Start();
            conSoda.Start(bufferSoda);
            conBeer.Start(bufferBeer);
        }

        static void Producer()
        {
            List<string> bottles = new List<string>() { "Sodavand", "Øl" };
            Random rand = new Random();
            while (true)
            {
                string product = bottles[rand.Next(0, bottles.Count)];
                buffer1.AddProduct(product);
            }
        }

        static void Splitter()
        {
            while (true)
            {
                string product = buffer1.PullProduct();

                if (product == "Sodavand")
                {
                    bufferSoda.AddProduct(product);
                    Console.WriteLine("The splitter added a soda to the soda buffer");
                } else
                {
                    bufferBeer.AddProduct(product);
                    Console.WriteLine("The splitter added a beer to the beer buffer");
                }
            }
        }

        static void Consumer(object buffer)
        {
            Buffer buf = buffer as Buffer;
            while (true)
            {
                Thread.Sleep(5000);
                string product = buf.PullProduct();
                Console.WriteLine("{0} has pulled a {1} from the machine", Thread.CurrentThread.Name, product);
            }
        }
    }
}
