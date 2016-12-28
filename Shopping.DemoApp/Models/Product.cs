namespace TeamWork.Models
{
    using System;
    using System.Collections.Generic;

    public class Product : BaseModel
    {
        
        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public string ImageUrl { get; set; }

        public Supplier  Supplier { get; set; }
        public virtual ICollection<OrderItem> OrderItems { get; set; }
    }
}
