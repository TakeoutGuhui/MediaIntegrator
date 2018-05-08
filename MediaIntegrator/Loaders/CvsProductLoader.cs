using Microsoft.VisualBasic.FileIO;
using System.Globalization;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System;

namespace MediaIntegrator.Loaders
{
    /// <summary>
    /// This class handles saving and loading products in csv-format to/from disk
    /// </summary>
    class CsvProductLoader : IProductLoader
    {
        private readonly string _fileName; // The path to the file where the products are saved
        private readonly string _fileExtension = ".csv";
        private readonly string _delimiter = ";"; // The delimiter that should be used to separate the values in the file
        
        public CsvProductLoader(string fileName)
        {
            _fileName = Path.Combine(Path.GetDirectoryName(fileName) ?? throw new InvalidOperationException(), Path.GetFileNameWithoutExtension(fileName) ?? throw new InvalidOperationException());
        }

        /// <summary>
        /// Loads the products from the file that _filePath points to
        /// </summary>
        /// <returns></returns>
        public List<Product> LoadProducts()
        {
            var parser = new TextFieldParser(_fileName + _fileExtension);
            var products = new List<Product>();
            parser.TextFieldType = FieldType.Delimited;
            parser.SetDelimiters(_delimiter);
            while (!parser.EndOfData)
            {
                string[] fields = parser.ReadFields();
                if (fields != null && fields.Length == 9)
                {
                    var productNumber = fields[0];
                    var name = fields[1];
                    decimal.TryParse(fields[2], NumberStyles.Any, new CultureInfo("sv-SE"), out var price);
                    uint.TryParse(fields[3], out var stock);
                    var artist = fields[4];
                    var publisher = fields[5];
                    var genre = fields[6];
                    uint.TryParse(fields[7], out var year);
                    var comment = fields[8];
                    var product = new Product()
                    {
                        ID = productNumber,
                        Name = name,
                        Price = price,
                        Stock = stock,
                        Artist = artist,
                        Publisher = publisher,
                        Genre = genre,
                        Year = year,
                        Comment = comment
                    };
                    products.Add(product);
                }

            }
            Console.WriteLine(DateTime.Now + ": Parsed the file: " + _fileName + _fileExtension);
            return products;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private string EscapeDelimeter(string data)
        {
            if (data.Contains(_delimiter))
            {
                data = $"\"{data}\"";
            }

            return data;
        }

        /// <summary>
        /// Converts the product to csv-format
        /// </summary>
        /// <param name="product"> The product that will be converted </param>
        /// <returns></returns>
        private string ConvertToCsv(Product product)
        {
            return
                $"{EscapeDelimeter(product.ID)};{EscapeDelimeter(product.Name)};{product.Price};{product.Stock};{EscapeDelimeter(product.Artist)};" +
                $"{EscapeDelimeter(product.Publisher)};{EscapeDelimeter(product.Genre)};{product.Year};{EscapeDelimeter(product.Comment)}";
        }

        /// <summary>
        /// Saves the products to file
        /// </summary>
        /// <param name="products"> The products that will be saved to file </param>
        public void SaveProducts(List<Product> products)
        {
            StringBuilder stringBuilder = new StringBuilder();
            foreach (var product in products)
            {
                string productLine = ConvertToCsv(product);
                stringBuilder.AppendLine(productLine);
            }
            File.WriteAllText(_fileName + _fileExtension, stringBuilder.ToString());
            Console.WriteLine(DateTime.Now + ": File saved at: " + _fileName + _fileExtension);
        }
    }
}
