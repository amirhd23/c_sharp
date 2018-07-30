using COMP2614_Assign06.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COMP2614_Assign06.Repositories
{
    class ProductVMRepo
    {
        
        //join Product table with Manufacturer and Supplier to create a List of ProductVM. 
        public List<ProductVM> getAllProductsInformation()
        {
            FoodStoreEntities context = new FoodStoreEntities();
            var query1 = from product in context.Products
                         from manufacturer in context.Manufacturers
                         where manufacturer.mfg == product.mfg
                         select new
                         {
                             product.productID,
                             product.name,//product description
                             product.price,
                             product.mfg,//name of manufacturer
                             manufacturer.mfgDiscount,
                             product.vendor,//supplier
                         };
            var query2 = (from item in query1
                         from supplier in context.Suppliers
                         where supplier.vendor == item.vendor
                         select new
                         {
                             ID =               item.productID,
                             Description =      item.name,
                             Price =            item.price,
                             Manufacturer =     item.mfg,
                             Mfg_Discount =     item.mfgDiscount,
                             Supplier =         item.vendor,
                             SupplierContact =  supplier.supplier_email
                         }).ToList();
            var query3 = (from item in query2
                         select new ProductVM
                         {
                             ID =               item.ID,
                             Description =      item.Description,
                             Price =            item.Price.Value.ToString("C"),
                             Manufacturer =     item.Manufacturer,
                             Mfg_Discount =     item.Mfg_Discount.ToString(),
                             Supplier =         item.Supplier,
                             SupplierContact =  item.SupplierContact
                         }).ToList();
            return query3;

        }

    }
}
