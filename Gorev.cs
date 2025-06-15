using SQLite;

namespace AlperAcikogoz_Odev3
{
    public class Gorev
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Baslik { get; set; } = "";
        public string Detay { get; set; } = "";
        public DateTime Tarih { get; set; }
        public TimeSpan Saat { get; set; }
        public bool Yapildi { get; set; } = false;
    }
}
