namespace Opgavesæt4.Models
{
    public class Album
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public decimal Price { get; set; }
        public string AlbumArtUrl { get; set; }
        public int GenreId { get; set; }
        public Genre Genre { get; set; }
        public int ArtistId { get; set; }
        public Artist Artist { get; set; }
        public IEnumerable<Song> Songs { get; set; }

    }
}
