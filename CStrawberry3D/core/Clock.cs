using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace CStrawberry3D.Core
{
    public class Clock:IDisposable
    {
        public static Clock Create()
        {
            return new Clock();
        }
        public int PeriodFps { get; set; }
        public int Fps { get; private set; }
        public float Delta { get; private set; }
        public int Limit { get; private set; }
        int _currFps;
        float _currPeriod;
        Stopwatch _watch;
        Clock()
        {
            Delta = 0;
            Limit = 60;
            _currFps = 0;
            PeriodFps = 100;
            Fps = 0;
            _currPeriod = 0;
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
            while (true)
            {
                Delta = _watch.ElapsedMilliseconds * 0.001f;
                if (Delta > 1.0f / Limit)
                {
                    break;
                }
            }
            _currFps++;
            _currPeriod += Delta;
            if (_currFps > PeriodFps)
            {
                Fps = (int)(_currFps / _currPeriod);
            }
            _watch.Restart();
        }
        public void Dispose()
        {
            Stop();
        }
    }
}
