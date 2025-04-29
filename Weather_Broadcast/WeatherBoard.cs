using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Weather_Broadcast
{
    public partial class WeatherBoard : Form
    {
        List<City> cityList;
        private string currentUsername;

        public static Label LabelSpinner { get; set; }

        //form movement variables
        private bool isDragging = false;
        private Point newPoint = new Point(0, 0);

        public WeatherBoard()
        {
            InitializeComponent();
            FillListCityData();
            FillCityNamesToCityComboBox();
            lblUsername.Click += LblUsername_Click;
            picUserIcon.Click += PicUserIcon_Click;
            this.Load += WeatherBoard_Load;
        }

        public WeatherBoard(string username)
        {
            InitializeComponent();
            currentUsername = username;
            lblUsername.Text = currentUsername; // Your label to display username
            lblUsername.Click += LblUsername_Click;
            picUserIcon.Click += PicUserIcon_Click; // if you have a PictureBox for the user icon
        }

        private void LblUsername_Click(object sender, EventArgs e)
        {
            OpenProfileForm();
        }

        private void PicUserIcon_Click(object sender, EventArgs e)
        {
            OpenProfileForm();
        }

        private void OpenProfileForm()
        {
            ProfileForm profileForm = new ProfileForm(currentUsername); // pass current username
            profileForm.Show();
            this.Hide(); // optional: hide current form
        }


        private void FillListCityData()
        {
            cityList = new List<City>();

            foreach (var pair in Constant.MapOfCityNameAndID)
            {
                City city = new City(pair.Key);
                cityList.Add(city);
            }
        }

        private void FillCityNamesToCityComboBox()
        {
            List<string> cityNames = new List<string>();

            foreach (var city in cityList)
            {
                cityNames.Add(city.Name);
            }

            //cbCityList.DataSource = cityNames;
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            LabelSpinner.Visible = true;
            // get current selected city obj 
            string currentSelectedCityName = selectCityTextBox.Text;

            string connectionString = "Data Source=MAYUR5365\\SQLEXPRESS;Initial Catalog=WeatherDB;Integrated Security=True";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "INSERT INTO SearchHistory (Username, CityName) VALUES (@username, @city)";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@username", currentUsername);
                    cmd.Parameters.AddWithValue("@city", currentSelectedCityName);
                    cmd.ExecuteNonQuery();
                }
            }


            // pass it to API 
            API apiWeather = new API(currentSelectedCityName, this);
            //fetch api to get current weather data and store them in CurrentWeatherResponseFromAPI field
            API.FetchWeatherDataFromAPI(false);
            selectCityTextBox.Text = "";

        }

        private void WeatherBoard_Load(object sender, EventArgs e)
        {
            LabelSpinner = labelSpinner;
            labelSpinner.Visible = false;

            string username = UserSession.Username;
            lblUsername.Text = username;

            AutoCompleteStringCollection cityColl = new AutoCompleteStringCollection();

            string[] listCityNames = Helper.GetListOfCityNames();

            foreach (var cityname in listCityNames)
            {
                cityColl.Add(cityname);
            }

            selectCityTextBox.AutoCompleteMode = AutoCompleteMode.Suggest;
            selectCityTextBox.AutoCompleteSource = AutoCompleteSource.CustomSource;
            selectCityTextBox.AutoCompleteCustomSource = cityColl;
        }

        private new void Refresh_Click(object sender, EventArgs e)
        {
            Close();
        }

        //moves window around based on mouse movement
        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDragging)
            {
                Point p = PointToScreen(e.Location);
                Location = new Point(p.X - newPoint.X, p.Y - newPoint.Y);
            }
        }

        //when mouse is down on panel
        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            isDragging = true;
            newPoint = new Point(e.X, e.Y);

        }

        //when user lets go of mouse
        private void panel1_MouseUp(object sender, MouseEventArgs e)
        {
            isDragging = false;
        }

        //minimize
        private void button1_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void EnteringCity(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnConfirm.PerformClick();
            }
        }

        private void WeatherBoard_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Application.OpenForms.Count == 1)
                Application.Exit();
        }

        private void btnHistory_Click(object sender, EventArgs e)
        {
            this.Hide();

            SearchHistoryForm historyForm = new SearchHistoryForm(currentUsername);
            historyForm.Show();
        }

        public void FetchCityWeatherFromHistory(string cityName)
        {
            selectCityTextBox.Text = cityName;
            btnConfirm.PerformClick(); // Simulates the Confirm button click
        }
    }
}