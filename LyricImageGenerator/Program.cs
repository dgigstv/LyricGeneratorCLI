using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Text;

namespace LyricImageGenerator
{
    public class Program
    {
        static int Main(string[] args)
        {
            if (args.Length != 1)
            {
                Console.WriteLine("Wrong number of arguments");
                Console.WriteLine("Usage: LyricImageGenerator <lyrics.txt>");
                return 1;
            }

            if (!Directory.Exists("output"))
            {
                Directory.CreateDirectory("output");
            }

            StreamReader reader = new StreamReader(args[0]);
            int count = 1;
            string lyric;
            while ((lyric = reader.ReadLine()) != null) {
                string[] words = lyric.Split(' ');
                string brightLyric = "";
                string outputDirectoryName = SanitizeFileName($"{count} {lyric}");
                string outputDirectory = Path.Join("output", outputDirectoryName);

                if (!Directory.Exists(outputDirectory))
                {
                    Directory.CreateDirectory(outputDirectory);
                }

                using (Image image = LyricImage.DrawImage(lyric, brightLyric.Trim()))
                {
                    image.Save(Path.Join(outputDirectory, "frame_1.png"), System.Drawing.Imaging.ImageFormat.Png);
                }

                for (int i = 0; i < words.Length; i++)
                {
                    string word = words[i];
                    brightLyric += word + " ";
                    using (Image image = LyricImage.DrawImage(lyric, brightLyric.Trim()))
                    {
                        image.Save(Path.Join(outputDirectory, $"frame_{i + 2}.png"), System.Drawing.Imaging.ImageFormat.Png);
                    }
                }

                count++;
            }

            return 0;
        }

        private static string SanitizeFileName(string fileName)
        {
            char[] invalid = Path.GetInvalidFileNameChars();
            StringBuilder newFileName = new StringBuilder(fileName);
            int idx;
            while ((idx = newFileName.ToString().IndexOfAny(invalid)) != -1)
            {
                newFileName[idx] = '_';
            }

            return newFileName.ToString();
        }
    }
}
