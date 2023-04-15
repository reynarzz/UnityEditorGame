using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonInspector
{
    public class DTime
    {
        private Stopwatch _stopWatch;
        private static float _time = 0f;
        private static float _dt = 0f;
        private float _prev = 0f;

        public static float TimeSinceStarted => _time;
        public static float DeltaTime => _dt;

        public DTime()
        {
            _stopWatch = new Stopwatch();
            _stopWatch.Start();
            _prev = _stopWatch.ElapsedMilliseconds / 1000f;

        }

        public void Update()
        {
            var secElapsep = _stopWatch.ElapsedMilliseconds / 1000f;

            _dt = secElapsep - _prev;
            _time += _dt;
            _prev = secElapsep;
        }
    }
}
