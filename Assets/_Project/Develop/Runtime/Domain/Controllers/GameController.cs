using _Project.Develop.Runtime.Core.Enums;
using _Project.Develop.Runtime.Core.Services;
using _Project.Develop.Runtime.Core.Signals;
using _Project.Develop.Runtime.Data.Configs;
using _Project.Develop.Runtime.Domain.Models;
using _Project.Develop.Runtime.Presentation.UI.Controllers;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using Zenject;
using R3;

namespace _Project.Develop.Runtime.Domain.Controllers
{
    public class GameController : MonoBehaviour
    {
        private IDisposable _currentVoiceSequence;
        private CompositeDisposable _timerDisposable = new CompositeDisposable();

        private GameModel _model;
        private TimeService _timeService;
        private AudioSource _backgroundVoice;
        private GameObject _sceneLightning;
        private UIController _uiController;
        private SignalBus _signalBus;

        [Inject]
        private void Construct
        (
            GameModel gameModel,
            TimeService timeService,
            AudioSource backgroundVoice,
            GameObject sceneLightning,
            UIController uiController,
            SignalBus signalBus
        )
        {
            _model = gameModel;
            _timeService = timeService;
            _backgroundVoice = backgroundVoice;
            _sceneLightning = sceneLightning;
            _uiController = uiController;
            _signalBus = signalBus;
        }

        private void Awake()
        {
            _signalBus.Subscribe<OnHeadDamagedSignal>(RegisterDamage);
            _signalBus.Subscribe<OnRestartGameSignal>(RestartGame);
            _signalBus.Subscribe<OnQuitGameSignal>(QuitGame);
        }

        private void Start()
        {
            _uiController.ShowScreen(_model.CurrentGameState);
            PlayStageVoice(_model.CurrentGameState, StartCombat);
        }

        private void PlayStageVoice(GameState state, Action onComplete = null)
        {
            var stage = _model.GetStageAudio(state);
            PlayVoiceWithSubtitles(stage, onComplete);
        }

        private void PlayVoiceWithSubtitles(StageAudio stage, Action onComplete = null)
        {
            _currentVoiceSequence?.Dispose();

            _backgroundVoice.clip = stage.Clip;
            _backgroundVoice.Play();

            _uiController.ShowSubtitles(stage.State, stage.Subtitles);

            var duration = stage.Clip.length;

            _currentVoiceSequence = Observable
                .Timer(TimeSpan.FromSeconds(duration))
                .Subscribe(_ =>
                {
                    onComplete?.Invoke();
                });
        }

        private void StartCombat()
        {
            _model.CurrentGameState = GameState.InCombat;
            _uiController.ShowScreen(_model.CurrentGameState);

            _timeService.StartTimer(_model.GetGameTime());
            _timeService.TimeLeft
                .Where(t => t >= 0)
                .Subscribe(t => UpdateTimer(t))
                .AddTo(_timerDisposable);
            _timeService.OnTimerEnd
                .Subscribe(_ => CheckWinConditions())
                .AddTo(_timerDisposable);
        }

        private void RegisterDamage(OnHeadDamagedSignal damagedSignal)
        {
            var damage = damagedSignal.damage;
            _model.AddDamage(damage);
            _uiController.SetGameProgress(_model.GetGameProgress());
        }

        private void UpdateTimer(float timeRemaining)
        {
            _uiController.SetGameTimeRemaining(timeRemaining);
        }

        private void CheckWinConditions()
        {
            _timerDisposable?.Dispose();

            if (_model.IsGameWon()) Win();
            else Lose();
        }

        private void Win()
        {
            _model.CurrentGameState = GameState.Victory;
            EndGame();
        }

        private void Lose()
        {
            _model.CurrentGameState = GameState.Defeat;
            EndGame();
        }

        private void EndGame()
        {
            var stage = _model.GetStageAudio(_model.CurrentGameState);
            PlayStageVoice(_model.CurrentGameState);
            _uiController.ShowScreen(_model.CurrentGameState);

            _sceneLightning.SetActive(false);

            _signalBus.Fire<OnEndGameSignal>();
        }

        private void RestartGame()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        private void QuitGame()
        {
            Application.Quit();
        }
    }
}