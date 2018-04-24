using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace MediaIntegrator.Loaders
{
    class XmlProductLoader : IProductLoader
    {
        public List<Product> LoadProducts()
        {
            XDocument productsXml = XDocument.Load("../../simplemedia.xml");

            List<Product> products =
                productsXml.Root.Elements("Item")
                .Select(h => new Product() { 
                    ID = (string) h.Element("ProductID"),
                    Name = (string) h.Element("Name"),
                    Stock = uint.Parse(h.Element("Count").Value),
                    Price = decimal.Parse(h.Element("Price").Value),
                    Comment = (string) h.Element("Comment"),
                    Artist = (string) h.Element("Artist"),
                    Publisher = (string) h.Element("Publisher"),
                    Genre = (string) h.Element("Genre"),
                    Year = uint.Parse(h.Element("Year").Value)
                })
                .ToList();
            return products;
        }
        public void SaveProducts(List<Product> products)
        {
            var xDoc = new XDocument(new XElement("Inventory",
            from product in products
            select new XElement("Item",
                new XElement("Name", product.Name),
                new XElement("Count", product.Stock),
                new XElement("Price", product.Price.ToString().Replace('.',',')),
                new XElement("Comment", product.Comment),
                new XElement("Artist", product.Artist),
                new XElement("Publisher", product.Publisher),
                new XElement("Genre", product.Genre),
                new XElement("Year", product.Year),
                new XElement("ProductID", product.ID))
            ));
            xDoc.Save("../../test.xml");
        }
    }
}
