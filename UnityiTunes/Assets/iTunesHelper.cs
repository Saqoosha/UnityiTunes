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


    // enum iTunesEPlS {
    //     iTunesEPlSStopped = 'kPSS',
    //     iTunesEPlSPlaying = 'kPSP',
    //     iTunesEPlSPaused = 'kPSp',
    //     iTunesEPlSFastForwarding = 'kPSF',
    //     iTunesEPlSRewinding = 'kPSR'
    // };
    // from above definition, convert it to int by JavaScript like this:
    // +function(key){return key.charCodeAt(0)<<24 | key.charCodeAt(1)<<16 | key.charCodeAt(2)<<8 | key.charCodeAt(3);}("kPSR")
    public enum iTunesStatus
    {
        Stopped = 1800426323,
        Playing = 1800426320,
        Paused = 1800426352,
        FastForwarding = 1800426310,
        Rewinding = 1800426322,
    }


    [DllImport("UnityiTunes")]
    private static extern void _Init(callback callback);
    [DllImport("UnityiTunes")]
    public static extern void Cleanup();
    [DllImport("UnityiTunes")]
    public static extern void PlayPause();
    [DllImport("UnityiTunes")]
    public static extern void Stop();
    [DllImport("UnityiTunes")]
    public static extern void Rewind();
    [DllImport("UnityiTunes")]
    public static extern int _GetStatus();
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

    public static iTunesStatus Status { get { return (iTunesStatus)_GetStatus(); } }
    public static float PlayerPosition { get { return (float)_GetPlayPosition(); } }
    public static string CurrentArtist { get { return Marshal.PtrToStringAuto(_GetArtist()); } }
    public static string CurrentTitle { get { return Marshal.PtrToStringAuto(_GetTitle()); } }
    public static float Duration { get { return (float)_GetDuration(); } }
    
}
