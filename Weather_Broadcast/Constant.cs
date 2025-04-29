using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Weather_Broadcast
{
    class Constant
    {
        // this dictionary holds cityname and city id in pair
        public static Dictionary<string, int> MapOfCityNameAndID = new Dictionary<string, int>()
        {
            { "Mumbai", 1275339 },
            { "Delhi", 1273294 },
            { "Bangalore", 1277333 },
            { "Pune", 1259229 },
            { "Hyderabad", 1269843 },
            { "Chennai", 1264527 },
            { "Kolkata", 1275004 },
            { "Nagpur", 1262180 },
            { "Ahmedabad", 1279233 },
            { "Jaipur", 1269515 },
            { "Calgary", 5913490 },
            { "Toronto", 6087824 },
            { "Vancouver", 6090785 },
            { "Saskatoon", 6141256 },
            { "Quebec", 6325494 },
            { "Airdrie", 5882799 },
            { "Medicine Hat", 6071618},
            { "Banff", 5892532 },
            { "California", 4350049 },
            { "Florida", 3851244 }
        };

        // API KEY
        public static string API_KEY = "c95760456e304111994133755250704";

        // URL TO FETCH
        public static string FETCH_WEATHER_URL = "http://api.weatherapi.com/v1/forecast.json?key=";
        public static int NUMBER_OF_WEATHER_FORECAST_DAYS = 5;
    }
}
