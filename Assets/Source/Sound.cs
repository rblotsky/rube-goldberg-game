using UnityEngine.Audio;
using UnityEngine;

namespace RubeGoldbergGame
{
    [System.Serializable]
    public class Sound
    {
        // DATA //
        // Basic Data
        public string name;
        [Range(0f, 1f)]
        public float volume;
        [Range(.1f, 3f)]
        public float pitch;
        public bool loop;

        // Audio Mixer
        public AudioMixerGroup audioMixGroup;

        // References
        public AudioClip clip;

        // Cached Data
        [HideInInspector]
        public AudioSource source;
    }
}