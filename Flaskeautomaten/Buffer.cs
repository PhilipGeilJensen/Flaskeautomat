using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Flaskeautomaten
{
    class Buffer
    {
        Queue<string> products = new Queue<string>();

        public void AddProduct(string product)
        {
            Thread.Sleep(2000);
            lock (products)
            {
                while (products.Count == 10)
                {
                    Console.WriteLine("{0} - Waiting to add product", Thread.CurrentThread.Name);
                    Monitor.Wait(products);
                }


                products.Enqueue(product);
                //Console.WriteLine("{0} - added a {1}",Thread.CurrentThread.Name, product);
                Monitor.PulseAll(products);
            }
        }

        public string PullProduct()
        {
            string product;
            Thread.Sleep(2000);
            lock (products)
            {
                while (products.Count == 0)
                {
                    Console.WriteLine("{0} - Waiting to pull product", Thread.CurrentThread.Name);
                    Monitor.Wait(products);
                }


                product = products.Dequeue();
                Monitor.PulseAll(products);
            }
            return product;
        }
    }
}
