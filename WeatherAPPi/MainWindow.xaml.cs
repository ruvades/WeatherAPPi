using MySql.Data.MySqlClient;
using System;
using System.Windows;

namespace WeatherAPPi
{
    public partial class MainWindow : Window
    {
        private MySqlConnection connection;
        private string server = "localhost";
        private string database = "examenproject";
        private string uid = "root"; 
        private string password = ""; 

        public MainWindow()
        {
            InitializeComponent();
            InitializeDatabase();
        }

        private void InitializeDatabase()
        {
            string connectionString = $"SERVER={server};DATABASE={database};UID={uid};PASSWORD={password};";

            connection = new MySqlConnection(connectionString);
        }

		private void LoginButton_Click(object sender, RoutedEventArgs e)
		{
			string username = UsernameTextBox.Text;
			string password = PasswordTextBox.Password;
			if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
			{
				MessageBox.Show("Username and password cannot be empty.");
				return; 
			}

			if (AuthenticateUser(username, password))
			{
				if (username.ToLower() == "admin")
				{
					Admin adminWindow = new Admin();
					adminWindow.Show();
					Close();
				}
				else
				{
					User userWindow = new User(username);
					userWindow.Show();
					Close();
				}
			}
			else
			{
				MessageBox.Show("Invalid username or password.");
			}
		}




		private bool AuthenticateUser(string username, string password)
        {
            string query = "SELECT COUNT(*) FROM users WHERE username = @username AND password = @password";

            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@username", username);
                command.Parameters.AddWithValue("@password", password);

                connection.Open();
                int count = Convert.ToInt32(command.ExecuteScalar());
                connection.Close();

                return count > 0;
            }
        }

		private void RegisterButton_Click(object sender, RoutedEventArgs e)
		{
			string username = UsernameTextBox.Text;
			string password = PasswordTextBox.Password;

			
			if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
			{
				MessageBox.Show("Username and password cannot be empty.");
				return; 
			}

			if (IsUsernameExists(username))
			{
				MessageBox.Show("Registration failed. Username already exists.");
			}
			else
			{
				if (RegisterUser(username, password))
				{
					MessageBox.Show("Registration successful!");
				}
				else
				{
					MessageBox.Show("Registration failed. Please try again.");
				}
			}
		}


		private bool IsUsernameExists(string username)
        {
            string query = "SELECT COUNT(*) FROM users WHERE username = @username";

            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@username", username);

                connection.Open();
                int count = Convert.ToInt32(command.ExecuteScalar());
                connection.Close();

                return count > 0;
            }
        }

        private bool RegisterUser(string username, string password)
        {
            string query = "INSERT INTO users (username, password) VALUES (@username, @password)";

            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@username", username);
                command.Parameters.AddWithValue("@password", password);

                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }
    }
}
