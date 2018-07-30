using COMP2614_Assign06.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COMP2614_Assign06.Repositories
{
    class ProductRepo
    {
        
        //event listeners for database data update
        public delegate void DatabaseListener();
        public event DatabaseListener DatabaseUpdated;

        public void NotifySubscribers()
        {
            if (DatabaseUpdated != null)
            {
                DatabaseUpdated();
            }
        }

        public void AddProduct(Product product)
        {
            FoodStoreEntities context = new FoodStoreEntities();
            int maxId = GetNextProductId();
            Product newProduct = new Product
            {
                productID   = maxId,
                name        = product.name,
                price       = product.price,
                mfg         = product.mfg,
                vendor      = product.vendor
            };
            context.Products.Add(newProduct);
            context.SaveChanges();
            NotifySubscribers();
        }

        public int GetNextProductId()
        {
            FoodStoreEntities context = new FoodStoreEntities();
            //find max id of items in product table
            var query = context.Products
                        .Select(p => p.productID).ToList();
            int maxId = query.Max();
            return maxId + 1;
        }

        //adds all products to the table
        //if there is error adding a product, adds error message with line number to error property
        public void AddAll(InputTracker input)
        {
            List<Product> products = input.products;
            List<int> lineNumbers = input.lineNumbers;
            for (int i = 0; i < products.Count; i++)
            {
                try
                {
                    AddProduct(products[i]);
                } catch
                {
                    input.error += "An Error exists on line " 
                        + lineNumbers[i] + ". This entry has been skipped.\n";
                }
            }
        }

        public void DeleteProduct(int productID)
        {
            FoodStoreEntities context = new FoodStoreEntities();
            Product product = (from p in context.Products
                        where p.productID == productID
                        select p).FirstOrDefault();
            if (product != null)
            {
                try
                {
                    context.Products.Remove(product);
                    context.SaveChanges();
                    NotifySubscribers();
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }
    }
}
