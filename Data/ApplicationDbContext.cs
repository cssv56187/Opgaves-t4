using Humanizer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Opgavesæt4.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using System;

namespace Opgavesæt4.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Playlist> Playlists { get; set; }
        public DbSet<Album> Albums { get; set; }
        public DbSet<Artist> Artists { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Song> Songs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Seed data for Genre
            modelBuilder.Entity<Genre>().HasData(
                new Genre { Id = 1, Name = "Rock", Description = "Rock music genre" },
                new Genre { Id = 2, Name = "Pop", Description = "Pop music genre" }
                // Add more genres as needed
            );

            // Seed data for Artist
            modelBuilder.Entity<Artist>().HasData(
                new Artist { Id = 1, Name = "Artist1" },
                new Artist { Id = 2, Name = "Artist2" }
                // Add more artists as needed
            );
            // Seed data for Album
            modelBuilder.Entity<Album>().HasData(
                new Album
                {
                    Id = 1,
                    Title = "Album1",
                    Price = 9.99m,
                    GenreId = 1,
                    ArtistId = 1,
                    AlbumArtUrl = "test"
                },
                new Album
                {
                    Id = 2,
                    Title = "Album2",
                    Price = 12.99m,
                    GenreId = 2,
                    ArtistId = 2,
                    AlbumArtUrl = "test"
                }
                // Add more albums as needed
            );
            // Seed data for ApplicationUser (IdentityUser)
            // You can seed IdentityUser if needed

            // Seed data for Playlist
            modelBuilder.Entity<Playlist>().HasData(
                new Playlist { Id = 1, Name = "My Playlist" }
                // Add more playlists as needed
            );
            // Seed data for Song
            modelBuilder.Entity<Song>().HasData(
                new Song
                {
                    Id = 1,
                    Name = "Song1",
                    Url = "song1-url",
                    PlaylistId = 1,
                    AlbumId = 1
                },
                new Song
                {
                    Id = 2,
                    Name = "Song2",
                    Url = "song2-url",
                    PlaylistId = 1,
                    AlbumId = 1
                }
             );
        }
        public void ClearDatabase()
        {
            Database.EnsureDeleted();
            Database.EnsureCreated();
        }

        public static void SeedApplicationUsers(UserManager<ApplicationUser> userManager, ApplicationDbContext _context)
        {
        // Check if the user already exists
            if (userManager.FindByEmailAsync("user@example.com").Result == null)
            {
                ApplicationUser user = new ApplicationUser
                {
                    UserName = "user@example.com",
                    Email = "user@example.com",
                    EmailConfirmed = true
                    // Add other properties as needed
                    };
                IdentityResult result = userManager.CreateAsync(user, "YourPassword123!").Result;
                if (result.Succeeded)
                {
                    var playlists = _context.Playlists.Where(p => p.Id == 1).ToList();
                    foreach (var playlist in playlists)
                    {
                        playlist.UserId = user.Id;
                    }
                    _context.SaveChanges();
                }
            }
        }
    }
}