using System;

namespace Assets.Scripts
{
    public interface IUIDataDispenser
    {
        public event Action<float> OnValueChange;
    }
}