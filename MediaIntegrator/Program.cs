using MediaIntegrator.Loaders;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaIntegrator
{
    class Program
    {
        static void Main(string[] args)
        {
            if(!Directory.Exists("fromMediaShop"))
                Directory.CreateDirectory("fromMediaShop");
            if(!Directory.Exists("toSimpleMedia"))
                Directory.CreateDirectory("toSimpleMedia");
            /*
            XmlProductLoader loader = new XmlProductLoader();
            List<Product> products = loader.LoadProducts();
            int i;
            */
            XmlProductLoader xmlLoader = new XmlProductLoader();
            CsvProductLoader csvLoader = new CsvProductLoader("../../MediaShop.csv");
            xmlLoader.SaveProducts(csvLoader.LoadProducts());
            
        }
    }
}
