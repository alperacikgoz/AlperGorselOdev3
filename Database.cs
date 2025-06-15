using SQLite;

namespace AlperAcikogoz_Odev3
{
    public class Database
    {
        private static SQLiteAsyncConnection db;
        private static bool initialized = false;

        public static async Task Init()
        {
            if (initialized) return;
            var dbPath = Path.Combine(FileSystem.AppDataDirectory, "gorevler.db");
            db = new SQLiteAsyncConnection(dbPath);
            await db.CreateTableAsync<Gorev>();
            initialized = true;
        }

        public static Task<List<Gorev>> GetGorevler() => db.Table<Gorev>().OrderBy(x => x.Tarih).ThenBy(x => x.Saat).ToListAsync();
        public static Task<int> AddGorev(Gorev gorev) => db.InsertAsync(gorev);
        public static Task<int> UpdateGorev(Gorev gorev) => db.UpdateAsync(gorev);
        public static Task<int> DeleteGorev(Gorev gorev) => db.DeleteAsync(gorev);
        public static Task<Gorev> GetGorevById(int id) => db.Table<Gorev>().Where(x => x.Id == id).FirstOrDefaultAsync();
    }
}
