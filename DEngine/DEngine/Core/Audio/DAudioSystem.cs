using NAudio.Wave.SampleProviders;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.IO;
using UnityEngine;

namespace DungeonInspector
{
    internal class PlayingAudio
    {
        public AudioFileReader Audio { get; set; }
        public WaveOutEvent Event { get; set; }
    }

    public class DAudioSystem : EngineSystemBase
    {
        private static Dictionary<string, PlayingAudio> _audiosByName;

        public DAudioSystem()
        {
            if(_audiosByName == null)
            _audiosByName = new Dictionary<string, PlayingAudio>();
        }

        public void Play(string audio)
        {
            var path = Path.Combine(Application.dataPath, "../DAssets/", audio);

            var cleanName = Path.GetFileNameWithoutExtension(audio);

            var audioReader = new AudioFileReader(path);
            var outputDevice = new WaveOutEvent(); 

            if (_audiosByName.ContainsKey(cleanName))
            {
                _audiosByName[cleanName].Event.Dispose();
                _audiosByName[cleanName].Audio.Dispose();

                _audiosByName[cleanName].Audio = audioReader;
                _audiosByName[cleanName].Event = outputDevice;
            }
            else
            {
                _audiosByName.Add(cleanName, new PlayingAudio()
                {
                    Audio = audioReader,
                    Event = outputDevice
                });
            }

            outputDevice.Init(audioReader);
            outputDevice.Play();
        }

        public void PlayLoop()
        {

        }

        public override void Update()
        {

        }

        public void StopAudio(string audio)
        {

        }

        public void SetVolume(string audio, float amount)
        {

        }

        public override void Cleanup()
        {
            UnityEngine.Debug.Log("called");

            foreach (var audio in _audiosByName.Values)
            {
                UnityEngine.Debug.Log("audio cleanup");

                audio.Event.Stop();
                audio.Audio.Dispose();
                audio.Event.Dispose();
            }
        }
    }
}
