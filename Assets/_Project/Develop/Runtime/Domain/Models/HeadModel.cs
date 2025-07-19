using _Project.Develop.Runtime.Data.Configs;
using UnityEngine;

namespace _Project.Develop.Runtime.Domain.Models
{
    public class HeadModel
    {
        private readonly float _minForceToDamage;
        private readonly float _damagePerStage;
        private readonly float _slowmoScale;
        private readonly float _slowmoDuration;
        private readonly int _hitsBeforeSlowmoMin;
        private readonly int _hitsBeforeSlowmoMax;

        public Transform TargetToLookAt { get; private set; }
        public int CurrentDamageStage { get; private set; } = 0;

        private float _accumulatedDamage = 0;

        private int _hitCount;
        private int _hitsToNextSlowmo;

        public HeadModel(GameConfig config, Transform playerTransform)
        {
            _minForceToDamage = config.MinForceToDamage;
            _damagePerStage = config.DamagePerStage;
            _slowmoScale = config.SlowmoScale;
            _slowmoDuration = config.SlowmoDuration;
            _hitsBeforeSlowmoMin = config.HitsBeforeSlowmoMin;
            _hitsBeforeSlowmoMax = config.HitsBeforeSlowmoMax;

            TargetToLookAt = playerTransform;

            ResetSlowmoCounter();
        }

        public void SetLookAtTarget(Transform target)
        {
            TargetToLookAt = target;
        }

        public Vector3 GetHitForce(float impulse, Vector3 forceDir)
        {
            return forceDir * impulse;
        }

        public bool TryApplyDamage(float impulse)
        {
            _accumulatedDamage += impulse;

            var newStage = Mathf.FloorToInt(_accumulatedDamage / _damagePerStage);
            if (newStage > CurrentDamageStage)
            {
                CurrentDamageStage = newStage;
                return true;
            }
            return false;
        }

        public bool IsHitForceEnough(float impulse)
        {
            return impulse > _minForceToDamage;
        }

        public void RegisterHit()
        {
            _hitCount++;
        }

        public bool IsSlowmoReady()
        {
            return _hitCount >= _hitsToNextSlowmo;
        }

        public void ResetSlowmoCounter()
        {
            _hitCount = 0;
            _hitsToNextSlowmo = Random.Range(_hitsBeforeSlowmoMin, _hitsBeforeSlowmoMax + 1);
        }

        public float GetSlowmoScale()
        {
            return _slowmoScale;
        }

        public float GetSlowmoDuration()
        {
            return _slowmoDuration;
        }
    }
}