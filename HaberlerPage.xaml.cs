using Microsoft.Maui.Controls;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Linq;
using System.Collections.Generic;

namespace AlperAcikogoz_Odev3
{
    public partial class HaberlerPage : ContentPage
    {
        public ObservableCollection<HaberKategoriGroup> HaberGruplari { get; set; } = new();

        public HaberlerPage()
        {
            InitializeComponent();
            HaberlerCollection.ItemsSource = HaberGruplari;
            HaberleriYukle();
        }

        public async void HaberleriYukle()
        {
            HaberGruplari.Clear();
            var kategoriler = new List<(string Kategori, string Kaynak, string Url)>
            {
                ("BBC Türkçe", "BBC", "https://www.bbc.co.uk/turkce/index.xml"),
                ("Hürriyet", "Hürriyet", "https://www.hurriyet.com.tr/rss/anasayfa"),
                ("NTV", "NTV", "https://www.ntv.com.tr/gundem.rss"),
                ("CNN Türk", "CNN", "https://www.cnnturk.com/feed/rss/news"),
                ("Anadolu Ajansı", "AA", "https://www.aa.com.tr/tr/rss/default?cat=guncel")
            };

            foreach (var (Kategori, Kaynak, Url) in kategoriler)
            {
                var haberler = await RssOku(Url, Kategori, Kaynak);
                if (haberler.Any())
                    HaberGruplari.Add(new HaberKategoriGroup(Kategori, haberler));
            }
        }

        public async Task<List<Haber>> RssOku(string url, string kategori, string kaynak)
        {
            var haberList = new List<Haber>();
            try
            {
                var client = new HttpClient();
                var xmlStr = await client.GetStringAsync(url);

                var doc = XDocument.Parse(xmlStr);
                var items = doc.Descendants("item").Take(10);
                foreach (var item in items)
                {
                    haberList.Add(new Haber
                    {
                        Baslik = item.Element("title")?.Value ?? "",
                        Ozet = TemizleHTML(item.Element("description")?.Value ?? ""),
                        Link = item.Element("link")?.Value ?? "",
                        Kategori = kategori,
                        Kaynak = kaynak,
                        Tarih = item.Element("pubDate")?.Value ?? ""
                    });
                }
            }
            catch { }
            return haberList;
        }

        // HTML etiketleri temizle
        string TemizleHTML(string html)
        {
            if (string.IsNullOrWhiteSpace(html)) return "";
            var arr = new System.Text.RegularExpressions.Regex("<.*?>").Replace(html, string.Empty);
            return arr.Replace("&nbsp;", " ").Replace("&quot;", "\"");
        }

        private async void HaberSecildi(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.FirstOrDefault() is Haber secilen)
            {
                await Navigation.PushAsync(new HaberDetayPage(secilen));
                HaberlerCollection.SelectedItem = null; // seçimi temizle
            }
        }

        // CollectionView için grup modeli
        public class HaberKategoriGroup : ObservableCollection<Haber>
        {
            public string Key { get; }
            public HaberKategoriGroup(string key, IEnumerable<Haber> haberler) : base(haberler)
            {
                Key = key;
            }
        }
    }

    // Haber modeli
    public class Haber
    {
        public string Baslik { get; set; }
        public string Ozet { get; set; }
        public string Link { get; set; }
        public string Kategori { get; set; }
        public string Kaynak { get; set; }
        public string Tarih { get; set; }
        public string Resim { get; set; } 
    }
}



