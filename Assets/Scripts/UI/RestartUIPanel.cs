using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class RestartUIPanel : UIPanel 
    {
        [SerializeField] private Button _restartButton;
        [SerializeField] private TMP_Text _message;

        public Button RestartButton => _restartButton;

        public void ShowWin()
        {
            _message.text = "Вы выиграли !";
            Enable();
        }

        public void ShowLoose()
        {
            _message.text = "Вы проиграли !";
            Enable();
        }
    }
}