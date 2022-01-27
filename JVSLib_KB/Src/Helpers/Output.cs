using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace JVSLib_KB.Src.Helpers
{
    class Output
    {
        //[DllImport("user32.dll", SetLastError = true)]
        //static extern void keybd_event(byte bVk, byte bScan, int dwFlags, int dwExtraInfo);

        //[DllImport("user32.dll")]
        //static extern byte MapVirtualKeyEx(byte uCode, byte uMapType, IntPtr dwhkl);

        //[DllImport("user32.dll")]
        //internal static extern uint SendInput(uint nInputs, [MarshalAs(UnmanagedType.LPArray), In] INPUT[] pInputs, int cbSize);

        //[StructLayout(LayoutKind.Sequential)]
        //public struct INPUT
        //{
        //    internal INPUT_TYPE type;
        //    internal InputUnion U;
        //    internal static int Size
        //    {
        //        get { return Marshal.SizeOf(typeof(INPUT)); }
        //    }
        //}
        //[StructLayout(LayoutKind.Explicit)]
        //internal struct InputUnion
        //{
        //    [FieldOffset(0)]
        //    internal MOUSEINPUT mi;
        //    [FieldOffset(0)]
        //    internal KEYBDINPUT ki;
        //    [FieldOffset(0)]
        //    internal HARDWAREINPUT hi;
        //}

        //[StructLayout(LayoutKind.Sequential)]
        //internal struct MOUSEINPUT
        //{
        //    internal int dx;
        //    internal int dy;
        //    internal int mouseData;
        //    internal MOUSEEVENTF dwFlags;
        //    internal uint time;
        //    internal UIntPtr dwExtraInfo;
        //}
        //[Flags]
        //internal enum MOUSEEVENTF : uint
        //{
        //    ABSOLUTE = 0x8000,
        //    HWHEEL = 0x01000,
        //    MOVE = 0x0001,
        //    MOVE_NOCOALESCE = 0x2000,
        //    LEFTDOWN = 0x0002,
        //    LEFTUP = 0x0004,
        //    RIGHTDOWN = 0x0008,
        //    RIGHTUP = 0x0010,
        //    MIDDLEDOWN = 0x0020,
        //    MIDDLEUP = 0x0040,
        //    VIRTUALDESK = 0x4000,
        //    WHEEL = 0x0800,
        //    XDOWN = 0x0080,
        //    XUP = 0x0100
        //}

        //[StructLayout(LayoutKind.Sequential)]
        //internal struct KEYBDINPUT
        //{
        //    internal short wVk;
        //    internal short wScan;
        //    internal KEYEVENTF dwFlags;
        //    internal int time;
        //    internal UIntPtr dwExtraInfo;
        //}
        //[Flags]
        //internal enum KEYEVENTF : uint
        //{
        //    EXTENDEDKEY = 0x0001,
        //    KEYUP = 0x0002,
        //    SCANCODE = 0x0008,
        //    UNICODE = 0x0004
        //}

        //[StructLayout(LayoutKind.Sequential)]
        //internal struct HARDWAREINPUT
        //{
        //    internal int uMsg;
        //    internal short wParamL;
        //    internal short wParamH;
        //}

        //[Flags]
        //internal enum INPUT_TYPE : uint
        //{
        //    INPUT_MOUSE = 0x0000,
        //    INPUT_KEYBOARD = 0x0001,
        //    INPUT_HARDWARE = 0x0002
        //}

        //public static void KeyDown(Keys key)
        //{
        //    var pInputs = new[]
        //    {
        //      new INPUT()
        //      {
        //          type = INPUT_TYPE.INPUT_KEYBOARD,
        //          U = new InputUnion() {
        //              ki = new KEYBDINPUT()
        //              {
        //                wScan = MapVirtualKeyEx((byte)key, 0x00, IntPtr.Zero),
        //                wVk = (short)key,
        //                dwFlags = KEYEVENTF.EXTENDEDKEY;
        //              }
        //          }
        //          }
        //       };
        //    SendInput((uint)pInputs.Length, pInputs, INPUT.Size);
        //}

        //public static void KeyUp(Keys key)
        //{

        //}

        public static uint IsExtendedKey(Keys key)
        {
            switch (key)
            {
                case Keys.Insert:
                case Keys.Delete:
                case Keys.Home:
                case Keys.End:
                case Keys.PageUp:
                case Keys.PageDown:
                case Keys.Up:
                case Keys.Down:
                case Keys.Left:
                case Keys.Right:
                case Keys.NumLock:
                case Keys.PrintScreen:
                case Keys.Divide:
                case Keys.Enter:
                    return 1;
            }
            return 0;
        }
    }
}
