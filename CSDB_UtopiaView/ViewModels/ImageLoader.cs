using System;
using System.Collections.Generic;
using System.IO;
using Avalonia.Media.Imaging;
using Avalonia.Platform;

namespace CSDB_UtopiaView.ViewModels;

public static class ImageLoader
{
    private static Dictionary<string, Bitmap> _images = new ();

    private const string BaseUrl = "avares://CSDB_UtopiaView/Assets/";
    private const string defaultFilename = "Fields/0trees.PNG";
    public static Bitmap? Get(string name)
    {
        Console.WriteLine($"Images loaded: {_images.Count}");
        Bitmap? image;
        string filename = BaseUrl+name;
        bool has = _images.TryGetValue(filename, out image);
        try
        {
            if (!has)
                using (Stream stream = AssetLoader.Open(new Uri(filename)))
                {
                    image = new Bitmap(stream);
                    _images.Add(filename, image);
                }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        
        return image;
    }

    public static Bitmap? GetDefault() => Get(defaultFilename);
   
}