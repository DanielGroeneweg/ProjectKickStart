using UnityEngine;
public class EnableAlpha : MonoBehaviour
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
    static void ForceAlphaChannel()
    {
        // Unity 2020+ only: forces backbuffer to keep alpha
        Application.SetStackTraceLogType(LogType.Log, StackTraceLogType.None);
        Screen.fullScreenMode = FullScreenMode.Windowed;
    }
}