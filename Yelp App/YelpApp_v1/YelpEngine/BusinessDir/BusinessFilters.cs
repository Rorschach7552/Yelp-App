using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YelpEngine
{
    public class BusinessFilters
    {
        public bool Price1Checked { get; set; }
        public bool Price2Checked { get; set; }
        public bool Price3Checked { get; set; }
        public bool Price4Checked { get; set; }

        public string GetPriceFilterValueList()
        {
            var priceValues = new List<string>();
            if (Price1Checked) priceValues.Add("'1'");
            if (Price2Checked) priceValues.Add("'2'");
            if (Price3Checked) priceValues.Add("'3'");
            if (Price4Checked) priceValues.Add("'4'");

            var result = string.Join(",", priceValues);
            return result;
        }

        public bool PriceFilterApplied()
        {
            return Price1Checked || Price2Checked || Price3Checked || Price4Checked;
        }
    }
}
