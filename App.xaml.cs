using Microsoft.Maui.Controls;

namespace AlperAcikogoz_Odev3
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            MainPage = new AppShell();

            // Uygulama açılırken tema tercihini oku ve uygula
            var tema = Preferences.Get("TemaModu", "light");
            Application.Current.UserAppTheme = tema == "dark" ? AppTheme.Dark : AppTheme.Light;
        }
    }
}
