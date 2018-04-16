using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WindowsInput;
using WindowsInput.Native;
using XInputDotNetPure;

namespace GamepadMapper
{
    class Program
    {
        async Task InputLoop(double milliseconds, CancellationToken cancellation)
        {
            var delay = TimeSpan.FromMilliseconds(milliseconds);
            var tDelta = milliseconds;
            while (cancellation.IsCancellationRequested)
            {
                var lastFrame = DateTime.Now;
                await Task.Delay(delay, cancellation);
                tDelta = (DateTime.Now - lastFrame).Milliseconds;
            }
        }
    }
}
