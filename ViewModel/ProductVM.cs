using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COMP2614_Assign06.ViewModel
{
    class ProductVM
    {
        public int      ID              { get; set; }
        public string   Description     { get; set; }
        public string   Price           { get; set; }
        public string   Manufacturer    { get; set; }
        public string   Mfg_Discount    { get; set; }
        public string   Supplier        { get; set; }
        public string   SupplierContact { get; set; }
    }
}
