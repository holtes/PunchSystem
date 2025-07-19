using _Project.Develop.Runtime.Core.Enums;
using _Project.Develop.Runtime.Presentation.UI.Views;
using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;
using TSS;


namespace _Project.Develop.Runtime.Presentation.UI.Controllers
{
    public class UIController : MonoBehaviour
    {
        [SerializeField] private TSSCore _tssCore;
        [SerializeField] private IntroView _introView;
        [SerializeField] private CombatView _combatView;
        [SerializeField] private VictoryView _victoryView;
        [SerializeField] private DefeatView _defeatView;
        [SerializeField] private List<GameStateText> _gameStateTexts;

        [Serializable]
        private struct GameStateText
        {
            public GameState State;
            public TextView TextView;
        }

        private Dictionary<GameState, TextView> _gameStateTextsDic = new Dictionary<GameState, TextView>();

        private void Awake()
        {
            _gameStateTextsDic = _gameStateTexts.ToDictionary(item => item.State, item => item.TextView);
        }

        public void ShowScreen(GameState gameState)
        {
            _tssCore.SelectState(gameState.ToString());
        }

        public void ShowSubtitles(GameState gameState, string text)
        {
            _gameStateTextsDic[gameState].SetText(text);
        }

        public void SetGameProgress(float value)
        {
            _combatView.SetProgress(value);
        }

        public void SetGameTimeRemaining(float timeRemaining)
        {
            _combatView.SetTime(timeRemaining);
        }
    }
}