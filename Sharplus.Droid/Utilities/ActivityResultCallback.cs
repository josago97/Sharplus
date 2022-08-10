using System;

namespace AndroidX.Activity.Result
{
    public class ActivityResultCallback : Java.Lang.Object, IActivityResultCallback
    {
        public event Action<ActivityResult> Callback;

        public ActivityResultCallback() { }

        public ActivityResultCallback(Action<ActivityResult> action) : base()
        {
            Callback += action;
        }

        public void OnActivityResult(Java.Lang.Object result)
        {
            ActivityResult activityResult = result as ActivityResult;

            Callback?.Invoke(activityResult);
        }
    }
}
