using Microsoft.Maui.Controls;

namespace AlperAcikogoz_Odev3
{
    public partial class AyarlarPage : ContentPage
    {
        public AyarlarPage()
        {
            InitializeComponent();
            // Sayfa açıldığında aktif tema neyse, switch'i ona göre ayarla
            KoyuModSwitch.IsToggled = Application.Current.UserAppTheme == AppTheme.Dark;
        }

        private void OnKoyuModToggled(object sender, ToggledEventArgs e)
        {
            Application.Current.UserAppTheme = e.Value ? AppTheme.Dark : AppTheme.Light;
            // İsteğe bağlı: Tercihi kalıcı olarak sakla
            Preferences.Set("TemaModu", e.Value ? "dark" : "light");
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            // Eğer kullanıcı tercih etmişse ona göre switchi ayarla
            var tema = Preferences.Get("TemaModu", "light");
            KoyuModSwitch.IsToggled = tema == "dark";
        }
    }
}
