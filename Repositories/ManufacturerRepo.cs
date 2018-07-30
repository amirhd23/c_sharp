using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COMP2614_Assign06.Repositories
{
    class ManufacturerRepo
    {
        
        public void AddManufacturer(Manufacturer newMfg)
        {
            FoodStoreEntities context = new FoodStoreEntities();
            Manufacturer manufacturer = new Manufacturer
            {
                mfg = newMfg.mfg,
                mfgDiscount = newMfg.mfgDiscount
            };
            try
            {
                context.Manufacturers.Add(manufacturer);
                context.SaveChanges();
            } catch (Exception e)
            {
                throw e;
            }
        }

    }
}
