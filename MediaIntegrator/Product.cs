using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows;

namespace MediaIntegrator
{
    /// <summary>
    /// Class that represents a product
    /// </summary>
    internal class Product
    {
        /// <summary>
        /// The properties below represents the properties of a product
        /// </summary>
        public string ID { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public uint Stock { get; set; }
        public string Artist { get; set; }
        public string Genre { get; set; }
        public string Publisher { get; set; }
        public uint Year { get; set; }
        public string Comment { get; set; }
    }
}
