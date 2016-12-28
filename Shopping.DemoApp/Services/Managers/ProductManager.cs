
using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics;
using TeamWork.Models;
using TeamWork;

namespace TeamWork
{
	public class ProductManager : BaseManager<Product>
	{
		public override string Identifier => "Product";

		
	}
}