using AForge.Imaging.Filters;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;

namespace delete68
{
    class Program
    {
        static void Main(string[] args)
        { 

            var files = new string[] { @"C:\Temp\35_20.png", @"C:\Temp\35_25.png", @"C:\Temp\35_30.png", @"C:\Temp\40_20.png", @"C:\Temp\40_25.png", @"C:\Temp\40_30.png", @"C:\Temp\45_20.png", @"C:\Temp\45_25.png", @"C:\Temp\45_30.png" };
            string back = @"C:\Temp\back.png";

            Bitmap img = CombineBitmap(files, back);
              Bitmap resized = new Bitmap(img, new Size(600, 600));
            resized.Save(@"C:\temp\ORIGINAL.png");

            Bitmap newImage = new Bitmap(600, 600);
            using (Graphics gr = Graphics.FromImage(newImage))
            {
                gr.SmoothingMode = SmoothingMode.HighQuality;
                gr.InterpolationMode = InterpolationMode.HighQualityBicubic;
                gr.PixelOffsetMode = PixelOffsetMode.HighQuality;
                gr.DrawImage(resized, new Rectangle(0, 0, 600, 600));
            }
            newImage.Save(@"C:\temp\ENHANDED.png");


            img.Save(@"C:\Temp\final.png", ImageFormat.Png);



        }
        


        private void DrawBitmapWithBorder(Bitmap bmp, Point pos, Graphics g)
        {
            const int borderSize = 20;

            using (Brush border = new SolidBrush(Color.White /* Change it to whichever color you want. */))
            {
                g.FillRectangle(border, pos.X - borderSize, pos.Y - borderSize,
                    bmp.Width + borderSize, bmp.Height + borderSize);
            }

            g.DrawImage(bmp, pos);
        }


        public static System.Drawing.Bitmap CombineBitmap(string[] files, string backgroundImage)
        {
            //read all images into memory
            List<System.Drawing.Bitmap> images = new List<System.Drawing.Bitmap>();
            System.Drawing.Bitmap finalImage = null;

            try
            {
                int width = 0;
                int height = 0;

                int i = 0;
                foreach (string image in files)
                {
                    //create a Bitmap from the file and add it to the list
                    System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(image);

                    //update the size of the final bitmap
                    if(i<3)
                    width += bitmap.Width;

                    if(i%3==0)
                    height += bitmap.Height;

                    images.Add(bitmap);

                    i++;
                }

                //create a bitmap to hold the combined image
                finalImage = new System.Drawing.Bitmap(width, height);


                System.Drawing.Bitmap backbmp = new System.Drawing.Bitmap(backgroundImage);
              


                //get a graphics object from the image so we can draw on it
                using (System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(finalImage))
                {
                    g.DrawImage(backbmp, new System.Drawing.Rectangle(0, 0, finalImage.Width, finalImage.Height));

                  

                    //go through each image and draw it on the final image
                    int offsetx = 0;
                    int offsety = 0;
                    int j = 0;
                    foreach (System.Drawing.Bitmap image in images)
                    {

                        g.DrawLine(new Pen(Brushes.Gray, 5), new Point(offsetx, offsety), new Point(offsetx, offsety+ image.Height));
                        g.DrawLine(new Pen(Brushes.Gray, 5), new Point(offsetx, offsety), new Point(offsetx+ image.Width, offsety));
                        g.DrawLine(new Pen(Brushes.Gray, 5), new Point(offsetx, offsety + image.Height), new Point(offsetx + image.Width, offsety + image.Height));
                        g.DrawLine(new Pen(Brushes.Gray, 5), new Point(offsetx + image.Width, offsety), new Point(offsetx + image.Width, offsety + image.Height));

                        g.DrawImage(image, new System.Drawing.Rectangle(offsetx, offsety, image.Width, image.Height));

                        


                        offsetx += image.Width;
                        if (j != 0 && j % 3 == 2)
                        {
                            offsetx = 0;
                            offsety += image.Height;
                        }

                        j++;

                    }
                }

                return finalImage;
            }
            catch (Exception ex)
            {
                if (finalImage != null)
                    finalImage.Dispose();

                throw ex;
            }
            finally
            {
                //clean up memory
                foreach (System.Drawing.Bitmap image in images)
                {
                    image.Dispose();
                }
            }
        }
    }
}
