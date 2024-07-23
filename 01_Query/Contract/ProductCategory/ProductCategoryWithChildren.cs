using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _01_Query.Contract.ProductCategory
{
    public class ProductCategoryWithChildren
    {
        public string Name { get; set; }
        public string Slug { get; set; }
        public List<ProductCategoryWithChildren> Children { get; set; }
    }
}
