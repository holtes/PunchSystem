using _Project.Develop.Runtime.Core.Services;
using _Project.Develop.Runtime.Core.Signals;
using _Project.Develop.Runtime.Domain.Models;
using _Project.Develop.Runtime.Presentation.Head;
using UnityEngine;
using Zenject;
using R3.Triggers;
using R3;

namespace _Project.Develop.Runtime.Domain.Controllers
{
    public class HeadController : MonoBehaviour
    {
        [SerializeField] private HeadView _view;
        [SerializeField] private CollisionDetectionService _collisionDetectionService;

        private HeadModel _model;
        private SlowmoService _slowmoService;
        private SignalBus _signalBus;

        [Inject]
        private void Construct
        (
            HeadModel headModel,
            SlowmoService slowmoService,
            SignalBus signalBus)
        {
            _model = headModel;
            _slowmoService = slowmoService;
            _signalBus = signalBus;
        }

        private void Awake()
        {
            _collisionDetectionService.OnCollisionDetected
                .Subscribe(collision => HitHead(collision))
                .AddTo(this);

            this.UpdateAsObservable()
                .Subscribe(_ => AddHeadMovements())
                .AddTo(this);
        }

        private void HitHead(Collision collision)
        {
            if (!collision.gameObject.CompareTag("Weapon")) return;

            var impulse = collision.relativeVelocity.magnitude * collision.rigidbody.mass * 0.2f;

            if (_model.IsHitForceEnough(impulse))
            {
                var hitPoint = collision.GetContact(0).point;
                var forceDir = -collision.GetContact(0).normal;
                var hitForce = _model.GetHitForce(impulse, forceDir);

                _view.ApplayHitForce(hitForce, hitPoint);

                ApplyVisualEffect(impulse, hitPoint, forceDir);

                if (!_slowmoService.IsPlaying)
                {
                    _model.RegisterHit();
                    if (_model.IsSlowmoReady()) TriggerSlowmoEffect();
                }

                _signalBus.Fire(new OnHeadDamagedSignal(impulse));
            }
        }

        private void AddHeadMovements()
        {
            _view.UpdateIdle();
            _view.LookAt(_model.TargetToLookAt);
        }

        private void ApplyVisualEffect(float impulse, Vector3 hitPoint, Vector3 forceDir)
        {
            if (_model.TryApplyDamage(impulse)) _view.UpdateDamageVisual(_model.CurrentDamageStage);

            _view.SpawnBloodFX(hitPoint, impulse);
            _view.SpawnDecal(hitPoint, forceDir);
            _view.PlayHitSound();
        }

        private void TriggerSlowmoEffect()
        {
            _slowmoService.TriggerSlowmo(_model.GetSlowmoScale(), _model.GetSlowmoDuration());
            _view.PlayBoneCrackSound();
            _model.ResetSlowmoCounter();
        }
    }
}