using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NAudio;
using NAudio.Wave;

namespace DungeonInspector
{
    public class DAudioFile : IDFile
    {
        public string Name { get; set; }
        public WaveStream Sample { get;  }
        public int Latency { get; set; } = 100;
        public int BufferCount { get; set; } = 15;
        public float Volume { get; set; } = 1.0f;

        public DAudioFile(WaveStream sample)
        {
            Sample = sample;
        }
    }
}
