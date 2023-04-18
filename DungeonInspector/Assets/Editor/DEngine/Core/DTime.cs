using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonInspector
{
    public class DTime : IDService
    {
        private Stopwatch _stopWatch;
        private static float _time = 0f;
        private static float _dt = 0f;
        private float _prev = 0f;

        public static float Time => _time;
        public static float DeltaTime => _dt;
        public static float TimeScale { get; set; } = 1;

        public void Init()
        {
            _stopWatch = new Stopwatch();
            _stopWatch.Start();
            _prev = _stopWatch.ElapsedMilliseconds / 1000f;
        }

        public void Update()
        {
            var secElapsep = _stopWatch.ElapsedMilliseconds / 1000f;

            _dt = (secElapsep - _prev) * TimeScale;
            _time += _dt;
            _prev = secElapsep;
        }

    }
}
