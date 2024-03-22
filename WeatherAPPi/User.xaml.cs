using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace WeatherAPPi
{
    public partial class User : Window
    {
        public string Username { get; set; }

        public User(string username)
        {
            InitializeComponent();
            Username = username;
            DataContext = this;
        }
        private async void GetWeatherButton_Click(object sender, RoutedEventArgs e)
        {
            string city = CityTextBox.Text;

            if (string.IsNullOrWhiteSpace(city))
            {
                WeatherInfoTextBlock.Text = "Please enter a city.";
                return;
            }

            try
            {
                string apiKey = "a5baa99fc68ca79f748895deb173f947";
                string apiUrl = $"http://api.openweathermap.org/data/2.5/weather?q={city}&appid={apiKey}&units=metric";
                string apiForecastUrl = $"http://api.openweathermap.org/data/2.5/forecast?q={city}&appid={apiKey}&units=metric";
                string unsplashApiKey = "zi_q4el-MhB5gm5_MziGtBWep6QakE9GlgTMeu1_mJM";
                string unsplashApiUrl = $"https://api.unsplash.com/photos/random?query={city}&client_id={unsplashApiKey}";

                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage response = await client.GetAsync(apiUrl);

                    if (response.IsSuccessStatusCode)
                    {
                        string responseBody = await response.Content.ReadAsStringAsync();
                        JObject weatherData = JObject.Parse(responseBody);

                        string weatherDescription = weatherData["weather"]?[0]?["description"]?.ToString();
                        string temperature = weatherData["main"]?["temp"]?.ToString();
                        string humidity = weatherData["main"]?["humidity"]?.ToString();
                        string windSpeed = weatherData["wind"]?["speed"]?.ToString();
                        HttpResponseMessage forecastResponse = await client.GetAsync(apiForecastUrl);

                        if (forecastResponse.IsSuccessStatusCode)
                        {
                            string forecastResponseBody = await forecastResponse.Content.ReadAsStringAsync();
                            JObject forecastData = JObject.Parse(forecastResponseBody);
                            string rainChanceNextHour = forecastData["list"]?[0]?["pop"]?.ToString();
                            string rainChanceNext24Hours = forecastData["list"]?[0]?["pop"]?.ToString();
                            string feelsLikeTemp = weatherData["main"]?["feels_like"]?.ToString();
                            var sunriseUnix = weatherData["sys"]?["sunrise"]?.Value<long>();
                            var sunsetUnix = weatherData["sys"]?["sunset"]?.Value<long>();
                            var sunriseTime = DateTimeOffset.FromUnixTimeSeconds(sunriseUnix ?? 0).LocalDateTime.ToString("HH:mm");
                            var sunsetTime = DateTimeOffset.FromUnixTimeSeconds(sunsetUnix ?? 0).LocalDateTime.ToString("HH:mm");
                            HttpResponseMessage imageResponse = await client.GetAsync(unsplashApiUrl);

                            if (imageResponse.IsSuccessStatusCode)
                            {
                                string imageResponseBody = await imageResponse.Content.ReadAsStringAsync();
                                JObject imageData = JObject.Parse(imageResponseBody);

                                string imageUrl = imageData["urls"]?["regular"]?.ToString();

                                WeatherInfoTextBlock.Text = $"Weather for {city}:\n\n" +
                                    $"Description: {weatherDescription}\n" +
                                    $"Temperature: {temperature} °C\n" +
                                    $"Feels Like: {feelsLikeTemp} °C\n" +
                                    $"Humidity: {humidity}%\n" +
                                    $"Wind Speed: {windSpeed} m/s\n" +
                                    $"Chance of Rain (Next Hour): {rainChanceNextHour}%\n" +
                                    $"Chance of Rain (Next 24 Hours): {rainChanceNext24Hours}%\n" +
                                    $"Sunrise: {sunriseTime}\n" +
                                    $"Sunset: {sunsetTime}";

                                if (!string.IsNullOrEmpty(imageUrl))
                                {
                                    WeatherImage.Source = new BitmapImage(new Uri(imageUrl));
                                }
                            }
                            else
                            {
                                WeatherInfoTextBlock.Text = $"Weather for {city}:\n\n" +
                                    $"Description: {weatherDescription}\n" +
                                    $"Temperature: {temperature} °C\n" +
                                    $"Feels Like: {feelsLikeTemp} °C\n" +
                                    $"Humidity: {humidity}%\n" +
                                    $"Wind Speed: {windSpeed} m/s\n" +
                                    $"Chance of Rain (Next Hour): {rainChanceNextHour}%\n" +
                                    $"Chance of Rain (Next 24 Hours): {rainChanceNext24Hours}%\n" +
                                    $"Sunrise: {sunriseTime}\n" +
                                    $"Sunset: {sunsetTime}";

                                WeatherImage.Source = new BitmapImage(new Uri("https://source.unsplash.com/featured/?mountain"));
                            }
                        }
                        else
                        {
                            WeatherInfoTextBlock.Text = "Failed to fetch weather forecast data.";
                        }
                    }
                    else
                    {
                        WeatherInfoTextBlock.Text = "Failed to fetch current weather data.";
                    }
                }
            }
            catch (Exception ex)
            {
                WeatherInfoTextBlock.Text = $"Error occurred: {ex.Message}";
            }
        }

    }
}
