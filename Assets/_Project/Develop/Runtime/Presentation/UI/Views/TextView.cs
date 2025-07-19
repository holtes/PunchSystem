using TMPro;
using UnityEngine;

namespace _Project.Develop.Runtime.Presentation.UI.Views
{
    public abstract class TextView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _viewText;

        public void SetText(string text)
        {
            _viewText.text = text;
        }
    }
}