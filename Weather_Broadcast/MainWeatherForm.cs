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
    public partial class MainWeatherForm : Form
    {
        private Weather weather;

        //form movement variables
        private bool isDragging = false;
        private Point newPoint = new Point(0, 0);

        public MainWeatherForm(dynamic WeatherResponse)
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.Sizable;
            this.MaximizeBox = true;
            this.MinimizeBox = true;
            this.ControlBox = true;
            weather = new Weather(WeatherResponse, dailyDetailsControl1, cityLabel, dateLabel, labelMainCurrentTemp, labelMainCurrentDescription, pbMainCurrentIcon, weatherForecastPanel);
            weather.DisplayCurrentWeather();
            weather.DisplayWeatherForecast();
        }
        private void MainWeatherForm_Resize(object sender, EventArgs e)
        {
            // Example: Center the weather forecast panel
            weatherForecastPanel.Location = new Point((this.ClientSize.Width - weatherForecastPanel.Width) / 2, weatherForecastPanel.Location.Y);
        }

        private new void Refresh_Click(object sender, EventArgs e)
        {
            // get current selected city obj 
          //  string currentSelectedCityName = textBox1selectNewCityTextBox.Text;

            this.Close();
            // pass it to API 
        //    API apiWeather = new API(currentSelectedCityName);
            //fetch api to get current weather data and store them in CurrentWeatherResponseFromAPI field
           // API.FetchWeatherDataFromAPI();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDragging)
            {
                Point p = PointToScreen(e.Location);
                Location = new Point(p.X - newPoint.X, p.Y - newPoint.Y);
            }
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            isDragging = true;
            newPoint = new Point(e.X, e.Y);
        }

        private void panel1_MouseUp(object sender, MouseEventArgs e)
        {
            isDragging = false;
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            WidgetOption newWidget = new WidgetOption();
            newWidget.Show();
        }

        private void MainWeatherForm_Load(object sender, EventArgs e)
        {

        }

        private void newCity_Click(object sender, EventArgs e)
        {
            string username = UserSession.Username;

            WeatherBoard weatherBoard = new WeatherBoard(username);
            weatherBoard.Show();
            this.Hide();    
        }

        private void MainWeatherForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Application.OpenForms.Count >= 1)
                Application.Exit();
        }

        //about form
        private void button1_Click_1(object sender, EventArgs e)
        {
            About about = new About();
            about.Show();
        }
    }
}
