using System.Collections.Concurrent;

namespace Opgavesæt4.Services
{
    public static class RequestCounterService
    {
        private static ConcurrentDictionary<string, int> songDictionary = new ConcurrentDictionary<string, int>();
        private static ConcurrentDictionary<string, int> albumDictionary = new ConcurrentDictionary<string, int>();

        public static int AddOrUpdateSong(string path)
        {
            return songDictionary.AddOrUpdate(path, 1, (_, currentValue) => currentValue + 1);
        }

        public static int AddOrUpdateAlbum(string path)
        {
            return albumDictionary.AddOrUpdate(path, 1, (_, currentValue) => currentValue + 1);
        }
    }
}
