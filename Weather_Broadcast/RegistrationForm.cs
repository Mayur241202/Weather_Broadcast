using System;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Data;
using System.Drawing;

namespace Weather_Broadcast
{
    public partial class RegistrationForm : Form
    {
        private TextBox txtUsername;
        private TextBox txtEmail;
        private TextBox txtPassword;
        private TextBox txtCity; // Added city textbox
        private Button btnRegister, btnLogin;
        private Label lblUsername;
        private Label lblEmail;
        private Label lblPassword;
        private Label lblCity; // Added city label
        private readonly Color primaryColor = Color.FromArgb(56, 132, 255);
        private readonly Color accentColor = Color.FromArgb(94, 169, 255);
        private readonly Color backgroundColor = Color.FromArgb(40, 44, 52);
        private readonly Color textColor = Color.FromArgb(240, 240, 240);
        private readonly Color placeholderColor = Color.FromArgb(150, 150, 150);
        private readonly int formPadding = 30;

        public RegistrationForm()
        {
            InitializeComponent();
            InitializeControls();
        }

        private void InitializeControls()
        {
            // Set Form Properties
            this.BackColor = System.Drawing.Color.FromArgb(135, 206, 235); // Dark Background
            this.ClientSize = new System.Drawing.Size(400, 300); // Increased size to accommodate new field
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Registration - Weather Broadcast";

            // Set Font Style
            Font labelFont = new Font("Segoe UI", 10F, FontStyle.Bold);
            Font buttonFont = new Font("Segoe UI", 10F, FontStyle.Bold);

            // Username Label
            lblUsername = new Label();
            lblUsername.Text = "Username:";
            lblUsername.BackColor = System.Drawing.Color.FromArgb(72, 120, 242);
            lblUsername.Location = new System.Drawing.Point(35, 58);
            lblUsername.AutoSize = true;
            lblUsername.ForeColor = System.Drawing.Color.White;
            lblUsername.Font = labelFont;
            lblUsername.FlatStyle = FlatStyle.Standard;
            this.Controls.Add(lblUsername);

            // Username TextBox
            txtUsername = new TextBox();
            txtUsername.Location = new System.Drawing.Point(135, 55);
            txtUsername.Size = new System.Drawing.Size(220, 30);
            txtUsername.Font = new Font("Segoe UI", 10F);
            this.Controls.Add(txtUsername);

            // Email Label
            lblEmail = new Label();
            lblEmail.Text = "Email:";
            lblEmail.BackColor = System.Drawing.Color.FromArgb(72, 120, 242);
            lblEmail.Location = new System.Drawing.Point(35, 98);
            lblEmail.AutoSize = true;
            lblEmail.ForeColor = System.Drawing.Color.White;
            lblEmail.Font = labelFont;
            lblEmail.FlatStyle = FlatStyle.Standard;
            this.Controls.Add(lblEmail);

            // Email TextBox
            txtEmail = new TextBox();
            txtEmail.Location = new System.Drawing.Point(135, 95);
            txtEmail.Size = new System.Drawing.Size(220, 30);
            txtEmail.Font = new Font("Segoe UI", 10F);
            this.Controls.Add(txtEmail);

            // Password Label
            lblPassword = new Label();
            lblPassword.Text = "Password:";
            lblPassword.BackColor = System.Drawing.Color.FromArgb(72, 120, 242);
            lblPassword.Location = new System.Drawing.Point(35, 138);
            lblPassword.AutoSize = true;
            lblPassword.ForeColor = System.Drawing.Color.White;
            lblPassword.Font = labelFont;
            lblPassword.FlatStyle = FlatStyle.Standard;
            this.Controls.Add(lblPassword);

            // Password TextBox
            txtPassword = new TextBox();
            txtPassword.Location = new System.Drawing.Point(135, 135);
            txtPassword.Size = new System.Drawing.Size(220, 30);
            txtPassword.Font = new Font("Segoe UI", 10F);
            txtPassword.PasswordChar = '*';
            this.Controls.Add(txtPassword);

            // City Label - NEW
            lblCity = new Label();
            lblCity.Text = "City:";
            lblCity.BackColor = System.Drawing.Color.FromArgb(72, 120, 242);
            lblCity.Location = new System.Drawing.Point(35, 178);
            lblCity.AutoSize = true;
            lblCity.ForeColor = System.Drawing.Color.White;
            lblCity.Font = labelFont;
            lblCity.FlatStyle = FlatStyle.Standard;
            this.Controls.Add(lblCity);

            // City TextBox - NEW
            txtCity = new TextBox();
            txtCity.Location = new System.Drawing.Point(135, 175);
            txtCity.Size = new System.Drawing.Size(220, 30);
            txtCity.Font = new Font("Segoe UI", 10F);
            this.Controls.Add(txtCity);

            // Login Button - Moved down
            btnLogin = new Button();
            btnLogin.Text = "Login";
            btnLogin.Location = new System.Drawing.Point(95, 228); // Updated position
            btnLogin.Size = new System.Drawing.Size(100, 40);
            btnLogin.Font = buttonFont;
            btnLogin.BackColor = System.Drawing.Color.FromArgb(72, 120, 242); // Blue color
            btnLogin.ForeColor = System.Drawing.Color.White;
            btnLogin.FlatStyle = FlatStyle.Standard;
            btnLogin.FlatAppearance.BorderSize = 0;
            btnLogin.Click += BtnLogin_Click;
            this.Controls.Add(btnLogin);

            // Register Button - Moved down
            btnRegister = new Button();
            btnRegister.Text = "Register";
            btnRegister.Location = new System.Drawing.Point(215, 228); // Updated position
            btnRegister.Size = new System.Drawing.Size(100, 40);
            btnRegister.Font = buttonFont;
            btnRegister.BackColor = System.Drawing.Color.FromArgb(255, 103, 58); // Orange color
            btnRegister.ForeColor = System.Drawing.Color.White;
            btnRegister.FlatStyle = FlatStyle.Standard;
            btnRegister.FlatAppearance.BorderSize = 0;
            btnRegister.Click += BtnRegister_Click;
            this.Controls.Add(btnRegister);

            // Create the panel
            Panel panel = new Panel();
            panel.BackColor = System.Drawing.Color.FromArgb(72, 120, 242);
            panel.Size = new System.Drawing.Size(this.Width, 30);
            panel.Location = new System.Drawing.Point(0, 0);
            panel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            this.Controls.Add(panel);

            // Create the label
            Label panelLabel = new Label();
            panelLabel.Text = "Register - Weather Forecast";
            panelLabel.ForeColor = Color.White;
            panelLabel.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            panelLabel.AutoSize = true;
            panelLabel.Location = new Point(10, 5); // Adjust position inside panel

            // Add the label to the panel
            panel.Controls.Add(panelLabel);
            this.FormClosing += FormUtils.HandleFormClosing;
        }

        private void BtnRegister_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string email = txtEmail.Text.Trim();
            string password = txtPassword.Text;
            string city = txtCity.Text.Trim();

            // Basic field check
            if (string.IsNullOrWhiteSpace(username) ||
                string.IsNullOrWhiteSpace(email) ||
                string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Please fill all required fields!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Username validation (alphanumeric, 3-20 chars)
            if (!System.Text.RegularExpressions.Regex.IsMatch(username, @"^[a-zA-Z0-9]{3,20}$"))
            {
                MessageBox.Show("Username must be 3–20 characters long and contain only letters and numbers.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Email validation
            if (!System.Text.RegularExpressions.Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                MessageBox.Show("Please enter a valid email address.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Password validation
            if (password.Length < 6)
            {
                MessageBox.Show("Password must be at least 6 characters long.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Optional city validation (only letters and spaces)
            if (!string.IsNullOrWhiteSpace(city) && !System.Text.RegularExpressions.Regex.IsMatch(city, @"^[a-zA-Z\s]+$"))
            {
                MessageBox.Show("City name must only contain letters and spaces.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Proceed with DB logic...
            string connectionString = "Data Source=MAYUR5365\\SQLEXPRESS;Initial Catalog=WeatherDB;Integrated Security=True";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    string checkQuery = "SELECT COUNT(*) FROM Users WHERE Username = @Username";
                    using (SqlCommand checkCommand = new SqlCommand(checkQuery, connection))
                    {
                        checkCommand.Parameters.Add("@Username", SqlDbType.VarChar).Value = username;
                        int userCount = (int)checkCommand.ExecuteScalar();

                        if (userCount > 0)
                        {
                            MessageBox.Show("Username already exists. Please choose a different one.",
                                "Registration Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }

                    string query = "INSERT INTO Users (Username, Email, Password, City) VALUES (@Username, @Email, @Password, @City)";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.Add("@Username", SqlDbType.VarChar).Value = username;
                        command.Parameters.Add("@Email", SqlDbType.VarChar).Value = email;
                        command.Parameters.Add("@Password", SqlDbType.VarChar).Value = password;
                        command.Parameters.Add("@City", SqlDbType.VarChar).Value =
                            string.IsNullOrWhiteSpace(city) ? DBNull.Value : (object)city;

                        int result = command.ExecuteNonQuery();

                        if (result > 0)
                        {
                            MessageBox.Show("Registration successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            new LoginForm().Show();
                            this.Hide();
                        }
                        else
                        {
                            MessageBox.Show("Registration failed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // Login button click handler
        private void BtnLogin_Click(object sender, EventArgs e)
        {
            // Open Login Form
            LoginForm loginForm = new LoginForm();
            loginForm.Show();
            this.Hide(); // Hide the current form
        }
    }
}