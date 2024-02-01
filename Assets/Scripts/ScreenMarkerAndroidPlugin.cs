using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenMarkerAndroidPlugin : MonoBehaviour
{
    private AndroidJavaClass _unityClass;
    private AndroidJavaObject _unityActivity;
    private AndroidJavaClass _pluginClass;
    private AndroidJavaObject _pluginInstance;

    [SerializeField]
    private Texture2D _image;

    private const string _pluginPackageName = "com.eis.plugin.ScreenMarker"; //TODO: Change this to your package name


    void Start()
    {
        _unityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        _unityActivity = _unityClass.GetStatic<AndroidJavaObject>("currentActivity");

        if (_unityActivity == null)
        {
            Debug.LogError("No Unity Activity");
        }

        _pluginClass = new AndroidJavaClass(_pluginPackageName); // Retreive class info. Note that this is not "instantiating".

        _pluginClass.CallStatic("implementationTest"); // Calling static method.
        _pluginClass.CallStatic("setIsUnity", true); // This must be called to notify this app is working with unity.
        _pluginInstance = new AndroidJavaObject(_pluginPackageName, _unityActivity, "12345"); // Instantiate ScreenMarker class instance.
    }

    public void PrintBasic()
    {
        if (_pluginInstance == null)
        {
            Debug.LogError("Plugin not initilized");
        }

        /* 
         * All instance methods must be call in the following way to run on UI thread.
         * If you don't call methods on the UI thread, it will give some errors.
         */

        _unityActivity.Call("runOnUiThread", new AndroidJavaRunnable(() =>
        {
            _pluginInstance.Call("setTextAll", "This is the moment");
            _pluginInstance.Call("showScreenMarker");
        }));
    }

    public void PrintTiledTextAndImage()
    {
        if (_pluginInstance == null)
        {
            Debug.LogError("Plugin not initilized");
        }

        var bitmap = ToAndroidBitmap(_image);

        _unityActivity.Call("runOnUiThread", new AndroidJavaRunnable(() =>
        {
            /*
             * Rotation parameter "must" be passed as float type.
             * Passing 30 will not work as it is an integer value.
             * 30.0f must be passed.
             */
            _pluginInstance.Call("setTextTileMode", 200, 60, "Hello", null, 20, 0x4c000000, -30.0f);
            _pluginInstance.Call("setImageTileMode", bitmap, -30.0f);
        }));
    }

    public void Reset()
    {
        if (_pluginInstance == null)
        {
            Debug.LogError("Plugin not initilized");
        }

        _unityActivity.Call("runOnUiThread", new AndroidJavaRunnable(() =>
        {
            _pluginInstance.Call("unsetImageTileMode");
            _pluginInstance.Call("unsetTextTileMode");
        }));
    }

    public void HideScreenMarker()
    {
        if (_pluginInstance == null)
        {
            Debug.LogError("Plugin not initilized");
        }

        _unityActivity.Call("runOnUiThread", new AndroidJavaRunnable(() =>
        {
            _pluginInstance.Call("hideScreenMarker");
        }));
    }


    /*
     * Convert Texture2D to Android Bitmap class so that we can 
     * directly pass the onverted bitmap instance to the target android function.
     */
    private static AndroidJavaObject ToAndroidBitmap(Texture2D tex2D)
    {
        byte[] pngBytes = tex2D.EncodeToPNG();
        return CallStaticOnce("android.graphics.BitmapFactory", "decodeByteArray", pngBytes, 0, pngBytes.Length);
    }

    private static AndroidJavaObject CallStaticOnce(string className, string methodName, params object[] args)
    {
        using var ajc = new AndroidJavaClass(className);
        return ajc.CallStatic<AndroidJavaObject>(methodName, args);
    }
}
