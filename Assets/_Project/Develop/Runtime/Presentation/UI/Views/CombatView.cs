using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

namespace _Project.Develop.Runtime.Presentation.UI.Views
{
    public class CombatView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _timer;
        [SerializeField] private Slider _victoryProgressBar;

        public void SetTime(float timeRemaining)
        {
            var timeSpan = TimeSpan.FromSeconds(timeRemaining);

            string formatted;

            if (timeSpan.TotalMinutes >= 1)
                formatted = string.Format("{0:D2}:{1:D2}.{2:D1}", timeSpan.Minutes, timeSpan.Seconds, timeSpan.Milliseconds / 100);
            else
                formatted = string.Format("{0:D2}.{1:D1}", timeSpan.Seconds, timeSpan.Milliseconds / 100);

            _timer.text = formatted;
        }

        public void SetProgress(float value)
        {
            _victoryProgressBar.value = value;
        }
    }
}