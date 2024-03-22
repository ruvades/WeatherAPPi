using MySql.Data.MySqlClient;
using System;
using System.Windows;

namespace WeatherAPPi
{
    public partial class Admin : Window
    {
        private MySqlConnection connection;
        private string server = "localhost";
        private string database = "examenproject";
        private string uid = "root";
        private string password = ""; 

        public Admin()
        {
            InitializeComponent();
            InitializeDatabase();
        }

        private void InitializeDatabase()
        {
            string connectionString = $"SERVER={server};DATABASE={database};UID={uid};PASSWORD={password};";

            connection = new MySqlConnection(connectionString);
        }

        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ResetAccounts();
                MessageBox.Show("All accounts have been reset successfully.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error occurred: {ex.Message}");
            }
        }

        private void ResetAccounts()
        {
            string query = "TRUNCATE TABLE users; ALTER TABLE users AUTO_INCREMENT = 1;";

            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }
            AddAdminAccount();
        }

        private void AddAdminAccount()
        {
            string username = "Admin";
            string password = "Admin";

            string query = "INSERT INTO users (username, password) VALUES (@username, @password)";

            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@username", username);
                command.Parameters.AddWithValue("@password", password);

                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }
        }

    }
}
