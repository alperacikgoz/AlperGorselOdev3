using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using Microsoft.Maui.Controls;

namespace AlperAcikogoz_Odev3
{
    public partial class KurlarPage : ContentPage
    {
        public class KurModel
        {
            public string Name { get; set; } = string.Empty;
            public string Fiyat { get; set; } = string.Empty;
            public string Icon { get; set; } = string.Empty;
            public string BgColor { get; set; } = "#e3f2fd";
        }

        public KurlarPage()
        {
            InitializeComponent();
            LoadKurlar();
        }

        private async void LoadKurlar()
        {
            LoadingIndicator.IsVisible = true;
            LoadingIndicator.IsRunning = true;
            KurListView.ItemsSource = null;

            try
            {
                var client = new HttpClient();
                var url = "https://finans.truncgil.com/today.json";
                var response = await client.GetStringAsync(url);

                var jsonDoc = JsonDocument.Parse(response);
                var kurList = new List<KurModel>();

                // Döviz + Altın: Key, Display, Emoji, Arkaplan Rengi
                var populerKurlar = new (string Key, string Display, string Icon, string Bg)[]
                {
                    ("USD", "Amerikan Doları (USD)", "💵", "#e3f2fd"),
                    ("EUR", "Euro (EUR)", "💶", "#f3e5f5"),
                    ("GBP", "İngiliz Sterlini (GBP)", "💷", "#fce4ec"),
                    ("CHF", "İsviçre Frangı (CHF)", "💴", "#ffe0b2"),
                    ("RUB", "Rus Rublesi (RUB)", "💰", "#dcedc8"),
                    ("JPY", "Japon Yeni (JPY)", "💴", "#fff9c4"),
                    ("CNY", "Çin Yuanı (CNY)", "💴", "#ffe0b2"),
                    ("BTC", "Bitcoin (BTC)", "₿", "#f9e79f"),

                    ("GRAM ALTIN", "Gram Altın", "🥇", "#fff9c4"),
                    ("ÇEYREK ALTIN", "Çeyrek Altın", "🥈", "#ffe0b2"),
                    ("YARIM ALTIN", "Yarım Altın", "🥉", "#e1bee7"),
                    ("TAM ALTIN", "Tam Altın", "🏅", "#ffe0b2"),
                    ("ONS", "Ons Altın", "🏆", "#b2ebf2"),
                    ("GÜMÜŞ", "Gümüş", "🪙", "#e0f7fa"),
                };

                foreach (var (Key, Display, Icon, Bg) in populerKurlar)
                {
                    if (jsonDoc.RootElement.TryGetProperty(Key, out var value))
                    {
                        string satis = "";
                        if (value.TryGetProperty("Satış", out var satisProp))
                            satis = satisProp.GetString() + " TL";
                        else if (value.TryGetProperty("Alış", out var alisProp))
                            satis = alisProp.GetString() + " TL";
                        else
                            satis = value.ToString() + " TL";

                        kurList.Add(new KurModel { Name = Display, Fiyat = satis, Icon = Icon, BgColor = Bg });
                    }
                }

                // Güncelleme zamanı
                if (jsonDoc.RootElement.TryGetProperty("Update_Date", out var tarih))
                    UpdateTimeLabel.Text = tarih.GetString();
                else
                    UpdateTimeLabel.Text = DateTime.Now.ToShortTimeString();

                KurListView.ItemsSource = kurList;
            }
            catch (Exception ex)
            {
                await DisplayAlert("Hata", "Kurlar yüklenemedi!\n" + ex.Message, "Tamam");
            }
            finally
            {
                LoadingIndicator.IsVisible = false;
                LoadingIndicator.IsRunning = false;
            }
        }

        // Yenile butonuna tıklanınca tekrar veri çeker
        private void OnRefreshClicked(object sender, EventArgs e)
        {
            LoadKurlar();
        }
    }
}
