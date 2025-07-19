using UnityEngine;

namespace _Project.Develop.Runtime.Data.Configs
{
    [CreateAssetMenu(menuName = "Configs/Game Config")]
    public class GameConfig : ScriptableObject
    {
        [Header("Game Settings")]
        [SerializeField] private int _winDamage = 10000;
        [SerializeField] private float _gameTime = 10f;

        [Header("Gamage Settings")]
        [SerializeField] private float _minForceToDamage = 5f;
        [SerializeField] private float _damagePerStage = 20f;

        [Header("Slow-mo Settings")]
        [SerializeField] private int _hitsBeforeSlowmoMin = 3;
        [SerializeField] private int _hitsBeforeSlowmoMax = 5;
        [SerializeField] private float _slowmoScale = 0.2f;
        [SerializeField] private float _slowmoDuration = 1.5f;
        [SerializeField] private float _slowmoReturnDuration = 0.5f;
        [SerializeField] private float _slowmoFadeInDuration = 0.3f;
        [SerializeField] private float _slowmoFadeOutDuration = 0.5f;

        public int WinDamage => _winDamage;
        public float GameTime => _gameTime;
        public float MinForceToDamage => _minForceToDamage;
        public float DamagePerStage => _damagePerStage;
        public int HitsBeforeSlowmoMin => _hitsBeforeSlowmoMin;
        public int HitsBeforeSlowmoMax => _hitsBeforeSlowmoMax;
        public float SlowmoScale => _slowmoScale;
        public float SlowmoDuration => _slowmoDuration;
        public float SlowmoReturnDuration => _slowmoReturnDuration;
        public float SlowmoFadeInDuration => _slowmoFadeInDuration;
        public float SlowmoFadeOutDuration => _slowmoFadeOutDuration;
    }
}