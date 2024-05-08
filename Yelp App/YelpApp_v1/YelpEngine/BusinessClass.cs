namespace YelpEngine
{
    public class BusinessClass
    {
        private readonly string businessId;
        private readonly string zip;
        private readonly string name;
        private readonly string city;
        private readonly string state;
        private readonly string address;
        private readonly double lat;
        private readonly double lon;
        private readonly int numTips;
        private readonly int numCheckins;
        private readonly string open;
        private readonly int stars;
        private readonly string fullAddress;

        public BusinessClass()
        {
        }

        public BusinessClass(string businessId, string zip, string name, string city, string state, string address, double lat, double lon, int numTips, int numCheckins, string open, int stars)
        {
            this.businessId = businessId;
            this.zip = zip;
            this.name = name;
            this.city = city;
            this.state = state;
            this.address = address;
            this.lat = lat;
            this.lon = lon;
            this.numTips = numTips;
            this.numCheckins = numCheckins;
            this.open = open;
            this.stars = stars;
            this.fullAddress = address + ", " + city + ", " + state + ", " + zip;
        }


        public string BusinessId => businessId;

        public string Zip => zip;

        public string Name => name;

        public string City => city;

        public string State => state;

        public string Address => address;

        public string FullAddress => fullAddress;
        
        public double Lat => lat;

        public double Lon => lon;

        public int NumTips => numTips;

        public int NumCheckins => numCheckins;

        public string Open => open;

        public int Stars => stars;
    }
}