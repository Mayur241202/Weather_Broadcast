using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Weather_Broadcast
{
    public partial class WidgetOption : Form
    {
        public WidgetOption()
        {
            InitializeComponent();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            string city = selectCity.Text.Trim();

            if (string.IsNullOrEmpty(city))
            {
                MessageBox.Show("Please enter a city name.");
                return;
            }

            // Fetch weather data asynchronously for the given city
            dynamic weatherData = await API.FetchWeatherDataForCity(city); // Ensure this method exists in API.cs

            if (weatherData != null)
            {
                // Create a new widget instance for the selected city
                Widget newWidget = new Widget(weatherData);
                newWidget.Show(); // Show non-modal (so multiple widgets can be opened)
            }
            else
            {
                MessageBox.Show("Failed to fetch weather data. Try a valid city.");
            }

            // Optionally close the options form
            this.Close();
        }


        private void WidgetOption_Load(object sender, EventArgs e)
        {
            //selectCity
            AutoCompleteStringCollection cityColl = new AutoCompleteStringCollection();

            string[] listCityNames = Helper.GetListOfCityNames();

            foreach (var cityname in listCityNames)
            {
                cityColl.Add(cityname);
            }

            selectCity.AutoCompleteMode = AutoCompleteMode.Suggest;
            selectCity.AutoCompleteSource = AutoCompleteSource.CustomSource;
            selectCity.AutoCompleteCustomSource = cityColl;
        }

        private void ConfirmCity(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                button1.PerformClick();
            }
        }
    }
}
