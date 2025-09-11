using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System;
public class TransparentObject : MonoBehaviour
{
    // ----- Win32 constants -----
    const int GWL_STYLE = -16;
    const int GWL_EXSTYLE = -20;

    const uint WS_POPUP = 0x80000000;
    const uint WS_VISIBLE = 0x10000000;

    const uint WS_EX_LAYERED = 0x00080000;
    const uint WS_EX_TRANSPARENT = 0x00000020;

    const int LWA_COLORKEY = 0x1;

    const uint SWP_NOMOVE = 0x0002;
    const uint SWP_NOSIZE = 0x0001;
    const uint SWP_SHOWWINDOW = 0x0040;

    static readonly IntPtr HWND_TOPMOST = new IntPtr(-1);

    // ----- Win32 imports -----
    [DllImport("user32.dll", SetLastError = true)]
    static extern int GetWindowLong(IntPtr hWnd, int nIndex);

    [DllImport("user32.dll", SetLastError = true)]
    static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

    [DllImport("user32.dll", SetLastError = true)]
    static extern bool SetLayeredWindowAttributes(IntPtr hwnd, uint crKey, byte bAlpha, uint dwFlags);

    [DllImport("user32.dll", SetLastError = true)]
    static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter,
                                    int X, int Y, int cx, int cy, uint uFlags);

    [DllImport("user32.dll")]
    private static extern IntPtr GetActiveWindow();

    void Awake()
    {
        Application.runInBackground = true;
        Application.targetFrameRate = 60;

        IntPtr hWnd = GetActiveWindow();

        // ----- Apply base window style -----
        int style = GetWindowLong(hWnd, GWL_STYLE);
        SetWindowLong(hWnd, GWL_STYLE, unchecked((int)(WS_POPUP | WS_VISIBLE)));

        // ----- Apply extended styles (layered + click-through) -----
        int exStyle = GetWindowLong(hWnd, GWL_EXSTYLE);
        SetWindowLong(hWnd, GWL_EXSTYLE, exStyle | (int)(WS_EX_LAYERED | WS_EX_TRANSPARENT));

        // ----- Set transparency (black = transparent) -----
        SetLayeredWindowAttributes(hWnd, 0x000000, 0, LWA_COLORKEY);

        // ----- Force window to topmost -----
        SetWindowPos(hWnd, HWND_TOPMOST, 0, 0, 0, 0, SWP_NOMOVE | SWP_NOSIZE | SWP_SHOWWINDOW);

        Debug.Log("Transparent overlay initialized (always-on-top). Press ESC to quit.");
    }

    void Update()
    {
        // Safety quit hotkey
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("Exiting overlay...");
            Application.Quit();
        }
    }
}