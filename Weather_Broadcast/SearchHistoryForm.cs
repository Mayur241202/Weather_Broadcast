using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace Weather_Broadcast
{
    public partial class SearchHistoryForm : Form
    {
        private string currentUsername;
        private ListView historyListView;
        private Button btnBack, btnDeleteHistory;

        public SearchHistoryForm(string username)
        {
            InitializeComponent();
            currentUsername = username;

            this.Text = "Search History";
            this.Size = new Size(500, 500);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(72, 120, 242);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;

            InitializeControls();
            LoadSearchHistory();
        }

        private void InitializeControls()
        {
            // Main Panel
            Panel panel = new Panel();
            panel.Size = new Size(440, 380);
            panel.Location = new Point(30, 30);
            panel.BackColor = Color.FromArgb(135, 206, 235);
            panel.BorderStyle = BorderStyle.FixedSingle;
            this.Controls.Add(panel);

   
            // History Heading Label
            Label lblHistoryHeading = new Label();
            lblHistoryHeading.Text = "Search History";
            lblHistoryHeading.Font = new Font("Arial", 20, FontStyle.Bold);
            lblHistoryHeading.ForeColor = Color.White;
            lblHistoryHeading.BackColor = Color.Transparent;
            lblHistoryHeading.AutoSize = true;
            lblHistoryHeading.TextAlign = ContentAlignment.MiddleCenter;
            lblHistoryHeading.Location = new Point((panel.Width - lblHistoryHeading.PreferredWidth) / 2, 5);

            panel.Controls.Add(lblHistoryHeading);

            // History ListView
            historyListView = new ListView();
            historyListView.Size = new Size(420, 280);
            historyListView.Location = new Point(10, 40);
            historyListView.View = View.Details;
            historyListView.FullRowSelect = true;
            historyListView.Columns.Add("City Name", 200);
            historyListView.Columns.Add("Search Time", 200);
            historyListView.Font = new Font("Arial", 10, FontStyle.Regular);
            historyListView.MouseDoubleClick += HistoryListView_MouseDoubleClick;
            panel.Controls.Add(historyListView);

            // Back Button
            btnBack = new Button();
            btnBack.Text = "Back";
            btnBack.Size = new Size(100, 35);
            btnBack.Location = new Point(100, 330); // shifted slightly left
            btnBack.BackColor = Color.FromArgb(52, 152, 219);
            btnBack.FlatStyle = FlatStyle.Standard;
            btnBack.FlatAppearance.BorderSize = 0;
            btnBack.Font = new Font("Arial", 10, FontStyle.Bold);
            btnBack.Click += BtnBack_Click;
            panel.Controls.Add(btnBack);

            // Delete History Button
            btnDeleteHistory = new Button();
            btnDeleteHistory.Text = "Delete History";
            btnDeleteHistory.Size = new Size(140, 35);
            btnDeleteHistory.Location = new Point(220, 330); // placed right next to back button
            btnDeleteHistory.BackColor = Color.FromArgb(231, 76, 60);
            btnDeleteHistory.FlatStyle = FlatStyle.Standard;
            btnDeleteHistory.FlatAppearance.BorderSize = 0;
            btnDeleteHistory.Font = new Font("Arial", 10, FontStyle.Bold);
            btnDeleteHistory.Click += BtnDeleteHistory_Click;
            panel.Controls.Add(btnDeleteHistory);
            this.FormClosing += FormUtils.HandleFormClosing;

        }

        private void LoadSearchHistory()
        {
            string connectionString = "Data Source=MAYUR5365\\SQLEXPRESS;Initial Catalog=WeatherDB;Integrated Security=True";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT CityName, Timestamp FROM SearchHistory WHERE Username = @username ORDER BY Timestamp DESC";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@username", currentUsername);
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (!reader.HasRows) // Check if no rows were returned
                    {
                        // If no history is found, show a message
                        ListViewItem item = new ListViewItem("History Not Found");
                        item.SubItems.Add(""); // Empty sub-item for time
                        historyListView.Items.Add(item);
                    }
                    else
                    {
                        // Load the history into the ListView
                        while (reader.Read())
                        {
                            string city = reader["CityName"].ToString();
                            string time = Convert.ToDateTime(reader["Timestamp"]).ToString("g");

                            ListViewItem item = new ListViewItem(city);
                            item.SubItems.Add(time);
                            historyListView.Items.Add(item);
                        }
                    }
                }
            }
        }

        private void HistoryListView_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (historyListView.SelectedItems.Count > 0)
            {
                string selectedCity = historyListView.SelectedItems[0].Text;

                WeatherBoard board = new WeatherBoard(currentUsername);
                board.Show();
                board.FetchCityWeatherFromHistory(selectedCity); // Optional: create this method in WeatherBoard
                this.Hide();
            }
        }

        private void BtnBack_Click(object sender, EventArgs e)
        {
            WeatherBoard board = new WeatherBoard(currentUsername);
            board.Show();
            this.Close();
        }

        private void BtnDeleteHistory_Click(object sender, EventArgs e)
        {
            // Confirm before deleting
            DialogResult result = MessageBox.Show("Are you sure you want to delete your entire search history?", "Confirm Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                string connectionString = "Data Source=MAYUR5365\\SQLEXPRESS;Initial Catalog=WeatherDB;Integrated Security=True";

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "DELETE FROM SearchHistory WHERE Username = @username";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@username", currentUsername);
                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Search history deleted successfully.");
                            historyListView.Items.Clear(); // Clear the ListView to show an empty history
                        }
                        else
                        {
                            MessageBox.Show("Failed to delete search history.");
                        }
                    }
                }
            }
        }
    }
}