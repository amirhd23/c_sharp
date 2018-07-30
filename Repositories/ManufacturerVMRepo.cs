using COMP2614_Assign06.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COMP2614_Assign06.Repositories
{
    class ManufacturerVMRepo
    {
        

        public List<ManufacturerVM> GetAllManufacturers()
        {
            FoodStoreEntities context = new FoodStoreEntities();
            List<ManufacturerVM> items = context.Manufacturers
                .Select(m => new ManufacturerVM { ManufacturerName = m.mfg })
                .Distinct().ToList();
            return items;

        }

        
    }
}
