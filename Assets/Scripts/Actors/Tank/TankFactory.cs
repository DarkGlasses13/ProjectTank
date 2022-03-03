using UnityEngine;
using Zenject;

namespace Assets.Scripts
{
    public class TankFactory : PlaceholderFactory<Tank, Transform, Transform, LevelStartup, Tank> { }
}