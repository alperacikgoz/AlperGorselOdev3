using Microsoft.Maui.Controls;
using Microsoft.Maui.ApplicationModel;

namespace AlperAcikogoz_Odev3
{
    public partial class HaberDetayPage : ContentPage
    {
        private Haber _haber;

        public HaberDetayPage(Haber haber)
        {
            InitializeComponent();
            _haber = haber;
            BaslikLabel.Text = haber.Baslik;
            OzetLabel.Text = haber.Ozet;
            TarihLabel.Text = haber.Tarih;
            KaynakLabel.Text = haber.Kaynak ?? "";
        }

        private async void OnPaylasClicked(object sender, EventArgs e)
        {
            await Share.Default.RequestAsync(new ShareTextRequest
            {
                Text = $"{_haber.Baslik}\n{_haber.Link}",
                Title = "Haberi Paylaş"
            });
        }

        private async void OnTarayiciClicked(object sender, EventArgs e)
        {
            try { await Launcher.Default.OpenAsync(_haber.Link); }
            catch { await DisplayAlert("Hata", "Tarayıcı açılamadı.", "Tamam"); }
        }

        private async void OnBackClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}
