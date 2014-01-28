using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace CStrawberry3D.Core
{
    public class Clock:IDisposable
    {
        //TODO
        public float Delta { get; private set; }
        public int Limit { get; private set; }
        Stopwatch _watch;
        public Clock()
        {
            Delta = 0;
            Limit = 60;
            _watch = new Stopwatch();
        }
        public void Start()
        {
            _watch.Restart();
        }
        public void Stop()
        {
            _watch.Reset();
        }
        public void Tick()
        {
            Delta = _watch.ElapsedMilliseconds * 0.001f;
            _watch.Restart();
        }
        public void Dispose()
        {
            Stop();
        }
    }
}
