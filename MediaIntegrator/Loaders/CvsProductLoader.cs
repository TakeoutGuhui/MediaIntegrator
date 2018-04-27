using Microsoft.VisualBasic.FileIO;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Text;

using MediaIntegrator;
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
            _fileName = Path.Combine(Path.GetDirectoryName(fileName), Path.GetFileNameWithoutExtension(fileName));
        }

        /// <summary>
        /// Loads the products from the file that _filePath points to
        /// </summary>
        /// <returns></returns>
        public List<Product> LoadProducts()
        {
            TextFieldParser parser = new TextFieldParser(_fileName + _fileExtension);
            List<Product> products = new List<Product>();
            parser.TextFieldType = FieldType.Delimited;
            parser.SetDelimiters(_delimiter);
            while (!parser.EndOfData)
            {
                string[] fields = parser.ReadFields();
                uint stock, year;
                decimal price;
                if (fields != null && fields.Length == 9)
                {
                    string productNumber = fields[0];
                    string name = fields[1];
                    decimal.TryParse(fields[2], NumberStyles.Any, new CultureInfo("sv-SE"), out price);
                    uint.TryParse(fields[3], out stock);
                    string artist = fields[4];
                    string publisher = fields[5];
                    string genre = fields[6];
                    uint.TryParse(fields[7], out year);
                    string comment = fields[8];
                    Product product = new Product()
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
                data = string.Format("\"{0}\"", data);
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
            return string.Format("{0};{1};{2};{3};{4};{5};{6};{7};{8}",
                EscapeDelimeter(product.ID),
                EscapeDelimeter(product.Name),
                product.Price,
                product.Stock,
                EscapeDelimeter(product.Artist),
                EscapeDelimeter(product.Publisher),
                EscapeDelimeter(product.Genre),
                product.Year,
                EscapeDelimeter(product.Comment));
        }

        /// <summary>
        /// Saves the products to file
        /// </summary>
        /// <param name="products"> The products that will be saved to file </param>
        public void SaveProducts(List<Product> products)
        {
            StringBuilder stringBuilder = new StringBuilder();
            foreach (Product product in products)
            {
                string productLine = ConvertToCsv(product);
                stringBuilder.AppendLine(productLine);
            }
            File.WriteAllText(_fileName + _fileExtension, stringBuilder.ToString());
            Console.WriteLine(DateTime.Now + ": File saved at: " + _fileName + _fileExtension);
        }
    }
}
