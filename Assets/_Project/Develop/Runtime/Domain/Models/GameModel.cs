using _Project.Develop.Runtime.Core.Enums;
using _Project.Develop.Runtime.Data.Configs;

namespace _Project.Develop.Runtime.Domain.Models
{
    public class GameModel
    {
        private readonly int _winDamage;
        private readonly float _gameTime;
        private readonly StageAudio[] _stages;

        public GameState CurrentGameState = GameState.Intro;

        private float _accumulatedDamage = 0;

        public GameModel(GameConfig gameConfig, GameAudioConfig gameAudioConfig)
        {
            _winDamage = gameConfig.WinDamage;
            _gameTime = gameConfig.GameTime;
            _stages = gameAudioConfig.Stages;
        }

        public void AddDamage(float amount)
        {
            _accumulatedDamage += amount;
        }

        public float GetGameProgress()
        {
            return _accumulatedDamage / _winDamage;
        }

        public bool IsGameWon()
        {
            return _accumulatedDamage >= _winDamage;
        }

        public float GetGameTime()
        {
            return _gameTime;
        }

        public StageAudio GetStageAudio(GameState state)
        {
            foreach (var stage in _stages)
            {
                if (stage.State == state)
                    return stage;
            }
            return null;
        }
    }
}