using MediaIntegrator.Loaders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaIntegrator
{
    class ProductList
    {
        /// <summary>
        /// The taken ID's
        /// </summary>
        private readonly HashSet<string> TakenIDs = new HashSet<string>();

        /// <summary>
        /// The taken names
        /// </summary>
        private readonly HashSet<string> TakenNames = new HashSet<string>();

        /// <summary>
        /// Checks if the parameter id is already taken
        /// </summary>
        /// <param name="id"> the id </param>
        /// <returns> Returns True if the id is already taken </returns>
        public bool IsIdTaken(string id)
        {
            return TakenIDs.Contains(id);
        }

        /// <summary>
        /// Checks if the name is already taken
        /// </summary>
        /// <param name="name"> the name </param>
        /// <returns> Returns True if the name is already taken </returns>
        public bool IsNameTaken(string name)
        {
            return TakenNames.Contains(name);
        }

        public List<Product> Products { get; set; }

        /// <summary>
        /// Used to load the products from source
        /// </summary>
        private readonly IProductLoader _productLoader;

        private ProductList(IProductLoader productLoader)
        {
            Products = new List<Product>();
            _productLoader = productLoader;
            //Products.CollectionChanged += ProductAddRemove;
        }

        public void LoadProducts()
        {
            List<Product> loadedProducts = _productLoader.LoadProducts();

            foreach (Product product in loadedProducts)
            {
                AddProduct(product);
            }
        }

        /// <summary>
        /// Adds a product to the list
        /// </summary>
        /// <param name="product"> The product that should be added </param>
        public void AddProduct(Product product)
        {
            if (IsIdTaken(product.ID) || IsNameTaken(product.Name)) return; // If the ID or name is already cancel the add
            TakenIDs.Add(product.ID); // Adds the product's ID to the taken ID's
            TakenNames.Add(product.Name); // Adds the product's name to the taken names
            Products.Add(product); // Adds the product to the Product list

        }

        public void AddProductList(List<Product> productList)
        {
            foreach (Product product in productList)
            {
                AddProduct(product);
            }
        }
    }
}
