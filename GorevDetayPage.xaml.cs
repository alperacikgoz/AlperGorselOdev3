namespace AlperAcikogoz_Odev3
{
    public partial class GorevDetayPage : ContentPage
    {
        public GorevDetayPage(int gorevId)
        {
            InitializeComponent();
            Yukle(gorevId);
        }

        private async void Yukle(int id)
        {
            await Database.Init();
            var gorev = await Database.GetGorevById(id);
            if (gorev != null)
            {
                BaslikLabel.Text = gorev.Baslik;
                DetayLabel.Text = gorev.Detay;
                TarihLabel.Text = "Tarih: " + gorev.Tarih.ToString("dd.MM.yyyy");
                SaatLabel.Text = "Saat: " + gorev.Saat.ToString(@"hh\:mm");
                DurumLabel.Text = gorev.Yapildi ? "Durum: TAMAMLANDI" : "Durum: BEKLEMEDE";
            }
        }
    }
}
