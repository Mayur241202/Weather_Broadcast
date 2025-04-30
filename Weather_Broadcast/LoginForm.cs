using System;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace Weather_Broadcast
{
    public partial class LoginForm : Form
    {
        // Declare controls
        private TextBox txtUsername;
        private TextBox txtPassword;
        private Button btnLogin;
        private Button btnRegister;
        private Label lblUsername;
        private Label lblPassword;

        public LoginForm()
        {
            InitializeComponent();
            InitializeControls();
        }

        private void InitializeControls()
        {
            // Set Form Properties for Background Color
            this.BackColor = System.Drawing.Color.FromArgb(135, 206, 235);
            this.ClientSize = new System.Drawing.Size(400, 250); // Adjust Form size
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Login - Weather Broadcast";

            // Set Font Style for Form Labels and Buttons
            Font labelFont = new Font("Segoe UI", 10F, FontStyle.Bold);
            Font buttonFont = new Font("Segoe UI", 10F, FontStyle.Bold);

            // Username Label
            lblUsername = new Label();
            lblUsername.Text = "Username:";
            lblUsername.BackColor = System.Drawing.Color.FromArgb(72, 120, 242);
            lblUsername.Location = new System.Drawing.Point(35, 58);
            lblUsername.AutoSize = true;
            lblUsername.Font = labelFont;
            lblUsername.ForeColor = System.Drawing.Color.White;
            lblUsername.FlatStyle = FlatStyle.Standard;
            this.Controls.Add(lblUsername);

            // Username TextBox
            txtUsername = new TextBox();
            txtUsername.Location = new System.Drawing.Point(135, 55);
            txtUsername.Size = new System.Drawing.Size(220, 30);
            txtUsername.Font = new Font("Segoe UI", 10F);
            txtUsername.BackColor = System.Drawing.Color.White;
            txtUsername.ForeColor = System.Drawing.Color.Black;
            this.Controls.Add(txtUsername);

            // Password Label
            lblPassword = new Label();
            lblPassword.Text = "Password:";
            lblPassword.BackColor = System.Drawing.Color.FromArgb(72, 120, 242);
            lblPassword.Location = new System.Drawing.Point(35, 108);
            lblPassword.AutoSize = true;
            lblPassword.Font = labelFont;
            lblPassword.ForeColor = System.Drawing.Color.White;
            this.Controls.Add(lblPassword);

            // Password TextBox
            txtPassword = new TextBox();
            txtPassword.Location = new System.Drawing.Point(135, 105);
            txtPassword.Size = new System.Drawing.Size(220, 30);
            txtPassword.Font = new Font("Segoe UI", 10F);
            txtPassword.PasswordChar = '*'; // Mask password
            txtPassword.BackColor = System.Drawing.Color.White;
            txtPassword.ForeColor = System.Drawing.Color.Black;
            this.Controls.Add(txtPassword);

            // Login Button
            btnLogin = new Button();
            btnLogin.Text = "Login";
            btnLogin.Location = new System.Drawing.Point(80, 160);
            btnLogin.Size = new System.Drawing.Size(100, 40);
            btnLogin.Font = buttonFont;
            btnLogin.BackColor = System.Drawing.Color.FromArgb(72, 120, 242); // Blue color
            btnLogin.ForeColor = System.Drawing.Color.White;
            btnLogin.FlatStyle = FlatStyle.Standard;
            btnLogin.FlatAppearance.BorderSize = 0;
            btnLogin.Click += BtnLogin_Click;
            this.Controls.Add(btnLogin);

            // Register Button
            btnRegister = new Button();
            btnRegister.Text = "Register";
            btnRegister.Location = new System.Drawing.Point(200, 160);
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
            panelLabel.Text = "Login - Weather Forecast";
            panelLabel.ForeColor = Color.White;
            panelLabel.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            panelLabel.AutoSize = true;
            panelLabel.Location = new Point(10, 5); // Adjust position inside panel

            // Add the label to the panel
            panel.Controls.Add(panelLabel);

            this.FormClosing += FormUtils.HandleFormClosing;

        }

        private void BtnLogin_Click(object sender, EventArgs e)
        {
            string usernameInput = txtUsername.Text.Trim();
            string passwordInput = txtPassword.Text;

            // --- Input Validations ---
            if (string.IsNullOrWhiteSpace(usernameInput) || string.IsNullOrWhiteSpace(passwordInput))
            {
                MessageBox.Show("Please enter both username and password.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!System.Text.RegularExpressions.Regex.IsMatch(usernameInput, @"^[a-zA-Z0-9]{3,20}$"))
            {
                MessageBox.Show("Username must be 3–20 characters long and contain only letters and numbers.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (passwordInput.Length < 6)
            {
                MessageBox.Show("Password must be at least 6 characters long.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // --- DB Authentication Logic ---
            string connectionString = "Data Source=MAYUR5365\\SQLEXPRESS;Initial Catalog=WeatherDB;Integrated Security=True";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT Username, City FROM Users WHERE Username = @username AND Password = @password";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@username", usernameInput);
                        cmd.Parameters.AddWithValue("@password", passwordInput);

                        SqlDataReader reader = cmd.ExecuteReader();

                        if (reader.Read())
                        {
                            string fetchedUsername = reader["Username"].ToString();
                            UserSession.Username = fetchedUsername;

                            if (!reader.IsDBNull(reader.GetOrdinal("City")))
                            {
                                string userCity = reader["City"].ToString();
                                UserSession.DefaultCity = userCity;
                            }

                            MessageBox.Show("Login Successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            WeatherBoard weatherForm = new WeatherBoard(fetchedUsername);
                            weatherForm.Show();
                            this.Hide();
                        }
                        else
                        {
                            MessageBox.Show("Invalid username or password.", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void BtnRegister_Click(object sender, EventArgs e)
        {
            // Open Registration Form
            RegistrationForm regForm = new RegistrationForm();
            regForm.Show();
            this.Hide();
        }
    }
}