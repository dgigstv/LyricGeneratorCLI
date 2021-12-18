using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LyricImageGenerator
{
    public static class LyricImage
    {
        private const int DRAW_COLOR_A = 255;
        private const int DRAW_COLOR_R = 255;
        private const int DRAW_COLOR_G = 255;
        private const int DRAW_COLOR_B = 255;

        private const int FADE_COLOR_A = 175;
        private const int FADE_COLOR_R = 255;
        private const int FADE_COLOR_G = 255;
        private const int FADE_COLOR_B = 255;

        private const string FONT = "Arial Black";
        private const int FONT_SIZE = 56;

        private const int IMAGE_WIDTH = 1920;
        private const int IMAGE_HEIGHT = 1080;

        private const int IMAGE_BOTTOM_SPACE = 75; // pixels

        public static Image DrawImage(string fadeLyric, string brightLyric)
        {
            Image image = new Bitmap(IMAGE_WIDTH, IMAGE_HEIGHT);
            using Graphics drawing = Graphics.FromImage(image);

            drawing.SmoothingMode = SmoothingMode.HighQuality;
            drawing.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;

            GraphicsPath fadeGraphicsPath = new GraphicsPath();
            GraphicsPath graphicsPath = new GraphicsPath();
            Color drawColor = Color.FromArgb(DRAW_COLOR_A, DRAW_COLOR_R, DRAW_COLOR_G, DRAW_COLOR_B);
            Color fadeColor = Color.FromArgb(FADE_COLOR_A, FADE_COLOR_R, FADE_COLOR_G, FADE_COLOR_B);
            SolidBrush drawBrush = new SolidBrush(drawColor);
            SolidBrush fadeBrush = new SolidBrush(fadeColor);
            FontFamily font = new FontFamily(FONT);
            Point position = new Point(0, 0);

            fadeGraphicsPath.AddString(fadeLyric, font, (int)FontStyle.Bold, FONT_SIZE, position, StringFormat.GenericDefault);

            if (!string.IsNullOrWhiteSpace(brightLyric))
            {
                graphicsPath.AddString(brightLyric, font, (int)FontStyle.Bold, FONT_SIZE, position, StringFormat.GenericDefault);
            }

            Rectangle measurement = Rectangle.Round(fadeGraphicsPath.GetBounds());
            // Don't use the text height in the calculation. We want everything to be lined up by the top-most edge.
            //drawing.TranslateTransform((image.Width - measurement.Width) / 2 - measurement.X, image.Height - measurement.Height - measurement.Y - IMAGE_BOTTOM_SPACE);
            drawing.TranslateTransform((image.Width - measurement.Width) / 2 - measurement.X, image.Height - measurement.Y - IMAGE_BOTTOM_SPACE);

            drawing.FillPath(fadeBrush, fadeGraphicsPath);

            if (!string.IsNullOrWhiteSpace(brightLyric))
            {
                drawing.FillPath(drawBrush, graphicsPath);
            }

            drawing.Save();

            return image;
        }
    }
}
