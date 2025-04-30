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
    public partial class FavoritesForm : Form
    {
        private string username;
        private WeatherBoard parentForm;

        //form movement variables
        private bool isDragging = false;
        private Point newPoint = new Point(0, 0);

        public FavoritesForm()
        {
            InitializeComponent();
        }

        public FavoritesForm(string username, WeatherBoard parentForm)
        {
            InitializeComponent();
            this.username = username;
            this.parentForm = parentForm;
            this.Load += FavoritesForm_Load;
        }

        private void FavoritesForm_Load(object sender, EventArgs e)
        {
            // Set the username label
            lblUsername.Text = username;

            // Load favorite cities
            LoadFavoriteCities();
        }

        private void LoadFavoriteCities()
        {
            // Clear existing items
            listBoxFavorites.Items.Clear();

            string connectionString = "Data Source=MAYUR5365\\SQLEXPRESS;Initial Catalog=WeatherDB;Integrated Security=True";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT CityName FROM Favorites WHERE Username = @username ORDER BY CityName";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@username", username);
                        SqlDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            string cityName = reader["CityName"].ToString();
                            listBoxFavorites.Items.Add(cityName);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading favorites: " + ex.Message, "Database Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            // Update the count label
            lblFavoritesCount.Text = $"Total Favorites: {listBoxFavorites.Items.Count}";
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (listBoxFavorites.SelectedItem == null)
            {
                MessageBox.Show("Please select a city to remove.", "No Selection",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string cityToRemove = listBoxFavorites.SelectedItem.ToString();

            // Confirm deletion
            DialogResult result = MessageBox.Show($"Remove {cityToRemove} from favorites?",
                "Confirm Removal", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                RemoveFavorite(cityToRemove);
                LoadFavoriteCities(); // Refresh the list
            }
        }

        private void RemoveFavorite(string cityName)
        {
            string connectionString = "Data Source=MAYUR5365\\SQLEXPRESS;Initial Catalog=WeatherDB;Integrated Security=True";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "DELETE FROM Favorites WHERE Username = @username AND CityName = @cityName";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@username", username);
                        cmd.Parameters.AddWithValue("@cityName", cityName);
                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show($"{cityName} removed from favorites.", "Success",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error removing from favorites: " + ex.Message, "Database Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnView_Click(object sender, EventArgs e)
        {
            if (listBoxFavorites.SelectedItem == null)
            {
                MessageBox.Show("Please select a city to view.", "No Selection",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string selectedCity = listBoxFavorites.SelectedItem.ToString();

            // Pass the selected city back to WeatherBoard
            parentForm.FetchCityWeatherFromFavorites(selectedCity);
            this.Close();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // Form movement handlers
        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            isDragging = true;
            newPoint = new Point(e.X, e.Y);
        }

        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDragging)
            {
                Point p = PointToScreen(e.Location);
                Location = new Point(p.X - newPoint.X, p.Y - newPoint.Y);
            }
        }

        private void panel1_MouseUp(object sender, MouseEventArgs e)
        {
            isDragging = false;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnMinimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        // Double click on a city to view it
        private void listBoxFavorites_DoubleClick(object sender, EventArgs e)
        {
            if (listBoxFavorites.SelectedItem != null)
            {
                btnView_Click(sender, e);
            }
        }
    }
}