namespace AlperAcikogoz_Odev3
{
    public partial class GorevEklePage : ContentPage
    {
        public GorevEklePage()
        {
            InitializeComponent();
            TarihPicker.Date = DateTime.Today;
        }

        private async void OnKaydetClicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(BaslikEntry.Text))
            {
                await DisplayAlert("Uyarı", "Başlık girin!", "Tamam");
                return;
            }

            var gorev = new Gorev
            {
                Baslik = BaslikEntry.Text,
                Detay = DetayEditor.Text,
                Tarih = TarihPicker.Date,
                Saat = SaatPicker.Time,
                Yapildi = false
            };
            await Database.Init();
            await Database.AddGorev(gorev);
            await Navigation.PopAsync();
        }
    }
}
