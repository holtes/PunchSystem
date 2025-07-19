using _Project.Develop.Runtime.Core.Enums;
using UnityEngine;

namespace _Project.Develop.Runtime.Data.Configs
{
    [System.Serializable]
    public class StageAudio
    {
        public GameState State;
        public AudioClip Clip;
        [TextArea(2, 6)]
        public string Subtitles;
    }
}