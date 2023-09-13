using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Bson;
using Opgavesæt4.Services;

namespace Opgavesæt4.CustomMiddleware
{
    public class CustomLogger
    {
        private readonly RequestDelegate _next;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public CustomLogger(RequestDelegate next, IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Path.ToString().ToLower().Contains("details"))
            {
                var splitPath = context.Request.Path.ToString().ToLower().Split('/');
                if (splitPath[1] == "albums")
                {
                    try
                    {
                        string rootPath = _webHostEnvironment.WebRootPath.Replace('\\', '/');
                        var folderPath = Path.Combine(rootPath + "/logFiles/");
                        var filePath = Path.Combine(rootPath + "/logFiles/", "albumLogs");
                        if (!Directory.Exists(folderPath))
                        {
                            Directory.CreateDirectory(folderPath);
                        }
                        if(!File.Exists(filePath))
                        {
                            var totalCount = RequestCounterService.AddOrUpdateAlbum($"Album{splitPath[3]}");
                            File.WriteAllLines(filePath, new List<string> { $"Album med Id: {splitPath[3]} efterspurgt. Total album requests: {totalCount}" });
                        }
                        else
                        {
                            var totalCount = RequestCounterService.AddOrUpdateAlbum($"Album{splitPath[3]}");
                            File.AppendAllLines(filePath, new List<string> { $"Album med Id: {splitPath[3]} efterspurgt. Total album requests: {totalCount}" });
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"error logging album count to file: {ex.Message}");
                    }

                }
                else if (splitPath[1] == "songs")
                {
                    try
                    {
                        string rootPath = _webHostEnvironment.WebRootPath.Replace('\\', '/');
                        var folderPath = Path.Combine(rootPath + "/logFiles/");
                        var filePath = Path.Combine(rootPath + "/logFiles/", "songLogs");
                        if (!Directory.Exists(folderPath))
                        {
                            Directory.CreateDirectory(folderPath);
                        }
                        if (!File.Exists(filePath))
                        {
                            var totalCount = RequestCounterService.AddOrUpdateSong($"Song{splitPath[3]}");
                            File.WriteAllLines(filePath, new List<string> { $"Song med Id: {splitPath[3]} efterspurgt. Total song requests: {totalCount}" });
                        }
                        else
                        {
                            var totalCount = RequestCounterService.AddOrUpdateSong($"Song{splitPath[3]}");
                            File.AppendAllLines(filePath, new List<string> { $"Song med Id: {splitPath[3]} efterspurgt. Total song requests: {totalCount} " });
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"error logging song count to file: {ex.Message}");
                    }
                }
            }
            
            await _next(context);
        }
    }
}
