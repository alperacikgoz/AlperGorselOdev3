using Microsoft.Maui.Controls;
using System.Collections.ObjectModel;
using System.Text.Json;
using System.Net.Http;
using System.Threading.Tasks;
using System.IO;
using System.Linq;

namespace AlperAcikogoz_Odev3
{
    public partial class HavaDurumuPage : ContentPage
    {
        public ObservableCollection<SehirModel> Sehirler { get; set; } = new();
        private const string KayitDosyasi = "sehirler.json";

        public HavaDurumuPage()
        {
            InitializeComponent();
            SehirlerListView.ItemsSource = Sehirler;
            SehirleriYukle();
        }

        private async void SehirleriYukle()
        {
            Sehirler.Clear();
            var path = Path.Combine(FileSystem.AppDataDirectory, KayitDosyasi);
            if (File.Exists(path))
            {
                var json = File.ReadAllText(path);
                var kayitliSehirler = JsonSerializer.Deserialize<List<string>>(json) ?? new();
                LoadingIndicator.IsVisible = true;
                LoadingIndicator.IsRunning = true;
                foreach (var sehir in kayitliSehirler)
                {
                    var durum = await HavaDurumuGetir(sehir);
                    Sehirler.Add(new SehirModel { Sehir = sehir, Hava = durum });
                }
                LoadingIndicator.IsVisible = false;
                LoadingIndicator.IsRunning = false;
            }
        }

        private async void OnSehirEkleClicked(object sender, EventArgs e)
        {
            var sehir = SehirEntry.Text?.Trim();
            if (string.IsNullOrWhiteSpace(sehir))
                return;

            var duzgunSehir = TurkceKarakterDuzelt(sehir);

            // Aynı şehir birden fazla eklenmesin
            if (Sehirler.Any(x => x.Sehir.Equals(duzgunSehir, System.StringComparison.OrdinalIgnoreCase)))
            {
                await DisplayAlert("Uyarı", "Bu şehir zaten listede var.", "Tamam");
                SehirEntry.Text = "";
                return;
            }

            LoadingIndicator.IsVisible = true;
            LoadingIndicator.IsRunning = true;
            var durum = await HavaDurumuGetir(duzgunSehir);
            LoadingIndicator.IsVisible = false;
            LoadingIndicator.IsRunning = false;

            if (durum.StartsWith("Hata"))
            {
                await DisplayAlert("Şehir Bulunamadı", durum, "Tamam");
                SehirEntry.Text = "";
                return;
            }

            Sehirler.Add(new SehirModel { Sehir = duzgunSehir, Hava = durum });
            SehirEntry.Text = "";
        }

        private void OnKaydetClicked(object sender, EventArgs e)
        {
            var path = Path.Combine(FileSystem.AppDataDirectory, KayitDosyasi);
            var kayitList = Sehirler.Select(x => x.Sehir).ToList();
            File.WriteAllText(path, JsonSerializer.Serialize(kayitList));
            DisplayAlert("Başarılı", "Şehirler kaydedildi!", "Tamam");
        }

        private void OnSilClicked(object sender, EventArgs e)
        {
            var btn = sender as Button;
            string sehir = btn?.CommandParameter?.ToString();
            if (!string.IsNullOrEmpty(sehir))
            {
                var item = Sehirler.FirstOrDefault(x => x.Sehir == sehir);
                if (item != null)
                    Sehirler.Remove(item);
            }
        }

        // API: wttr.in (anahtarsız), ör: https://wttr.in/Istanbul?format=%C+%t
        private async Task<string> HavaDurumuGetir(string sehir)
        {
            try
            {
                var apiSehir = System.Net.WebUtility.UrlEncode(sehir);
                var url = $"https://wttr.in/{apiSehir}?format=%C+%t";
                var client = new HttpClient();
                var result = await client.GetStringAsync(url);

                // Yanıt boşsa veya şehir yoksa
                if (string.IsNullOrWhiteSpace(result) || result.Contains("Unknown location"))
                    return "Hata: Şehir bulunamadı.";

                return result; // Ör: "Az bulutlu +21°C"
            }
            catch
            {
                return "Hata: Hava durumu alınamadı.";
            }
        }

        // Türkçe karakterleri İngilizce'ye çevir
        private string TurkceKarakterDuzelt(string input)
        {
            string[,] harfler = new string[,]
            {
                {"ç", "c"}, {"Ç", "C"},
                {"ğ", "g"}, {"Ğ", "G"},
                {"ı", "i"}, {"İ", "I"},
                {"ö", "o"}, {"Ö", "O"},
                {"ş", "s"}, {"Ş", "S"},
                {"ü", "u"}, {"Ü", "U"}
            };
            for (int i = 0; i < harfler.GetLength(0); i++)
                input = input.Replace(harfler[i, 0], harfler[i, 1]);
            return input;
        }

        public class SehirModel
        {
            public string Sehir { get; set; }
            public string Hava { get; set; }
        }
    }
}
