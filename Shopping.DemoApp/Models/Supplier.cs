namespace TeamWork.Models
{
    using System;
    using System.Collections.Generic;

    public class Supplier
    {
        public string Name { get; set; }
        public virtual ICollection<Product> Products { get; set; }
    }
}
