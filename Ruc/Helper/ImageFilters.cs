using System.Drawing;

namespace Ruc.Helper
{
    internal static class ImageFilters
    {
        public static void ImageToBlackAndWhite(Bitmap bm)
        {
            for (var x = 0; x < bm.Width; x++)
                for (var y = 0; y < bm.Height; y++)
                {
                    var pix = bm.GetPixel(x, y);
                    bm.SetPixel(x, y, pix.GetBrightness() > 0.90f ? Color.White : Color.Black);
                }
        }
    }
}
