using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaIntegrator.Loaders
{
    interface IProductLoader
    {
        List<Product> LoadProducts();
        void SaveProducts(List<Product> products);
    }
}
