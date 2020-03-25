using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace BFPoc.Util
{
    public sealed class CodeTimer : IDisposable
    {
        private readonly DateTime _start;
        private DateTime _stop;
        private readonly Stopwatch _stopwatch;

        public string Caller { get; }

        public CodeTimer([CallerMemberName]string caller = "")
        {
            Caller = caller;
            _start = DateTime.Now;
            _stopwatch = Stopwatch.StartNew();
        }

        public void Stop()
        {
            _stop = DateTime.Now;
            _stopwatch.Stop();
        }

        public override string ToString()
        {
            return
                $"[{Caller}] Start: {_start}, Stop: {_stop}, OverallTime: {(double) _stopwatch.ElapsedMilliseconds / 1000:#.000}s";
        }

        public DateTime StartTime => _start;
        public DateTime EndTime => _stop;

        public void Dispose()
        {
            _stopwatch.Stop();
        }
    }
}