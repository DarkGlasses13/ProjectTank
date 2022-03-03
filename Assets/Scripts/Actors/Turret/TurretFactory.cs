using UnityEngine;
using Zenject;

namespace Assets.Scripts
{
    public class TurretFactory : PlaceholderFactory<Turret, Transform, Transform, LevelStartup, Turret> { }
}