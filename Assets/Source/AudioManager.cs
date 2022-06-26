using UnityEngine.Audio;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RubeGoldbergGame
{
    public class AudioManager : MonoBehaviour
    {
        // DATA //
        // Sounds
        public Sound[] sounds;

        // Singleton Pattern
        public static AudioManager instance;


        // FUNCTIONS //
        // Unity Defaults
        void Awake()
        {
            // Prevent duplication of AudioManager
            if (instance == null)
                instance = this;
            else
            {
                Destroy(gameObject);
                return;
            }

            // Keep AudioManager in all scenes
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;

            // Assign sound characteristics to each source
            foreach (Sound s in sounds)
            {
                s.source = gameObject.AddComponent<AudioSource>();
                s.source.clip = s.clip;

                s.source.volume = s.volume;
                s.source.pitch = s.pitch;
                s.source.loop = s.loop;

                // Set the source's audio mixer and update the group used to the one specified in the sound.
                s.source.outputAudioMixerGroup = s.audioMixGroup;
            }
        }

        // Called on every scene change
        void OnSceneLoaded(Scene scene, LoadSceneMode sceneMode)
        {
            switch (scene.name)
            {
                case "MainMenuScene":
                    Stop("Level");
                    Play("Theme");
                    break;
                case "SampleLevel":
                    Stop("Theme");
                    Play("Level");
                    break;
            }
        }

        public void Play(string name)
        {
            Sound s = Array.Find(sounds, sound => sound.name == name);
            if (s == null)
            {
                Debug.LogWarning("Sound: " + name + " not found!");
                return;
            }

            // Play sound
            s.source.Play();
        }

        public void Stop(string name)
        {
            Sound s = Array.Find(sounds, item => item.name == name);
            if (s == null)
            {
                Debug.LogWarning("Sound " + name + " not found!");
                return;
            }

            // Stop sound
            s.source.Stop();
        }
    }
}