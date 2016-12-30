
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

        internal async Task<Product> StartSale(Product product)
        {
            return await AzureService.Instance.Client.InvokeApiAsync<Product, Product>("StartSale", product);
        }
    }
}