using System;

namespace BookstoreMVC5.Models
{
    [Serializable]
    public class CartItem
    {
        public Book book { get; set; }

        public int quantity { get; set; }

        public int countCart { get; set; }

        public string meThod { get; set; }

        public long priceTotal { get; set; }

        public long priceSaleee { get; set; }
    }
}