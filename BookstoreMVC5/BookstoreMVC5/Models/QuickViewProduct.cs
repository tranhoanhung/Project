using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookstoreMVC5.Models
{
    public class QuickViewProduct
    {
        public int ID { get; set; }
        public string ProductName { get; set; }
        public string CategoryName { get; set; }
        public string Images { get; set; }
        public string Details { get; set; }
        public double Price { get; set; }
        public Nullable<double> Pricesale { get; set; }
        public string Author { get; set; }
        public string Translator { get; set; }
        public string Pagesize { get; set; }
        public Nullable<int> Pagetotal { get; set; }

    }
}