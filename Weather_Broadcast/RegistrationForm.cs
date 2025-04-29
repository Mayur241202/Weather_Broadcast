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
        private Button btnRegister, btnLogin;
        private Label lblUsername;
        private Label lblEmail;
        private Label lblPassword;

        public RegistrationForm()
        {
            InitializeComponent();
            InitializeControls();
        }

        private void InitializeControls()
        {
            // Set Form Properties
            this.BackColor = System.Drawing.Color.FromArgb(135, 206, 235); // Dark Background
            this.ClientSize = new System.Drawing.Size(400, 250); // Adjusted size
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Registration - Weather Broadcast";

            // Set Font Style
            Font labelFont = new Font("Segoe UI", 12F, FontStyle.Regular);
            Font buttonFont = new Font("Segoe UI", 10F, FontStyle.Bold);

            // Username Label
            lblUsername = new Label();
            lblUsername.Text = "Username:";
            lblUsername.Location = new System.Drawing.Point(20, 40);
            lblUsername.Size = new System.Drawing.Size(100, 20);
            lblUsername.ForeColor = System.Drawing.Color.White;
            lblUsername.Font = labelFont;
            this.Controls.Add(lblUsername);

            // Username TextBox
            txtUsername = new TextBox();
            txtUsername.Location = new System.Drawing.Point(120, 40);
            txtUsername.Size = new System.Drawing.Size(220, 30);
            txtUsername.Font = new Font("Segoe UI", 10F);
            this.Controls.Add(txtUsername);

            // Email Label
            lblEmail = new Label();
            lblEmail.Text = "Email:";
            lblEmail.Location = new System.Drawing.Point(20, 80);
            lblEmail.Size = new System.Drawing.Size(100, 20);
            lblEmail.ForeColor = System.Drawing.Color.White;
            lblEmail.Font = labelFont;
            this.Controls.Add(lblEmail);

            // Email TextBox
            txtEmail = new TextBox();
            txtEmail.Location = new System.Drawing.Point(120, 80);
            txtEmail.Size = new System.Drawing.Size(220, 30);
            txtEmail.Font = new Font("Segoe UI", 10F);
            this.Controls.Add(txtEmail);

            // Password Label
            lblPassword = new Label();
            lblPassword.Text = "Password:";
            lblPassword.Location = new System.Drawing.Point(20, 120);
            lblPassword.Size = new System.Drawing.Size(100, 20);
            lblPassword.ForeColor = System.Drawing.Color.White;
            lblPassword.Font = labelFont;
            this.Controls.Add(lblPassword);

            // Password TextBox
            txtPassword = new TextBox();
            txtPassword.Location = new System.Drawing.Point(120, 120);
            txtPassword.Size = new System.Drawing.Size(220, 30);
            txtPassword.Font = new Font("Segoe UI", 10F);
            txtPassword.PasswordChar = '*';
            this.Controls.Add(txtPassword);

            // Login Button
            btnLogin = new Button();
            btnLogin.Text = "Login";
            btnLogin.Location = new System.Drawing.Point(80, 160);
            btnLogin.Size = new System.Drawing.Size(100, 40);
            btnLogin.Font = buttonFont;
            btnLogin.BackColor = System.Drawing.Color.FromArgb(72, 120, 242); // Blue color
            btnLogin.ForeColor = System.Drawing.Color.White;
            btnLogin.FlatStyle = FlatStyle.Flat;
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
            btnRegister.FlatStyle = FlatStyle.Flat;
            btnRegister.FlatAppearance.BorderSize = 0;
            btnRegister.Click += BtnRegister_Click;
            this.Controls.Add(btnRegister);

            // Add a stylish panel for a better background effect
            Panel panel = new Panel();
            panel.BackColor = System.Drawing.Color.FromArgb(72, 120, 242);
            panel.Size = new System.Drawing.Size(this.Width, 30);
            panel.Location = new System.Drawing.Point(0, 0);
            this.Controls.Add(panel);
        }

        private void BtnRegister_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUsername.Text) ||
                string.IsNullOrWhiteSpace(txtEmail.Text) ||
                string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MessageBox.Show("Please fill all fields!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string connectionString = "Data Source=MAYUR5365\\SQLEXPRESS;Initial Catalog=WeatherDB;Integrated Security=True";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "INSERT INTO Users (Username, Email, Password) VALUES (@Username, @Email, @Password)";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.Add("@Username", SqlDbType.VarChar).Value = txtUsername.Text;
                        command.Parameters.Add("@Email", SqlDbType.VarChar).Value = txtEmail.Text;
                        command.Parameters.Add("@Password", SqlDbType.VarChar).Value = txtPassword.Text;

                        int result = command.ExecuteNonQuery();

                        if (result > 0)
                        {
                            MessageBox.Show("Registration successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoginForm loginForm = new LoginForm();
                            loginForm.Show();
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
