using COMP2614_Assign06.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COMP2614_Assign06.Repositories
{
    class SupplierRepo
    {
        public List<SupplierVM> GetDistinctSuppliers()
        {
            FoodStoreEntities context = new FoodStoreEntities();
            List<SupplierVM> suppliers = context.Suppliers
                        .Select(s => new SupplierVM { SupplierName = s.vendor })
                        .Distinct().ToList();
            return suppliers;
        }

        public Supplier GetSupplier(string supplierName)
        {
            FoodStoreEntities context = new FoodStoreEntities();
            return (from s in context.Suppliers
                    where s.vendor == supplierName
                    select s).FirstOrDefault();
        }
    }
}
