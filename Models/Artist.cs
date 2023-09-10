namespace Opgavesæt4.Models
{
    public class Artist
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<Album> Albums { get; set;}
    }
}
