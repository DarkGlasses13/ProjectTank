using UnityEngine;

namespace Assets.Scripts
{
    public class UIPanel : MonoBehaviour
    {
        public void Enable() => gameObject.SetActive(true);

        public void Disable() => gameObject.SetActive(false);
    }
}