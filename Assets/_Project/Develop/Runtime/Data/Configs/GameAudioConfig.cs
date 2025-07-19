using UnityEngine;

namespace _Project.Develop.Runtime.Data.Configs
{
    [CreateAssetMenu(fileName = "GameAudioConfig", menuName = "Configs/Game Audio Config")]
    public class GameAudioConfig : ScriptableObject
    {
        [SerializeField] private StageAudio[] _stages;

        public StageAudio[] Stages => _stages;
    }
}