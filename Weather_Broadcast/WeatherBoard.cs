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
            this.Load += WeatherBoard_Load;
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

            if (string.IsNullOrWhiteSpace(currentSelectedCityName))
            {
                MessageBox.Show("Please enter a city name.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                LabelSpinner.Visible = false;
                return;
            }

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

            // If user has a default city stored in UserSession, populate the search box
            if (!string.IsNullOrEmpty(UserSession.DefaultCity))
            {
                selectCityTextBox.Text = UserSession.DefaultCity;

                // Optionally, you can automatically click the Confirm button to get weather data
                // Uncomment the following line if you want this behavior
                // btnConfirm.PerformClick();
            }
            this.FormClosing += FormUtils.HandleFormClosing;
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

        // NEW FAVORITE METHODS

        private void btnAddToFavorites_Click(object sender, EventArgs e)
        {
            string cityName = selectCityTextBox.Text.Trim();

            if (string.IsNullOrWhiteSpace(cityName))
            {
                MessageBox.Show("Please enter a city name before adding to favorites.",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Check if the city is already in favorites
            if (IsCityInFavorites(cityName))
            {
                MessageBox.Show($"{cityName} is already in your favorites.",
                    "Already in Favorites", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Add city to favorites in database
            AddCityToFavorites(cityName);
        }

        private bool IsCityInFavorites(string cityName)
        {
            bool exists = false;
            string connectionString = "Data Source=MAYUR5365\\SQLEXPRESS;Initial Catalog=WeatherDB;Integrated Security=True";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT COUNT(*) FROM Favorites WHERE Username = @username AND CityName = @cityName";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@username", currentUsername);
                        cmd.Parameters.AddWithValue("@cityName", cityName);
                        int count = (int)cmd.ExecuteScalar();
                        exists = count > 0;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error checking favorites: " + ex.Message,
                        "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            return exists;
        }

        private void AddCityToFavorites(string cityName)
        {
            string connectionString = "Data Source=MAYUR5365\\SQLEXPRESS;Initial Catalog=WeatherDB;Integrated Security=True";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "INSERT INTO Favorites (Username, CityName) VALUES (@username, @cityName)";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@username", currentUsername);
                        cmd.Parameters.AddWithValue("@cityName", cityName);
                        int result = cmd.ExecuteNonQuery();

                        if (result > 0)
                        {
                            MessageBox.Show($"{cityName} added to favorites!",
                                "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("Failed to add to favorites. Please try again.",
                                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error adding to favorites: " + ex.Message,
                        "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnViewFavorites_Click(object sender, EventArgs e)
        {
            FavoritesForm favoritesForm = new FavoritesForm(currentUsername, this);
            favoritesForm.Show();
        }

        public void FetchCityWeatherFromHistory(string cityName)
        {
            selectCityTextBox.Text = cityName;
            btnConfirm.PerformClick(); // Simulates the Confirm button click
        }

        public void FetchCityWeatherFromFavorites(string cityName)
        {
            selectCityTextBox.Text = cityName;
            btnConfirm.PerformClick(); // Simulates the Confirm button click
        }
    }
}