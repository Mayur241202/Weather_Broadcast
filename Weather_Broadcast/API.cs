using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Weather_Broadcast
{
    public class API
    {
        public static string currentUsername { get; private set; }
        public static string SelectedCity { get; private set; }
        private static dynamic DataResponseFromAPI { get; set; }
        private static dynamic CurrentWeatherInfo { get; set; }     
        private static dynamic ForecastWeatherInfo { get; set; }
        public static MainWeatherForm MainWeatherForm { get; private set; }
        public static WeatherBoard WeatherBoardForm { get; set; }
        
       
        public API(string city)
        {
            SelectedCity = city;
        }

        public API(string city, WeatherBoard weatherBoard)
        {
            SelectedCity = city;
            WeatherBoardForm = weatherBoard;
        }

        public static dynamic GetCurrentWeatherResponseFromAPI()
        {
            return DataResponseFromAPI;
        }

        public async static void FetchWeatherDataFromAPI(bool widget)
        {
            var url = Constant.FETCH_WEATHER_URL + Constant.API_KEY + "+&q=" + SelectedCity + "&days=" + Constant.NUMBER_OF_WEATHER_FORECAST_DAYS;

            try
            {
                //fetch the result from api.
                using (HttpClient client = new HttpClient())
                using (HttpResponseMessage response = await client.GetAsync(url))
                using (HttpContent content = response.Content)
                {
                    // ... Read the string.
                    string result = await content.ReadAsStringAsync();
                    //assign the json data, parsed
                    DataResponseFromAPI = JsonConvert.DeserializeObject<dynamic>(result);

                    WeatherBoard.LabelSpinner.Visible = false;

                    if (DataResponseFromAPI != null)
                    {
                        if (widget)
                        {
                            Widget Widget = new Widget(DataResponseFromAPI);
                            Widget.Show();
                        }
                        else
                        {
                            MainWeatherForm = new MainWeatherForm(DataResponseFromAPI);
                            MainWeatherForm.Show();
                            WeatherBoardForm.Close();
                        }
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Invalid city name. Try again!!!");
            }
            
        }

        public async static Task<dynamic> FetchWeatherDataForCity(string city)
        {
            var url = Constant.FETCH_WEATHER_URL + Constant.API_KEY + "+&q=" + city + "&days=" + Constant.NUMBER_OF_WEATHER_FORECAST_DAYS;

            try
            {
                using (HttpClient client = new HttpClient())
                using (HttpResponseMessage response = await client.GetAsync(url))
                using (HttpContent content = response.Content)
                {
                    string result = await content.ReadAsStringAsync();
                    dynamic responseData = JsonConvert.DeserializeObject<dynamic>(result);
                    return responseData;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error fetching data: " + ex.Message);
                return null;
            }
        }
    }
}
