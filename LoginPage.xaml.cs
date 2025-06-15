using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using Microsoft.Maui.Controls;

namespace AlperAcikogoz_Odev3
{
    public partial class LoginPage : ContentPage
    {
        // Firebase Web API Key'in
        private const string WebApiKey = "AIzaSyAjeNBuLPUlUethluNRatWyUEp51wRH638";

        public LoginPage()
        {
            InitializeComponent();
        }

        private async void OnLoginClicked(object sender, EventArgs e)
        {
            ErrorLabel.IsVisible = false;
            var email = EmailEntry.Text?.Trim();
            var password = PasswordEntry.Text;

            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                ErrorLabel.Text = "E-posta ve şifre boş olamaz!";
                ErrorLabel.IsVisible = true;
                return;
            }

            var client = new HttpClient();
            var request = new
            {
                email = email,
                password = password,
                returnSecureToken = true
            };

            var json = JsonSerializer.Serialize(request);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var endpoint = $"https://identitytoolkit.googleapis.com/v1/accounts:signInWithPassword?key={WebApiKey}";

            try
            {
                var response = await client.PostAsync(endpoint, content);
                if (response.IsSuccessStatusCode)
                {
                    // Giriş başarılı
                    // SHELL İLE UYUMLU YÖNLENDİRME
                    await Shell.Current.GoToAsync("//MainPage"); // veya "//Giriş" AppShell'deki route ismine göre
                }
                else
                {
                    var errorResult = await response.Content.ReadAsStringAsync();
                    ErrorLabel.Text = "Giriş başarısız! Bilgilerinizi kontrol edin.";
                    ErrorLabel.IsVisible = true;
                }
            }
            catch (Exception ex)
            {
                ErrorLabel.Text = "Bağlantı hatası: " + ex.Message;
                ErrorLabel.IsVisible = true;
            }
        }

        private async void OnRegisterTapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new RegisterPage());
        }
    }
}
