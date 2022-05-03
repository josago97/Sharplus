using System.IO;
using Android.Graphics;

namespace Sharplus.Droid
{
    public static class Extensions
    {
        public static byte[] ToByteArray(this Bitmap bitmap, Bitmap.CompressFormat format, int quality = 0)
        {
            using MemoryStream stream = new MemoryStream();
            bitmap.Compress(format, quality, stream);
            return stream.ToArray();
        }
    }
}