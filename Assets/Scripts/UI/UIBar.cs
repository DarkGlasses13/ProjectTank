using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class UIBar : MonoBehaviour 
    {
        private Slider _slider;

        private void OnEnable() => _slider = GetComponent<Slider>();

        public void SetValue(float value) => _slider.value = value;
    }
}