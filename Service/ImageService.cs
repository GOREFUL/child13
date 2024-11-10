using System.Drawing;
using ImageFormatWPF = System.Drawing.Imaging.ImageFormat;

namespace child13.Service
{
    public class ImageService
    {
        public static Bitmap CaptureScreenArea(int x1, int y1, int x2, int y2)
        {
            int width = x2 - x1;
            int height = y2 - y1;
            Bitmap bitmap = new Bitmap(width, height);
            using (Graphics g = Graphics.FromImage(bitmap))
            {
                g.CopyFromScreen(x1, y1, 0, 0, bitmap.Size);
            }

            return bitmap;
        }
        public static void PreprocessImage(string path,
            int x1, int y1, int x2, int y2)
        {
            Bitmap capturedImage = CaptureScreenArea(x1, y1, x2, y2);
            capturedImage.Save(path, ImageFormatWPF.Png);
        }
    }
}
