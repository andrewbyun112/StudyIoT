﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BogusTestApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var repository = new SampleCustomerRepository();
            var customers = repository.GetCustomers();
            //Console.WriteLine(customers);
            Console.WriteLine(JsonConvert.SerializeObject(customers, Formatting.Indented));
        }
    }
}
