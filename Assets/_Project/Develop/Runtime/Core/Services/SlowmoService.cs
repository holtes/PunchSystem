using System;
using R3;
using DG.Tweening;

namespace _Project.Develop.Runtime.Core.Services
{
    public class SlowmoService
    {
        public Observable<Unit> OnSlowmoStarted => _onSlowmoStarted;
        public Observable<Unit> OnSlowmoEnded => _onSlowmoEnded;

        public bool IsPlaying = false;

        private readonly Subject<Unit> _onSlowmoStarted = new();
        private readonly Subject<Unit> _onSlowmoEnded = new();

        private IDisposable _slowmoDisposable;
        private Tween _returnTween;

        private TimeService _timeService;
        private float _slowmoReturnDuration;

        public SlowmoService(TimeService timeService, float slowmoReturnDuration)
        {
            _timeService = timeService;
            _slowmoReturnDuration = slowmoReturnDuration;
        }

        public void TriggerSlowmo(float scale, float duration)
        {
            _slowmoDisposable?.Dispose();
            _returnTween?.Kill();

            StartSlowmo(scale);

            _slowmoDisposable = Observable.Timer(TimeSpan.FromSeconds(duration))
                .Subscribe(_ => EndSlowmo());
        }

        private void StartSlowmo(float scale)
        {
            IsPlaying = true;
            _timeService.SetTimeScale(scale);
            _onSlowmoStarted.OnNext(Unit.Default);
        }

        private void EndSlowmo()
        {
            _returnTween = DOTween.To(
                () => _timeService.TimeScale,
                newValue => _timeService.SetTimeScale(newValue),
                1f,
                _slowmoReturnDuration
            ).SetEase(Ease.OutQuad)
             .OnComplete(() =>
             {
                 IsPlaying = false;
                 _timeService.ResetTimeScale();
                 _onSlowmoEnded.OnNext(Unit.Default);
             });
        }
    }
}