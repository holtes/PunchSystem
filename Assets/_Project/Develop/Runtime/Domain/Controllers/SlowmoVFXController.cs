using _Project.Develop.Runtime.Core.Services;
using _Project.Develop.Runtime.Domain.Models;
using UnityEngine;
using UnityEngine.Rendering;
using DG.Tweening;
using R3;
using Zenject;

namespace _Project.Develop.Runtime.Domain.Controllers
{
    [RequireComponent(typeof(Volume))]
    public class SlowmoVFXController : MonoBehaviour
    {
        [SerializeField] private Volume _volume;

        private Tween _fxTween;

        private SlowmoVFXModel _model;
        private SlowmoService _slowmoService;


        [Inject]
        private void Construct(SlowmoVFXModel slowmoVFXModel, SlowmoService slowmoService)
        {
            _model = slowmoVFXModel;
            _slowmoService = slowmoService;
        }

        private void Awake()
        {
            _volume.weight = 0f;

            _slowmoService.OnSlowmoStarted
                .Subscribe(_ => EnableFX())
                .AddTo(this);

            _slowmoService.OnSlowmoEnded
                .Subscribe(_ => DisableFX())
                .AddTo(this);
        }

        private void EnableFX()
        {
            _fxTween?.Kill();
            _fxTween = DOTween.To(() => _volume.weight,
                val => _volume.weight = val,
                1f,
                _model.GetFadeInDuration());
        }

        private void DisableFX()
        {
            _fxTween?.Kill();
            _fxTween = DOTween.To(() => _volume.weight,
                val => _volume.weight = val,
                0f,
                _model.GetFadeOutDuration());
        }
    }
}