using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COMP2614_Assign06.Utils
{
    class FileReader
    {
        //reads products from a csv file
        //file format: name, price, manufacturer, supplier 
        public static InputTracker ReadCsvFile(string filePath)
        {
            StreamReader sr = new StreamReader(filePath, false);
            List<Product> products  = new List<Product>();
            List<int> lineNumbers = new List<int>();
            string error = string.Empty;
            int lineCounter = 1;
            while (!sr.EndOfStream)
            {
                string line = sr.ReadLine();
                Product product = readOneProduct(line);
                if (product != null)
                {
                    products.Add(product);
                    lineNumbers.Add(lineCounter);
                } else
                {
                    error += "An error exists on line " + lineCounter + ". This entry has been skipped\n.";
                }
                lineCounter++;
            }
            InputTracker tracker = new InputTracker
            {
                products = products,
                lineNumbers = lineNumbers,
                error = error
            };
            return tracker;

        }

        //file format: name, price, manufacturer, supplier 
        private static Product readOneProduct(string productCSV)
        {
            const int NUM_OF_FIELDS = 4;
            const int NAME          = 0;
            const int MFG           = 1;
            const int PRICE         = 2;
            const int SUPPLIER      = 3;
            string[] fields = productCSV.Trim().Split(',');
            if (fields.Count() != NUM_OF_FIELDS)
            {
                return null;
            }
            decimal price;
            bool success = decimal.TryParse(fields[PRICE].Trim(), out price);
            if (!success)
            {
                return null;
            }
            if (!Validator.ValidateAlphabetic(fields[NAME].Trim())) {
                return null;
            }
            Product product = new Product
            {
                name = fields[NAME].Trim(),
                price = price,
                mfg = fields[MFG].Trim(),
                vendor = fields[SUPPLIER].Trim()
            };
            return product;
        }
    }

    class InputTracker
    {
        public List<Product> products { get; set; }
        public List<int> lineNumbers { get; set; }
        public string error { get; set; }
    }
}
