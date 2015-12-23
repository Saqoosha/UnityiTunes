using UnityEngine;
using System;
using System.Runtime.InteropServices;

public class MonoPInvokeCallbackAttribute : Attribute
{
    private Type type;
    public MonoPInvokeCallbackAttribute(Type t)
    {
        type = t;
    }
}

public class iTunesHelper
{
    private delegate void callback(string message);
 
    [MonoPInvokeCallback(typeof(callback))]
    private static void _internalStatusChange(string message)
    {
        if (OnStatusChanged != null)
        {
            OnStatusChanged(message);
        }
    }

    [DllImport("UnityiTunes")]
    private static extern void _Init(callback callback);
    [DllImport("UnityiTunes")]
    private static extern void _Cleanup();
    [DllImport("UnityiTunes")]
    public static extern double _GetPlayPosition();
    [DllImport("UnityiTunes")]
    private static extern IntPtr _GetArtist();
    [DllImport("UnityiTunes")]
    private static extern IntPtr _GetTitle();
    [DllImport("UnityiTunes")]
    private static extern double _GetDuration();

    public static event Action<string> OnStatusChanged;

    public static void Init()
    {
        _Init(_internalStatusChange);
    }

    public static void Cleanup()
    {
        _Cleanup();
    }

    public static float PlayerPosition
    {
        get
        {
            return (float)_GetPlayPosition();
        }
    }

    public static string CurrentArtist
    {
        get
        {
            return Marshal.PtrToStringAuto(_GetArtist());
        }
    }

    public static string CurrentTitle
    {
        get
        {
            return Marshal.PtrToStringAuto(_GetTitle());
        }
    }

    public static float Duration
    {
        get
        {
            return (float)_GetDuration();
        }
    }
    
}
