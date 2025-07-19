using _Project.Develop.Runtime.Core.Services;
using UnityEngine;
using Zenject;

namespace _Project.Develop.Runtime.Presentation.Head
{
    public class HeadView : MonoBehaviour
    {
        [SerializeField] private Rigidbody _headRb;
        [SerializeField] private Transform _headTransform;

        [Header("Audio effects")]
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private AudioClip[] _hitSounds;
        [SerializeField] private AudioClip _boneCrackClip;

        [Header("VFX")]
        [SerializeField] private SkinnedMeshRenderer _headRenderer;
        [SerializeField] Material[] _damageStages;
        [SerializeField] private GameObject _bloodEffectPrefab;
        [SerializeField] private Transform _bloodParent;
        [SerializeField] private GameObject[] _decalPrefabs;

        [Header("Head Movements")]
        [SerializeField] private Transform _leftEye;
        [SerializeField] private Transform _rightEye;
        [SerializeField] private float _shakeFrequency;
        [SerializeField] private float _shakeAmplitude;
        [SerializeField] private float _eyeLookSpeed;
        [SerializeField] private float _maxEyeAngle;

        private float _nextShakeTime = 0f;

        private TimeService _timeService;

        [Inject]
        private void Construct(TimeService timeService)
        {
            _timeService = timeService;
        }

        public void ApplayHitForce(Vector3 hitForce, Vector3 hitPosition)
        {
            _headRb.AddForceAtPosition(hitForce, hitPosition, ForceMode.Impulse);
        }

        public void UpdateDamageVisual(int stage)
        {
            stage = Mathf.Clamp(stage, 0, _damageStages.Length - 1);
            _headRenderer.material = _damageStages[stage];
        }

        public void SpawnBloodFX(Vector3 position, float force)
        {
            if (!_bloodEffectPrefab) return;

            var fx = Instantiate(_bloodEffectPrefab, position, Quaternion.identity, _bloodParent);
            var main = fx.GetComponent<ParticleSystem>().main;
            main.startSpeed = Mathf.Lerp(1, 10, force / 50f);
        }

        public void SpawnDecal(Vector3 position, Vector3 normal)
        {
            if (_decalPrefabs.Length == 0) return;
            var prefab = _decalPrefabs[Random.Range(0, _decalPrefabs.Length)];
            var rotation = Quaternion.LookRotation(normal);
            var decal = Instantiate(prefab, position + normal * 0.01f, rotation, _bloodParent);
            Destroy(decal, 5f);
        }

        public void PlayHitSound()
        {
            if (_hitSounds.Length == 0) return;
            var index = Random.Range(0, _hitSounds.Length);
            _audioSource.PlayOneShot(_hitSounds[index]);
        }

        public void PlayBoneCrackSound()
        {
            if (_boneCrackClip == null) return;
            _audioSource.PlayOneShot(_boneCrackClip);
        }

        public void UpdateIdle()
        {
            if (_timeService.GameTime < _nextShakeTime)
                return;

            _nextShakeTime = _timeService.GameTime + Random.Range(0.1f, 0.3f);

            var shakeMagnitude = Mathf.Sin(_timeService.GameTime * _shakeFrequency) * _shakeAmplitude;

            var randomDirection = new Vector3(
                Random.Range(-1f, 1f),
                Random.Range(-1f, 1f),
                Random.Range(-0.5f, 0.5f)
            ).normalized;

            var shakeForce = randomDirection * shakeMagnitude;

            _headRb.AddForce(shakeForce, ForceMode.Impulse);
        }

        public void LookAt(Transform target)
        {
            RotateEyeToTarget(_leftEye, target.position);
            RotateEyeToTarget(_rightEye, target.position);
        }

        private void RotateEyeToTarget(Transform eye, Vector3 targetPos)
        {
            var dirToTarget = targetPos - eye.position;
            var lookRot = Quaternion.LookRotation(dirToTarget);
            var limitedRot = Quaternion.RotateTowards(eye.rotation, lookRot, _eyeLookSpeed * _timeService.DeltaTime);

            var angle = Quaternion.Angle(Quaternion.LookRotation(eye.forward), lookRot);
            if (angle <= _maxEyeAngle) eye.rotation = limitedRot;
        }
    }
}