using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonInspector
{
    public class DTime : DEngineSystemBase
    {
        private Stopwatch _stopWatch;
        private static float _time = 0f;
        private static float _dt = 0f;
        private float _prev = 0f;

        public static float Time => _time;
        public static float DeltaTime => _dt;
        public static float TimeScale { get; set; } = 1;

        private static int _fpsCount;
        private static int _fps;
        private float _timeToFPS;
        public static int FPs => _fps;
        public static float DeltaTimeUnscaled { get; private set; }
        public static float FixedUpdateInterval { get; set; } = 0.02f;

        public DTime()
        {
            _stopWatch = new Stopwatch();
            _stopWatch.Start();
            _prev = _stopWatch.ElapsedMilliseconds / 1000f;
        }

        public override void OnPause()
        {
            _stopWatch.Stop();
        }

        public override void OnResume()
        {
            _stopWatch.Start();
        }

        public override void Update()
        {
            var secElapsep = _stopWatch.ElapsedMilliseconds / 1000f;

            _dt = (secElapsep - _prev);

            DeltaTimeUnscaled = _dt;
            _dt *= TimeScale;
            


            _time += _dt;
            _prev = secElapsep;

            _timeToFPS += DeltaTimeUnscaled;

            if(_timeToFPS >= 1)
            {
                _timeToFPS = 0;
                _fps = _fpsCount;
                _fpsCount = 0;
            }
            else
            {
                _fpsCount++;
            }
        }

    }
}
