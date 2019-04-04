using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace delete68
{
    class Program
    {
        static void Main(string[] args)
        {
            var files = new string[] { @"C:\Temp\1.png", @"C:\Temp\2.png", @"C:\Temp\3.png", @"C:\Temp\4.png", @"C:\Temp\5.png", @"C:\Temp\6.png", @"C:\Temp\7.png", @"C:\Temp\8.png", @"C:\Temp\9.png" };

            Bitmap img = CombineBitmap(files);
            Bitmap resized = new Bitmap(img, new Size(800, 800));

            resized.Save(@"C:\Temp\final.png", ImageFormat.Png);



        }
            public static System.Drawing.Bitmap CombineBitmap(string[] files)
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

                //get a graphics object from the image so we can draw on it
                using (System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(finalImage))
                {
                  

                    //go through each image and draw it on the final image
                    int offsetx = 0;
                    int offsety = 0;
                    int j = 0;
                    foreach (System.Drawing.Bitmap image in images)
                    {
                      

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
