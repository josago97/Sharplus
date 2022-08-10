using System.Text.Json;
using Android.Content;

namespace Sharplus.Droid
{
    public static class IntentExtensions
    {
        public static Intent PutExtra<T>(this Intent intent, string name, T extra)
        {
            string json = JsonSerializer.Serialize(extra);
            intent.PutExtra(name, json);
            return intent;
        }

        public static T GetExtra<T>(this Intent intent, string name)
        {
            T result;

            intent.TryGetExtra(name, out result);

            return result;
        }

        public static bool TryGetExtra<T>(this Intent intent, string name, out T result)
        {
            result = default(T);
            bool success = false;

            try
            {
                result = JsonSerializer.Deserialize<T>(intent.GetStringExtra(name));
                success = true;
            }
            catch { }

            return success;
        }
    }
}
