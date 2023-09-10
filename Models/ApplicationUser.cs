using Microsoft.AspNetCore.Identity;

namespace Opgavesæt4.Models
{
    public class ApplicationUser : IdentityUser
    {
        public IEnumerable<Playlist> Playlists { get; set; }
    }
}
