using System;
using System.Collections;
using System.Collections.Generic;
using ntDev;
using UnityEngine;

namespace BaseGame
{
    public class ManagerAudio : MonoBehaviour
    {
        public static ManagerAudio Instance;
        public static SoundData Data;

        void Awake()
        {
            if (Instance != null) return;
            Instance = this;
            Data = soundData;
        }

        public List<AudioSource> listMusic = new List<AudioSource>();
        List<AudioSource> listSound = new List<AudioSource>();

        [SerializeField] SoundData soundData;

        public static void AddAudioSource(List<AudioSource> list, AudioClip audio, bool clear = false, bool loop = false)
        {
            if (clear)
                foreach (AudioSource source in list)
                    source.Stop();

            for (int i = 0; i < list.Count; ++i)
                if (!list[i].isPlaying)
                {
                    list[i].clip = audio;
                    list[i].Play();
                    list[i].loop = loop;
                    return;
                }

            GameObject obj = new GameObject("NewAudio");
            AudioSource newAudio = obj.AddComponent<AudioSource>();
            newAudio.transform.SetParent(Instance.transform);
            newAudio.transform.localPosition = Vector3.zero;

            newAudio.clip = audio;
            newAudio.Play();
            newAudio.loop = loop;
            list.Add(newAudio);
        }

        public static void PlayMusic(AudioClip audio, bool immediateChange = true, float vol = 1)
        {
            if (Instance == null) return;
            Instance.StopAllCoroutines();
            if (immediateChange) Instance.StartCoroutine(Instance.ChangeMusic(audio, vol));
            else Instance.StartCoroutine(Instance.ChangeMusicWithDelay(audio, vol));
        }

        IEnumerator ChangeMusicWithDelay(AudioClip audio, float vol)
        {
            float volume = SaveGame.Music ? vol : 0;
            while (volume > 0)
            {
                volume -= 0.01f;
                foreach (AudioSource source in Instance.listMusic)
                    source.volume = volume;
                yield return null;
            }
            StopMusic();
            if (audio != null)
            {
                AddAudioSource(Instance.listMusic, audio, true, true);
                foreach (AudioSource source in Instance.listMusic)
                    source.volume = volume;
                while (volume < (SaveGame.Music ? vol : -vol))
                    while (volume < vol)
                    {
                        volume = vol;
                        foreach (AudioSource source in Instance.listMusic)
                            source.volume = volume;
                        yield return null;
                    }
            }
        }

        IEnumerator ChangeMusic(AudioClip audio, float vol)
        {
            float volume = SaveGame.Music ? vol : 0;
            StopMusic();
            if (audio != null)
            {
                AddAudioSource(Instance.listMusic, audio, true, true);
                foreach (AudioSource source in Instance.listMusic)
                    source.volume = volume;
                while (volume < (SaveGame.Music ? vol : -vol))
                    while (volume < vol)
                    {
                        volume = vol;
                        foreach (AudioSource source in Instance.listMusic)
                            source.volume = volume;
                        yield return null;
                    }
            }
        }

        public static void PauseMusic()
        {
            if (Instance == null) return;
            foreach (AudioSource source in Instance.listMusic)
                source.Pause();
        }

        public static void ResumeMusic()
        {
            if (Instance == null) return;
            foreach (AudioSource source in Instance.listMusic)
                source.UnPause();
        }

        public static void StopMusic()
        {
            if (Instance == null) return;
            foreach (AudioSource source in Instance.listMusic)
                source.Stop();
        }

        public static void MuteMusic()
        {
            if (Instance == null) return;
            foreach (AudioSource source in Instance.listMusic)
                source.volume = 0;
        }

        public static void UnMuteMusic()
        {
            if (Instance == null) return;
            foreach (AudioSource source in Instance.listMusic)
                source.volume = 1;
        }

        public static void PlaySound(AudioClip audio, bool clear = false, int limit = 0)
        {
            if (Instance == null) return;
            if (audio == null) return;

            if (limit > 0)
            {
                int t = 0;
                foreach (AudioSource aS in Instance.listSound)
                    if (aS.isPlaying && aS.clip == audio)
                    {
                        ++t;
                        if (t == limit) return;
                    }
            }


            AddAudioSource(Instance.listSound, audio, clear);
            if (SaveGame.Sound)
                UnMuteSound();
            else MuteSound();
        }

        public static void PauseSound()
        {
            if (Instance == null) return;
            foreach (AudioSource source in Instance.listSound)
                source.Pause();
        }

        public static void ResumeSound()
        {
            if (Instance == null) return;
            foreach (AudioSource source in Instance.listSound)
                source.UnPause();
        }

        public static void StopSound()
        {
            if (Instance == null) return;
            foreach (AudioSource source in Instance.listSound)
                source.Stop();
        }

        public static void MuteSound()
        {
            if (Instance == null) return;
            foreach (AudioSource source in Instance.listSound)
                source.volume = 0;
        }

        public static void UnMuteSound()
        {
            if (Instance == null) return;
            foreach (AudioSource source in Instance.listSound)
                source.volume = 1;
        }
    }
}