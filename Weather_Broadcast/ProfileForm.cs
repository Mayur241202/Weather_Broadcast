using System;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace Weather_Broadcast
{
    public partial class ProfileForm : Form
    {
        private Label lblUsername, lblEmail, lblNewPassword;
        private TextBox txtUsername, txtEmail, txtNewPassword;
        private Button btnUpdatePassword, btnBack, btnLogout;

        private string currentUsername;

        public ProfileForm(string username)
        {
            InitializeComponent();
            currentUsername = username;

            this.Text = "Profile Page";
            this.Size = new Size(400, 400); // Adjust form size
            this.StartPosition = FormStartPosition.CenterScreen; // Center the form

            this.BackColor = Color.FromArgb(72, 120, 242); // Background color
            this.ForeColor = Color.White; // Default font color for all controls
            this.FormBorderStyle = FormBorderStyle.FixedDialog; // Remove maximize button

            InitializeControls();
            LoadUserData();
        }

        private void InitializeControls()
        {
            // Create a nice rounded panel as the form container
            Panel panel = new Panel();
            panel.Size = new Size(350, 300);
            panel.Location = new Point(16, 20);
            panel.BackColor = Color.LightSkyBlue;
            panel.BorderStyle = BorderStyle.FixedSingle;
            this.Controls.Add(panel);

            Font labelFont = new Font("Segoe UI", 12F, FontStyle.Bold);

            // Label - Username
            lblUsername = new Label();
            lblUsername.Text = "Username:";
            lblUsername.BackColor = System.Drawing.Color.FromArgb(72, 120, 242);
            lblUsername.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblUsername.Location = new Point(30, 30);
            lblUsername.AutoSize = true;
            lblUsername.ForeColor = Color.White;
            lblUsername.FlatStyle = FlatStyle.Standard;
            panel.Controls.Add(lblUsername);

            // TextBox - Username
            txtUsername = new TextBox();
            txtUsername.Location = new Point(150, 30);
            txtUsername.Width = 150;
            txtUsername.ReadOnly = true;
            txtUsername.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            txtUsername.BackColor = System.Drawing.Color.White;
            txtUsername.ForeColor = System.Drawing.Color.Black;
            txtUsername.BorderStyle = BorderStyle.None;
            panel.Controls.Add(txtUsername);

            // Label - Email
            lblEmail = new Label();
            lblEmail.Text = "Email:";
            lblEmail.BackColor = System.Drawing.Color.FromArgb(72, 120, 242);
            lblEmail.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblEmail.FlatStyle = FlatStyle.Standard;
            lblEmail.AutoSize = true;
            lblEmail.Location = new Point(30, 70);
            lblEmail.ForeColor = Color.White;
            panel.Controls.Add(lblEmail);

            // TextBox - Email
            txtEmail = new TextBox();
            txtEmail.Location = new Point(150, 70);
            txtEmail.Width = 150;
            txtEmail.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            txtEmail.ReadOnly = true;
            txtEmail.BackColor = System.Drawing.Color.White;
            txtEmail.ForeColor = System.Drawing.Color.Black;
            txtEmail.BorderStyle = BorderStyle.None;
            panel.Controls.Add(txtEmail);

            // Label - New Password
            lblNewPassword = new Label();
            lblNewPassword.Text = "New Password:";
            lblNewPassword.BackColor = System.Drawing.Color.FromArgb(72, 120, 242);
            lblNewPassword.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblNewPassword.FlatStyle = FlatStyle.Standard;
            lblNewPassword.AutoSize = true;
            lblNewPassword.Location = new Point(30, 110);
            lblNewPassword.ForeColor = Color.White;
            panel.Controls.Add(lblNewPassword);

            // TextBox - New Password
            txtNewPassword = new TextBox();
            txtNewPassword.Location = new Point(150, 110);
            txtNewPassword.Width = 150;
            txtNewPassword.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            txtNewPassword.PasswordChar = '*';
            txtNewPassword.BackColor = System.Drawing.Color.White;
            txtNewPassword.ForeColor = System.Drawing.Color.Black;
            txtNewPassword.BorderStyle = BorderStyle.None;
            panel.Controls.Add(txtNewPassword);

            // Button - Update Password
            btnUpdatePassword = new Button();
            btnUpdatePassword.Text = "Update Password";
            btnUpdatePassword.Location = new Point(50, 160);
            btnUpdatePassword.Width = 250;
            btnUpdatePassword.Height = 35;
            btnUpdatePassword.BackColor = Color.FromArgb(46, 204, 113);
            btnUpdatePassword.FlatStyle = FlatStyle.Standard;
            btnUpdatePassword.FlatAppearance.BorderSize = 0;
            btnUpdatePassword.Font = new Font("Arial", 10, FontStyle.Bold);
            btnUpdatePassword.Click += BtnUpdatePassword_Click;
            panel.Controls.Add(btnUpdatePassword);

            // Button - Back
            btnBack = new Button();
            btnBack.Text = "Back";
            btnBack.Location = new Point(50, 210);
            btnBack.Width = 100;
            btnBack.Height = 35;
            btnBack.BackColor = Color.FromArgb(52, 152, 219);
            btnBack.FlatStyle = FlatStyle.Standard;
            btnBack.FlatAppearance.BorderSize = 0;
            btnBack.Font = new Font("Arial", 10, FontStyle.Bold);
            btnBack.Click += btnBack_Click;
            panel.Controls.Add(btnBack);

            // Button - Logout
            btnLogout = new Button();
            btnLogout.Text = "Logout";
            btnLogout.Location = new Point(200, 210); // Positioned below Update Password button
            btnLogout.Width = 100;
            btnLogout.Height = 35;
            btnLogout.BackColor = Color.FromArgb(231, 76, 60);
            btnLogout.FlatStyle = FlatStyle.Standard;
            btnLogout.FlatAppearance.BorderSize = 0;
            btnLogout.Font = new Font("Arial", 10, FontStyle.Bold);
            btnLogout.Click += BtnLogout_Click;
            panel.Controls.Add(btnLogout);

            this.FormClosing += FormUtils.HandleFormClosing;

        }

        private void LoadUserData()
        {
            string connectionString = "Data Source=MAYUR5365\\SQLEXPRESS;Initial Catalog=WeatherDB;Integrated Security=True";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT Username, Email FROM Users WHERE Username = @username";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@username", currentUsername);
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        txtUsername.Text = reader["Username"].ToString();
                        txtEmail.Text = reader["Email"].ToString();
                    }
                }
            }
        }

        private void BtnUpdatePassword_Click(object sender, EventArgs e)
        {
            string newPassword = txtNewPassword.Text.Trim();

            if (string.IsNullOrEmpty(newPassword))
            {
                MessageBox.Show("Please enter a new password.");
                return;
            }

            if (newPassword.Length < 6 ||
                !System.Text.RegularExpressions.Regex.IsMatch(newPassword, @"[A-Za-z]") ||
                !System.Text.RegularExpressions.Regex.IsMatch(newPassword, @"\d"))
            {
                MessageBox.Show("Password must be at least 6 characters long and contain both letters and numbers.", "Weak Password", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string connectionString = "Data Source=MAYUR5365\\SQLEXPRESS;Initial Catalog=WeatherDB;Integrated Security=True";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "UPDATE Users SET Password = @password WHERE Username = @username";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@password", newPassword);
                    cmd.Parameters.AddWithValue("@username", currentUsername);

                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Password updated successfully!");
                        txtNewPassword.Clear();
                        LoginForm loginForm = new LoginForm();
                        loginForm.Show();
                        this.Hide();
                    }
                    else
                    {
                        MessageBox.Show("Password update failed.");
                    }
                }
            }
        }


        private void btnBack_Click(object sender, EventArgs e)
        {
            // Optionally show WeatherBoard again
            WeatherBoard weatherBoard = new WeatherBoard(currentUsername);
            weatherBoard.Show();

            // Close ProfileForm or just hide it based on your requirement
            this.Close();  // Hide or close the current ProfileForm
        }

        private void BtnLogout_Click(object sender, EventArgs e)
        {
            // Close the ProfileForm and redirect to LoginForm
            LoginForm loginForm = new LoginForm(); // Make sure LoginForm exists
            loginForm.Show();
            this.Close(); // Close ProfileForm after redirecting to LoginForm
        }
    }
}
