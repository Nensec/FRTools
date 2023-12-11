using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FRTools.MaskGenerator
{
    class Program
    {
        static async Task Main()
        {
            // To generate specific masks, remove the unwanted ones from the array

            int[] breeds = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 17, 18, 19, 20, 21, 22, 23, 24 };
            int[] genders = { 0, 1 };
            int[] eyetypes = { 0, 5, 6, 11 }; // common = 0, multi gaze = 5, primal = 6, innocent = 11

            foreach (var breed in breeds)
                foreach (var gender in genders)
                    foreach (var eyeType in eyetypes)
                    {
                        if (eyeType == 6) // Primale shape is different for each element
                        {
                            for (int element = 1; element < 12; element++)
                            {
                                await GenerateMask(breed, gender, eyeType, element);
                            }
                        }
                        else
                            await GenerateMask(breed, gender, eyeType, 0);
                    }

        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Interoperability", "CA1416:Validate platform compatibility", Justification = "This is only designed to run on windows")]
        private static async Task GenerateMask(int breed, int gender, int eyeType, int element)
        {
            Console.WriteLine($"Fetching - Breed: {breed} Gender: {gender} Eyetype: {eyeType} Element: {element}");
            while (true)
            {
                try
                {
                    using (var client = new HttpClient())
                    {
                        var html = await client.GetStringAsync($"https://flightrising.com/includes/scryer_getdragon.php?gender={gender}&setage=1&prigene=0&bodycolor=2&secgene=0&wingcolor=2&tertgene=0&tertcolor=10&breed={breed}&elem={element}&eyetype={eyeType}");
                        var img = ScrapeImageUrl(html);
                        var alternateDragon = await client.GetByteArrayAsync(img);

                        html = await client.GetStringAsync($"https://flightrising.com/includes/scryer_getdragon.php?gender={gender}&setage=1&prigene=0&bodycolor=2&secgene=0&wingcolor=2&tertgene=0&tertcolor=2&breed={breed}&elem={element}&eyetype=1");
                        img = ScrapeImageUrl(html);
                        var commonDragon = await client.GetByteArrayAsync(img);

                        Bitmap common, alternate, mask = new(350, 350);

                        using (var memstream = new MemoryStream(commonDragon))
                            common = (Bitmap)Image.FromStream(memstream);

                        using (var memstream = new MemoryStream(alternateDragon))
                            alternate = (Bitmap)Image.FromStream(memstream);

                        for (int x = 0; x < 350; x++)
                            for (int y = 0; y < 350; y++)
                            {
                                var commonPixel = common.GetPixel(x, y);
                                var alternatePixel = alternate.GetPixel(x, y);

                                if (commonPixel != alternatePixel)
                                {
                                    int grayScale = (int)((alternatePixel.R * 0.3) + (alternatePixel.G * 0.59) + (alternatePixel.B * 0.11));
                                    mask.SetPixel(x, y, Color.FromArgb(alternatePixel.A, grayScale, grayScale, grayScale));
                                }
                                else
                                    mask.SetPixel(x, y, Color.Transparent);
                            }

                        var fileName = $"{breed}_{gender}_{eyeType}_{element}.png";

                        Console.WriteLine($"Saving mask: {fileName}");
                        Directory.CreateDirectory("Output");

                        using (var stream = new FileStream("Output\\" + fileName, FileMode.Create))
                            mask.Save(stream, ImageFormat.Png);
                    }
                    break;
                }
                catch { }
            }
        }

        private static string ScrapeImageUrl(string scryerHtmlPage)
        {
            var imgTag = Regex.Match(scryerHtmlPage, @"<img[^>]*?src\s*=\s*[""']?([^'"" >]+?)[ '""][^>]*?>");
            return imgTag.Groups[1].Value;
        }
    }
}
