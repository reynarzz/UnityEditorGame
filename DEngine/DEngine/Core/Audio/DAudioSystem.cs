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

    public class DAudioSystem : DEngineSystemBase
    {
        private Dictionary<string, PlayingAudio> _audiosByName;
        private Dictionary<string, DAudioFile> _audios;

        private string AudioBasePath = Path.Combine(Application.dataPath, "../DAssets/Audio");
        private Dictionary<string, WaveOutEvent> _playingAudios;

        public DAudioSystem()
        {
            _audiosByName = new Dictionary<string, PlayingAudio>();
            _playingAudios = new Dictionary<string, WaveOutEvent>();

            _audios = new Dictionary<string, DAudioFile>();

            LoadAudios();
        }

        private void LoadAudios()
        {
            var background = new WaveFileReader(AudioBasePath + "/ForgottenPlains/Music/Fair_Fight_(Battle).wav");
            var sewers = new WaveFileReader(AudioBasePath + "/JDSherbert/JDSherbert - Ambiences Music Pack - Bloodrat Sewers.wav");
            var battle = new WaveFileReader(AudioBasePath + "/Minifantasy_Dungeon_Music/Music/Goblins_Dance_(Battle).wav");



            var enemyHit = new WaveFileReader(AudioBasePath + "/ForgottenPlains/Fx/16_Hit_on_brick_1.wav");
            var shoot = new WaveFileReader(AudioBasePath + "/ForgottenPlains/Fx/06_step_stone_1.wav");
            var step1 = new AudioFileReader(AudioBasePath + "/ForgottenPlains/Fx/05_step_dirt_1.wav");
            var step2 = new AudioFileReader(AudioBasePath + "/ForgottenPlains/Fx/05_step_dirt_2.wav");
            var step3 = new AudioFileReader(AudioBasePath + "/ForgottenPlains/Fx/05_step_dirt_3.wav");
            var chestOpen = new AudioFileReader(AudioBasePath + "/Fx/01_chest_open_3.wav");
            var potionTaken = new AudioFileReader(AudioBasePath + "/Fx/08_human_charge_1.wav");
            var doorEnter = new AudioFileReader(AudioBasePath + "/Fx/27_sword_miss_3.wav");
            var orcHit = new AudioFileReader(AudioBasePath + "/Fx/25_orc_walk_stone_3.wav");
            var playerDamage = new AudioFileReader(AudioBasePath + "/Fx/11_human_damage_1.wav");
            var playerDead = new AudioFileReader(AudioBasePath + "/Fx/10_human_special_atk_2.wav");
            var doorOpen = new AudioFileReader(AudioBasePath + "/Fx/01_chest_open_1.wav");



            // Music
            _audios.Add("Background", new DAudioFile(new LoopStream(background)) { Latency = 300, BufferCount = 2 });
            _audios.Add("Sewers", new DAudioFile(new LoopStream(sewers)) { Latency = 300, BufferCount = 2 });
            _audios.Add("Battle", new DAudioFile(new LoopStream(battle)) { Latency = 300, BufferCount = 2 });

            // FX
            _audios.Add("EnemyHit", new DAudioFile(enemyHit));
            _audios.Add("OrcHit", new DAudioFile(orcHit));
            _audios.Add("Shoot", new DAudioFile(shoot));

            _audios.Add("Step1", new DAudioFile(step1) { Volume = 0.3f });
            _audios.Add("Step2", new DAudioFile(step2) { Volume = 0.3f });
            _audios.Add("Step3", new DAudioFile(step3) { Volume = 0.3f });
            _audios.Add("ChestOpen", new DAudioFile(chestOpen));
            _audios.Add("PotionTaken", new DAudioFile(potionTaken));
            _audios.Add("DoorEnter", new DAudioFile(doorEnter));
            _audios.Add("PlayerDamage", new DAudioFile(playerDamage));
            _audios.Add("PlayerDead", new DAudioFile(playerDead));
            _audios.Add("DoorOpen", new DAudioFile(doorOpen));
        }

        public void Play(string audio)
        {
            if (!string.IsNullOrEmpty(audio))
            {
                if (_audios.TryGetValue(audio, out var audioFile))
                {
                    var playEvent = default(WaveOutEvent);

                    if (_playingAudios.TryGetValue(audio, out var device))
                    {
                        playEvent = device;
                        audioFile.Sample.Position = 0;
                        //--playEvent.Volume = audioFile.Volume;

                        if (playEvent.PlaybackState != PlaybackState.Playing)
                        {
                            playEvent.Play();
                        }
                    }
                    else
                    {
                        playEvent = new WaveOutEvent();
                        //--playEvent.Volume = audioFile.Volume;
                        playEvent.DesiredLatency = audioFile.Latency;
                        playEvent.Init(audioFile.Sample);
                        _playingAudios.Add(audio, playEvent);
                        playEvent.Play();
                    }
                }
            }
            else
            {
                Debug.LogError("Empty audio name!");
            }
        }

        public void PlayLoop()
        {

        }

        public override void Update()
        {

        }

        public void StopAudio(string audio)
        {
            if (!string.IsNullOrEmpty(audio))
            {
                if (_audios.TryGetValue(audio, out var audioFile))
                {
                    if (_playingAudios.TryGetValue(audio, out var device))
                    {
                        audioFile.Sample.Position = 0;
                        device.Stop();
                    }
                }
            }
            else
            {
                Debug.Log("Empty audio name!");
            }
        }

        public void SetVolume(string audio, float amount)
        {

        }

        public override void Cleanup()
        {
            //UnityEngine.Debug.Log("Audio cleanup");

            foreach (var audio in _playingAudios.Values)
            {

                audio.Stop();
                audio.Dispose();
            }

            _playingAudios.Clear();
        }

        public override void OnPause()
        {
            foreach (var audio in _playingAudios.Values)
            {
                audio.Pause();
            }
        }

        public override void OnResume()
        {
            foreach (var audio in _playingAudios.Values)
            {
                audio.Play();
            }
        }
    }
}
