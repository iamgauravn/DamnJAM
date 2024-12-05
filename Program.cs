using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DamnJAM
{
    public class Program
    {
        private const uint INPUT_MOUSE = 0;

        private const uint MOUSEEVENTF_LEFTDOWN = 0x0002;
        private const uint MOUSEEVENTF_LEFTUP = 0x0004;

        [StructLayout(LayoutKind.Sequential)]
        struct INPUT
        {
            public uint type;
            public MOUSEINPUT mi;
        }

        [StructLayout(LayoutKind.Sequential)]
        struct MOUSEINPUT
        {
            public int dx;
            public int dy;
            public uint mouseData;
            public uint dwFlags;
            public uint time;
            public IntPtr dwExtraInfo;
        }

        [DllImport("user32.dll")]
        static extern uint SendInput(uint nInputs, INPUT[] pInputs, int cbSize);

        static void SimulateMouseClickAtCurrentPosition()
        {
            INPUT[] inputs = new INPUT[2];

            inputs[0] = new INPUT
            {
                type = INPUT_MOUSE,
                mi = new MOUSEINPUT
                {
                    dwFlags = MOUSEEVENTF_LEFTDOWN
                }
            };

            inputs[1] = new INPUT
            {
                type = INPUT_MOUSE,
                mi = new MOUSEINPUT
                {
                    dwFlags = MOUSEEVENTF_LEFTUP
                }
            };

            SendInput((uint)inputs.Length, inputs, Marshal.SizeOf(typeof(INPUT)));
        }

        static void Main()
        {

            Console.WriteLine("Enter the duration in minutes :");
            if (!int.TryParse(Console.ReadLine(), out int minutes) || minutes <= 0)
            {
                Console.WriteLine("Invalid input. Please enter a positive integer.");
                return;
            }

            Console.WriteLine("Enter the delay duration in seconds :");
            if (!int.TryParse(Console.ReadLine(), out int sleepDuration) || sleepDuration <= 0)
            {
                Console.WriteLine("Invalid input. Please enter a positive integer.");
                return;
            }

            sleepDuration = sleepDuration * 1000;

            var duration = TimeSpan.FromMinutes(minutes);
            var stopwatch = Stopwatch.StartNew();

            Console.WriteLine("Simulating mouse click at the current position...");

            while (stopwatch.Elapsed < duration)
            {
                SimulateMouseClickAtCurrentPosition();
                Thread.Sleep(sleepDuration);
            }

        }
    }
}