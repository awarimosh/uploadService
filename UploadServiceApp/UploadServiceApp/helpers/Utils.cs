using System;
using System.Collections.Generic;
using System.IO;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Encoder = System.Drawing.Imaging.Encoder;

namespace UploadServiceApp.Helpers
{
    public class Utils
    {
        //set the resolution, 72 is usually good enough for displaying images on monitors
        static float imageResolution = 72;

        //set the compression level. higher compression = better quality = bigger images
        static long compressionLevel = 80L;

        public static String getRootFolder()
        {
            string currentPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string parentPath = Path.GetFullPath(Path.Combine(Path.Combine(currentPath, ".."), ".."));

            return parentPath;
        }

        public static void saveThumb(Image image, string filename, int width)
        {
            double resolution = (double) image.Height / image.Width;
            int newWidth = width;
            int newHeight = (int) Math.Round(width * resolution);
            Console.WriteLine(String.Format("original {0} x {1} --- generated {2} x {3}", image.Width, image.Height, newWidth, newHeight));

            //start the resize with a new image
            Bitmap newImage = new Bitmap(newWidth, newHeight);

            //set the new resolution
            newImage.SetResolution(imageResolution, imageResolution);

            //start the resizing
            using (var graphics = Graphics.FromImage(newImage))
            {
                //set some encoding specs
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                graphics.DrawImage(image, 0, 0, newWidth, newHeight);
            }

            //save the image to a memorystream to apply the compression level
            using (MemoryStream ms = new MemoryStream())
            {
                EncoderParameters encoderParameters = new EncoderParameters(1);
                encoderParameters.Param[0] = new EncoderParameter(Encoder.Quality, compressionLevel);

                newImage.Save(ms, GetEncoderInfo("image/jpeg"), encoderParameters);

                //save the image as byte array here if you want the return type to be a Byte Array instead of Image
                //byte[] imageAsByteArray = ms.ToArray();
            }

            //return the image
            newImage.Save(Path.Combine(Utils.getRootFolder(), "thumbs", width + "_" + filename));
        }

        private static ImageCodecInfo GetEncoderInfo(String mimeType)
        {
            int j;
            ImageCodecInfo[] encoders;
            encoders = ImageCodecInfo.GetImageEncoders();
            for (j = 0; j < encoders.Length; ++j)
            {
                if (encoders[j].MimeType == mimeType)
                    return encoders[j];
            }
            return null;
        }
    }
}
