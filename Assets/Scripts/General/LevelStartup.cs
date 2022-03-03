using System.Collections.Generic;
using UnityEngine;
using Zenject;
using UnityEngine.Events;

namespace Assets.Scripts
{
    public class LevelStartup : MonoBehaviour
    {
        [HideInInspector] public UnityEvent OnRestart;

        [Header("PARENTS")]
        [SerializeField] private Transform _actorsParent; 

        [Header("SPAWN POINTS")]
        [SerializeField] private SpawnPoint _tankSpawnPoint;
        [SerializeField] private List<SpawnPoint> _turretSpawnPoints;
        [SerializeField] private List<SpawnPoint> _bossTurretSpawnPoints;

        [Header("ACTOR PREFABS")]
        [SerializeField] private Turret _turretPrefab;
        [SerializeField] private Turret _bossTurretPrefab;
        [SerializeField] private Tank _tankPrefab;
        [SerializeField] private PlayerCamera _playerCameraPrefab;

        private TankFactory _tankFactory;
        private Helther _tankHelther;
        private TurretFactory _turretFactory;
        private Helther[] _bossTurretsHelther;
        private PlayerCameraFactory _playerCameraFactory;
        private RestartUIPanel _restartPanel;

        [Inject] private void Construct
        (
            TankFactory tankFactory,
            PlayerCameraFactory playerCameraFactory,
            TurretFactory turretFactory,
            RestartUIPanel restartPanel
        )
        {
            _tankFactory = tankFactory;
            _turretFactory = turretFactory;
            _playerCameraFactory = playerCameraFactory;
            _restartPanel = restartPanel;
        }

        private void Start()
        {
            Tank tank = SpawnTank();
            SpawnPlayerCamera(tank.transform);
            SpawnTurrets();
            SpawnBossTurrets();
            _restartPanel.RestartButton.onClick.AddListener(Restart);
        }

        private void Restart()
        {
            OnRestart?.Invoke();
            _restartPanel.Disable();
        }

        private void Lose() => _restartPanel.ShowLoose();

        private void Win() => _restartPanel.ShowWin();

        private PlayerCamera SpawnPlayerCamera(Transform target) => _playerCameraFactory.Create(_playerCameraPrefab, _actorsParent, target);

        private Tank SpawnTank()
        {
            Tank tank = _tankFactory.Create(_tankPrefab, _actorsParent, _tankSpawnPoint.transform, this);
            _tankHelther = tank.GetComponent<Helther>();
            _tankHelther.OnDeath += Lose;
            return tank;
        }

        private void SpawnBossTurrets()
        {
            _bossTurretsHelther = new Helther[_bossTurretSpawnPoints.Count];

            for (int i = 0; i < _bossTurretSpawnPoints.Count; i++)
            {
                Turret boss = _turretFactory.Create
                (
                    _bossTurretPrefab,
                    _actorsParent,
                    _bossTurretSpawnPoints[i].transform,
                    this
                );

                _bossTurretsHelther[i] = boss.GetComponent<Helther>();
                _bossTurretsHelther[i].OnDeath += Win;
            }
        }

        private void SpawnTurrets()
        {
            foreach (SpawnPoint spawnPoint in _turretSpawnPoints)
                _turretFactory.Create(_turretPrefab, _actorsParent, spawnPoint.transform, this);
        }

        private void OnDisable()
        {
            _restartPanel.RestartButton.onClick.RemoveListener(Restart);
            _tankHelther.OnDeath -= Lose;

            foreach (Helther bossTurret in _bossTurretsHelther)
                bossTurret.OnDeath -= Win;

            OnRestart.RemoveAllListeners();
        }
    }
}