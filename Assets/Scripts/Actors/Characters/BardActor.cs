using System.Collections;
using System.Collections.Generic;
using OmniGlyph.Actors;
using UnityEngine;

namespace OmniGlyph.Actors.Characters {
    public class BardActor : BotActor {
        private AudioSource _audioSource;

        private AudioClip[] _explorationMusic = new AudioClip[] { };
        private AudioClip[] _combatMusic = new AudioClip[] { };

        protected override void Start() {
            base.Start();
            AudioSource audio = gameObject.GetComponent<AudioSource>();
            if (audio == null) {
                SetupAudioSource();
            } else {
                _audioSource = audio;
            }
        }
        public override void Init() {
            base.Init();
            LoadMusic();
            Context.GameStateChanged += OnGameStateChange;
            Debugger.Log("Braggi Initialized");
        }
        private void SetupAudioSource() {
            _audioSource = gameObject.AddComponent<AudioSource>();
            _audioSource.volume = 1f;
            _audioSource.spatialBlend = 1f;

            _audioSource.rolloffMode = AudioRolloffMode.Logarithmic;
            _audioSource.minDistance = 7.5f;
            _audioSource.maxDistance = 32f;
            _audioSource.spread = 60f;
        }
        private void LoadMusic() {
            string pathPrefx = "Audio/Music/";
            _explorationMusic = Resources.LoadAll<AudioClip>($"{pathPrefx}Exploration");
            _combatMusic = Resources.LoadAll<AudioClip>($"{pathPrefx}Combat");
        }
        private void PlayMusic() {
            if (Context == null) {
                return;
            }
            if (_audioSource.isPlaying) {
                return;
            }
            switch (Context.CurrentGameState) {
                case GameStates.Roam:
                    _audioSource.clip = PickClip(_explorationMusic);
                    break;
                case GameStates.Combat:
                    _audioSource.clip = PickClip(_combatMusic);
                    break;
                default:
                    break;
            }
            if (_audioSource.clip != null) {
                _audioSource.Play();
            }
        }
        private AudioClip PickClip(AudioClip[] clips) {
            if (clips.Length == 0) {
                return null;
            }

            return clips[Random.Range(0, clips.Length)];
        }

        private void StopMusic() {
            if (_audioSource.isPlaying)
                _audioSource.Stop();
        }
        private void OnGameStateChange(GameStates newState) {
            StopMusic();
            PlayMusic();
        }
        void Update() {
            if (!_isFollowingPlayer) {
                StartFollowingPlayer();
            }
            PlayMusic();
        }


    }
}
