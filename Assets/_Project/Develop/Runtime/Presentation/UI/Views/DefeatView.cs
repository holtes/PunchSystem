using _Project.Develop.Runtime.Core.Signals;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using R3;

namespace _Project.Develop.Runtime.Presentation.UI.Views
{
    public class DefeatView : TextView
    {
        [SerializeField] private Button _restartBtn;

        private SignalBus _signalBus;

        [Inject]
        private void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }

        private void Awake()
        {
            _restartBtn.OnClickAsObservable()
                .Subscribe(_ => RestartGame())
                .AddTo(this);
        }

        private void RestartGame()
        {
            _signalBus.Fire<OnRestartGameSignal>();
        }
    }
}