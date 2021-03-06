using System.Drawing;
using System.Drawing.Imaging;
using System;

namespace Lab3ImageView
{
    static class  ImageController
    {
        private static Image original;

        private static Bitmap modified;

        private static Bitmap noise;

        private static Random rnd = new Random();

        public static void adjust(float b, float a, bool IsNoise = false)
        {
            ColorMatrix cm = new ColorMatrix(new float[][]
            {
                new float [] {b,0,0,0,0},
                new float [] {0,b,0,0,0},
                new float [] {0,0,b,0,0},
                new float [] {0,0,0,a,0},
                new float [] {0,0,0,0,1},
            });

            ImageAttributes attributes = new ImageAttributes();
            attributes.SetColorMatrix(cm);

            Point[] points =
            {
                new Point(0,0),
                new Point(original.Width,0),
                new Point(0, original.Height),
            };
            Rectangle rectangle = new Rectangle(0, 0, original.Width, original.Height);

            Bitmap bitmap = new Bitmap(original.Width, original.Height);
            using(Graphics gr = Graphics.FromImage(bitmap))
            {
                gr.DrawImage(original, points, rectangle, GraphicsUnit.Pixel, attributes);

                if (IsNoise)
                {
                    if (noise == null || noise.Width != original.Width || noise.Height != original.Height)
                        genNoise(original.Width, original.Height);
                    gr.DrawImage(noise, points, rectangle, GraphicsUnit.Pixel);
                }
            }

            if (modified != null)
                modified.Dispose();
            modified = bitmap;
        }

        private static void genNoise(int w, int h)
        {
            Bitmap bmp = new Bitmap(w, h);

            for(int x = 0; x < bmp.Width; x++)
                for(int y = 0; y < bmp.Height; y++)
                {
                    int r = rnd.Next(0, 255);
                    bmp.SetPixel(x, y, Color.FromArgb(150, r, r, r));
                }

            if (noise != null)
                noise.Dispose();

            noise = bmp;
        }

        public static Bitmap getModified()
        {
            return modified;
        }

        public static void setOriginal(Image image)
        {
            if (original == image)
                return;

            if(original != null)
                original.Dispose();

            original = image;

        }

    }
}
