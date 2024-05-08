using System;
using System.Collections.Generic;
using System.Text;
using YelpEngine.BusinessDir;

namespace YelpEngine
{
    public class Filter
    {
        private Dictionary<string, string> attributeDictionary = new Dictionary<string, string>();
        
        private AttributeFilters attributesFilter = new AttributeFilters();
        private BusinessFilters businessFilters = new BusinessFilters();

        private List<string> attlist = new List<string>();
        private List<string> plist = new List<string>();
        private List<string> catlist = new List<string>();
        
        private string zip;

        public Filter(string zip, AttributeFilters attributesFilters, BusinessFilters businessFilters, List<string> catagories)
        {
            this.zip = zip;
            this.catlist = catagories;
            this.attributesFilter = attributesFilters;
            this.businessFilters = businessFilters;
            LoadBaseFilters();
        }

        private void LoadBaseFilters()
        {
            attributeDictionary.Add("BikeParking","True");
            attributeDictionary.Add("breakfast","True");
            attributeDictionary.Add("brunch","True");
            attributeDictionary.Add("BusinessAcceptsCreditCards","True");
            attributeDictionary.Add("dessert","True");
            attributeDictionary.Add("dinner","True");
            attributeDictionary.Add("GoodForKids","True");
            attributeDictionary.Add("latenight","True");
            attributeDictionary.Add("lunch","True");
            attributeDictionary.Add("OutdoorSeating","True");
            attributeDictionary.Add("RestaurantsDelivery","True");
            attributeDictionary.Add("RestaurantsGoodForGroups","True");
            attributeDictionary.Add("RestaurantsReservations","True");
            attributeDictionary.Add("RestaurantsTakeOut","True");
            attributeDictionary.Add("WheelchairAccessible","True");
            attributeDictionary.Add("WiFi","free");
            
            if (attributesFilter.AcceptCreditCardsChecked)
            {
                attlist.Add("BusinessAcceptsCreditCards");
            }
            if (attributesFilter.TakesReservationsChecked)
            {
                attlist.Add("RestaurantsReservations");
            }
            if (attributesFilter.WheelchairAccessibleChecked)
            {
                attlist.Add("WheelchairAccessible");
            }
            if (attributesFilter.OutdoorSeatingChecked)
            {
                attlist.Add("OutdoorSeating");
            }
            if (attributesFilter.GoodforKidsChecked)
            {
                attlist.Add("GoodForKids");
            }
            if (attributesFilter.GoodforGroupsChecked)
            {
                attlist.Add("RestaurantsGoodForGroups");
            }
            if (attributesFilter.DeliveryChecked)
            {
                attlist.Add("RestaurantsDelivery");
            }
            if (attributesFilter.TakeOutChecked)
            {
                attlist.Add("RestaurantsTakeOut");
            }
            if (attributesFilter.FreeWiFiChecked)
            {
                attlist.Add("WiFi");
            }
            if (attributesFilter.BikeParkingChecked)
            {
                attlist.Add("BikeParking");
            }
            if (attributesFilter.BreakfastChecked)
            {
                attlist.Add("breakfast");
            }
            if (attributesFilter.LunchChecked)
            {
                attlist.Add("lunch");
            }
            if (attributesFilter.DinnerChecked)
            {
                attlist.Add("dinner");
            }
            if (attributesFilter.BrunchChecked)
            {
                attlist.Add("brunch");
            }
            if (attributesFilter.DessertChecked)
            {
                attlist.Add("dessert");
            }
            if (attributesFilter.LateNightChecked)
            {
                attlist.Add("latenight");
            }

            if (businessFilters.Price1Checked) plist.Add("1");
            if (businessFilters.Price2Checked) plist.Add("2");
            if (businessFilters.Price3Checked) plist.Add("3");
            if (businessFilters.Price4Checked) plist.Add("4");
            
        }

        public string returnSQLString()
        {
            
            string start = $@"SELECT distinct business.* from business, categories WHERE business.business_id = categories.business_id and zipcode = '{zip.Trim()}'";
            string SQLLine = String.Empty;
            StringBuilder sb = new StringBuilder();
            sb.Append(start);

            foreach (string a in attlist)
            {
                sb.Append(@$" and ('{a}','{attributeDictionary[a]}') in (Select attr_name, value from attributes where business_id = business.business_id)");
            }
            
            foreach (string p in plist)
            {
                sb.Append(@$" and ('RestaurantsPriceRange2','{p}') in (Select attr_name, value from attributes where business_id = business.business_id)");
            }
            
            if (catlist.Count != 0)
                sb.Append($@"and category_name in ({listToSQLList(catlist)})");
            
            SQLLine = sb.ToString();
            sb.Clear();
            return SQLLine + ";";
        }
        
        private static string listToSQLList(List<string> some_list)
        {
            StringBuilder builder = new StringBuilder();
            foreach (string item in some_list)
            {
                if (some_list.IndexOf(item) != some_list.Count - 1)
                {
                    builder.Append($"'{item}'").Append(",");
                }
                else
                {
                    builder.Append($"'{item}'");
                }
            }
            return builder.ToString();
        }

    }
}