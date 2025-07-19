using UnityEngine;
using System;
using R3;

namespace _Project.Develop.Runtime.Core.Services
{
    public class TimeService
    {
        public readonly float GameTime;
        public readonly float TimeScale;
        public readonly float DeltaTime;

        public ReactiveProperty<float> TimeLeft = new ReactiveProperty<float>();
        public Observable<Unit> OnTimerEnd => _onTimerEnd;

        private Subject<Unit> _onTimerEnd = new Subject<Unit>();
        private IDisposable _timerDisposable;

        public TimeService()
        {
            GameTime = Time.time;
            TimeScale = Time.timeScale;
            DeltaTime = Time.deltaTime;
        }

        public void SetTimeScale(float scale)
        {
            Time.timeScale = scale;
            Time.fixedDeltaTime = 0.02f;
        }

        public void ResetTimeScale()
        {
            SetTimeScale(1f);
        }

        public void StartTimer(float timerDuration)
        {
            TimeLeft.Value = timerDuration;
            _timerDisposable = Observable.EveryUpdate()
                .Subscribe(_ => UpdateTimer());
        }

        public void UpdateTimer()
        {
            TimeLeft.Value -= Time.unscaledDeltaTime;
            if (TimeLeft.Value <= 0) StopTimer();
        }

        public void StopTimer()
        {
            _onTimerEnd.OnNext(Unit.Default);
            _timerDisposable?.Dispose();
        }
    }
}