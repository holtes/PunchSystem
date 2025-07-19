using _Project.Develop.Runtime.Data.Configs;

namespace _Project.Develop.Runtime.Domain.Models
{
    public class SlowmoVFXModel
    {
        private readonly float _fadeInDuration;
        private readonly float _fadeOutDuration;

        public SlowmoVFXModel(GameConfig gameConfig)
        {
            _fadeInDuration = gameConfig.SlowmoFadeInDuration;
            _fadeOutDuration = gameConfig.SlowmoFadeOutDuration;
        }

        public float GetFadeInDuration()
        {
            return _fadeInDuration;
        }

        public float GetFadeOutDuration()
        {
            return _fadeOutDuration;
        }
    }
}