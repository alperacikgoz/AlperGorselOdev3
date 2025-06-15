using Microsoft.Maui.Controls;

namespace AlperAcikogoz_Odev3
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
        }

        // Giriş Yap menüsü tıklandığında LoginPage'e gider
        private async void OnLoginMenuClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(LoginPage));
        }
    }
}
