using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using Microsoft.Maui.Controls;

namespace AlperAcikogoz_Odev3
{
    public partial class RegisterPage : ContentPage
    {
        private const string WebApiKey = "AIzaSyAjeNBuLPUlUethluNRatWyUEp51wRH638";

        public RegisterPage()
        {
            InitializeComponent();
        }

        private async void OnRegisterClicked(object sender, EventArgs e)
        {
            ErrorLabel.IsVisible = false;
            var email = EmailEntry.Text?.Trim();
            var password = PasswordEntry.Text;

            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password) || password.Length < 6)
            {
                ErrorLabel.Text = "Geçerli bir e-posta ve en az 6 karakterli şifre giriniz!";
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
            var endpoint = $"https://identitytoolkit.googleapis.com/v1/accounts:signUp?key={WebApiKey}";

            try
            {
                var response = await client.PostAsync(endpoint, content);
                if (response.IsSuccessStatusCode)
                {
                    await DisplayAlert("Başarılı", "Kayıt başarılı! Giriş yapabilirsiniz.", "Tamam");
                    await Navigation.PopAsync();
                }
                else
                {
                    ErrorLabel.Text = "Kayıt başarısız! E-posta kullanımda olabilir.";
                    ErrorLabel.IsVisible = true;
                }
            }
            catch (Exception ex)
            {
                ErrorLabel.Text = "Bağlantı hatası: " + ex.Message;
                ErrorLabel.IsVisible = true;
            }
        }

        private async void OnLoginTapped(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}
