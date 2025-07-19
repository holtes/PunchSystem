using _Project.Develop.Runtime.Core.Services;
using _Project.Develop.Runtime.Core.Signals;
using _Project.Develop.Runtime.Data.Configs;
using _Project.Develop.Runtime.Domain.Models;
using _Project.Develop.Runtime.Presentation.UI.Controllers;
using UnityEngine;
using Zenject;

namespace _Project.Develop.Runtime.Bootstrap.Installers
{
    public class SceneInstaller : MonoInstaller
    {
        [SerializeField] private GameConfig _gameConfig;
        [SerializeField] private GameAudioConfig _gameAudioConfig;
        [SerializeField] private Transform _playerTransform;
        [SerializeField] private AudioSource _backgroundVoice;
        [SerializeField] private GameObject _sceneLightning;
        [SerializeField] private UIController _uiController;

        public override void InstallBindings()
        {
            BindSignalBus();
            BindSignals();
            BindTimeService();
            BindSlowmoParams();
            BindSlowmoService();
            BindGameModel();
            BindHeadModel();
            BindPlayerModel();
            BindSlowmoVFXModel();
            BindPlayerTransform();
            BindBackgroundVoice();
            BindSceneLightning();
            BindUIController();
        }

        private void BindSignalBus()
        {
            SignalBusInstaller.Install(Container);
        }

        private void BindSignals()
        {
            Container.DeclareSignal<OnHeadDamagedSignal>();
            Container.DeclareSignal<OnQuitGameSignal>();
            Container.DeclareSignal<OnRestartGameSignal>();
            Container.DeclareSignal<OnEndGameSignal>();
        }

        private void BindTimeService()
        {
            Container
                .Bind<TimeService>()
                .AsSingle();
        }

        private void BindSlowmoParams()
        {
            Container
                .Bind<float>()
                .FromInstance(_gameConfig.SlowmoReturnDuration);
        }

        private void BindSlowmoService()
        {
            Container
                .Bind<SlowmoService>()
                .AsSingle();
        }

        private void BindGameModel()
        {
            Container
                .Bind<GameModel>()
                .AsSingle()
                .WithArguments(_gameConfig, _gameAudioConfig);
        }

        private void BindPlayerModel()
        {
            Container
                .Bind<PlayerModel>()
                .AsSingle();
        }

        private void BindHeadModel()
        {
            Container
                .Bind<HeadModel>()
                .AsSingle()
                .WithArguments(_gameConfig, _playerTransform);
        }

        private void BindSlowmoVFXModel()
        {
            Container
                .Bind<SlowmoVFXModel>()
                .AsSingle()
                .WithArguments(_gameConfig);
        }

        private void BindPlayerTransform()
        {
            Container
                .Bind<Transform>()
                .FromInstance(_playerTransform);
        }

        private void BindBackgroundVoice()
        {
            Container
                .Bind<AudioSource>()
                .FromInstance(_backgroundVoice);
        }

        private void BindSceneLightning()
        {
            Container
                .Bind<GameObject>()
                .FromInstance(_sceneLightning);
        }

        private void BindUIController()
        {
            Container
                .Bind<UIController>()
                .FromInstance(_uiController);
        }
    }
}