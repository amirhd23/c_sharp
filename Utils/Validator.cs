using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace COMP2614_Assign06.Utils
{
    class Validator
    {
        //validates a string to be only alphabetical
        public static bool ValidateAlphabetic(string input)
        {
            //only alphabetical chars are allowed for product name
            Regex regex = new Regex(@"^[A-Za-z ]+$");
            Match match = regex.Match(input);
            return match.Success ? true : false;
            
        }

        public static bool ValidateDecimal(string input)
        {
            decimal parsedValue;
            bool success = decimal.TryParse(input, out parsedValue);
            return success;
        }

        public static bool ValidateInteger(string input)
        {
            int parsedValue;
            bool success = int.TryParse(input, out parsedValue);
            return success;
        }
    }
}
