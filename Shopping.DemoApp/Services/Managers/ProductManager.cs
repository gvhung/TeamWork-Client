
using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics;
using TeamWork.Models;
using TeamWork;
using System;
using System.Collections.Generic;
using System.Net.Http;

namespace TeamWork
{
	public class ProductManager : BaseManager<Product>
	{
		public override string Identifier => "Product";

        public Task<DateTime?> StartSale(Product product)
        {
            return new Task<DateTime?>(() => {
                var qs = new Dictionary<string, string>();
                qs.Add("id", product.Id);
                var dateTime = AzureService.Instance.Client.InvokeApiAsync("StartSale", null, HttpMethod.Post, qs).Result;
                return (DateTime)dateTime.Root;
            });
        }
    }
}