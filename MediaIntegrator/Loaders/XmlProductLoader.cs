using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace MediaIntegrator.Loaders
{
    class XmlProductLoader : IProductLoader
    {
        private readonly string _fileExtension = ".xml";
        private readonly string _fileName; // The path to the file where the products are saved
        public XmlProductLoader(string fileName)
        {
            _fileName = Path.Combine(Path.GetDirectoryName(fileName) ?? throw new InvalidOperationException(), Path.GetFileNameWithoutExtension(fileName) ?? throw new InvalidOperationException());
        }

        public List<Product> LoadProducts()
        {
            var productsXml = XDocument.Load(_fileName + _fileExtension);

            if (productsXml.Root != null)
            {
                List<Product> products =
                    productsXml.Root.Elements("Item")
                        .Select(productXml => new Product() { 
                            ID = (string) productXml.Element("ProductID"),
                            Name = (string) productXml.Element("Name"),
                            Stock = uint.Parse(productXml.Element("Count")?.Value ?? throw new InvalidOperationException()),
                            Price = decimal.Parse(productXml.Element("Price")?.Value ?? throw new InvalidOperationException()),
                            Comment = (string) productXml.Element("Comment"),
                            Artist = (string) productXml.Element("Artist"),
                            Publisher = (string) productXml.Element("Publisher"),
                            Genre = (string) productXml.Element("Genre"),
                            Year = uint.Parse(productXml.Element("Year")?.Value ?? throw new InvalidOperationException())
                        }).ToList();
                Console.WriteLine(DateTime.Now + ": Parsed the file: " + _fileName + _fileExtension);
                return products;
            }

            return null;
        }
        public void SaveProducts(List<Product> products)
        {
            var xDocument = new XDocument(new XElement("Inventory",
            from product in products
            select new XElement("Item",
                new XElement("Name", product.Name),
                new XElement("Count", product.Stock),
                new XElement("Price", product.Price.ToString(CultureInfo.InvariantCulture).Replace('.',',')),
                new XElement("Comment", product.Comment),
                new XElement("Artist", product.Artist),
                new XElement("Publisher", product.Publisher),
                new XElement("Genre", product.Genre),
                new XElement("Year", product.Year),
                new XElement("ProductID", product.ID))
            ));
            xDocument.Save(_fileName + _fileExtension);
            Console.WriteLine(DateTime.Now + ": File saved at: " + _fileName + _fileExtension);
        }
    }
}
