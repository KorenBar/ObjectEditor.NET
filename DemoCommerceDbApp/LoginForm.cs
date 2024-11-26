namespace DemoCommerceDbApp
{
    public partial class LoginForm : Form
    {
        /// <summary>
        /// Gets or sets the username
        /// </summary>
        public string Username
        {
            get => usernameTextBox.Text;
            set => usernameTextBox.Text = value;
        }
        /// <summary>
        /// Gets or sets the password
        /// </summary>
        public string Password
        {
            get => passwordTextBox.Text;
            set => passwordTextBox.Text = value;
        }

        public LoginForm()
        {
            InitializeComponent();
        }

        private void loginButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Username))
            {
                MessageBox.Show("Please enter a username.", "Login failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (string.IsNullOrWhiteSpace(Password))
            {
                MessageBox.Show("Please enter a password.", "Login failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
