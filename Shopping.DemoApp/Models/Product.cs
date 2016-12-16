namespace TeamWork.Models
{
    using System;

    public class Product
    {
        public string Id { get; set; }

        public DateTimeOffset? CreatedAt { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public string ImageUrl { get; set; }

        public Supplier  Supplier { get; set; }
    }
}
