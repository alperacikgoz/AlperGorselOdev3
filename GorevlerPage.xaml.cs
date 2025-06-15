using System.Collections.ObjectModel;

namespace AlperAcikogoz_Odev3
{
    public partial class GorevlerPage : ContentPage
    {
        public ObservableCollection<Gorev> Gorevler { get; set; } = new();

        public GorevlerPage()
        {
            InitializeComponent();
            GorevlerList.ItemsSource = Gorevler;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await Database.Init();
            var list = await Database.GetGorevler();
            Gorevler.Clear();
            foreach (var g in list) Gorevler.Add(g);
        }

        private async void OnEkleClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new GorevEklePage());
        }

        private async void OnSilClicked(object sender, EventArgs e)
        {
            var btn = sender as Button;
            if (btn?.CommandParameter is int id)
            {
                var gorev = Gorevler.FirstOrDefault(x => x.Id == id);
                if (gorev != null)
                {
                    bool silinsinMi = await DisplayAlert("Onay", $"“{gorev.Baslik}” silinsin mi?", "Evet", "Vazgeç");
                    if (silinsinMi)
                    {
                        await Database.DeleteGorev(gorev);
                        Gorevler.Remove(gorev);
                    }
                }
            }
        }

        private async void OnDetayClicked(object sender, EventArgs e)
        {
            var btn = sender as Button;
            if (btn?.CommandParameter is int id)
                await Navigation.PushAsync(new GorevDetayPage(id));
        }

        private async void OnCheckChanged(object sender, CheckedChangedEventArgs e)
        {
            var cb = sender as CheckBox;
            if (cb?.BindingContext is Gorev gorev)
            {
                gorev.Yapildi = e.Value;
                await Database.UpdateGorev(gorev);
            }
        }
    }
}
