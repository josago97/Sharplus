using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Android.Content;
using Android.Graphics;
using Android.Runtime;
using Android.Util;

namespace Sharplus.Droid.Widgets
{
    public class ImageView : Android.Widget.ImageView
    {
        private CancellationTokenSource _cancellationTokenSource;

        public ImageView(Context context) : base(context) { }

        public ImageView(Context context, IAttributeSet attrs) : base(context, attrs) { }

        public ImageView(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr) { }

        public ImageView(Context context, IAttributeSet attrs, int defStyleAttr, int defStyleRes) : base(context, attrs, defStyleAttr, defStyleRes) { }

        protected ImageView(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer) { }

        public async Task SetImageUrlAsync(string imageUrl, int? resourceImageDefault = null, CancellationToken cancellation = default)
        {
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellation);

            CancellationToken cancellationToken = _cancellationTokenSource.Token;
            Bitmap image = await DownloadImageAsync(imageUrl);

            if (!cancellationToken.IsCancellationRequested)
            {
                if (image != null)
                    SetImageBitmap(image);
                else if (resourceImageDefault.HasValue)
                    SetImageResource(resourceImageDefault.Value);
            }
        }

        private async Task<Bitmap> DownloadImageAsync(string url)
        {
            Bitmap imageBitmap = null;

            try
            {
                using HttpClient httpClient = new HttpClient();
                byte[] imageBytes = await httpClient.GetByteArrayAsync(url);

                if (imageBytes != null && imageBytes.Length > 0)
                {
                    imageBitmap = BitmapFactory.DecodeByteArray(imageBytes, 0, imageBytes.Length);
                }
            }
            catch { }

            return imageBitmap;
        }
    }
}