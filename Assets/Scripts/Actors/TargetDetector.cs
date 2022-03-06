using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Zenject;

namespace Assets.Scripts
{
    public class TargetDetector : MonoBehaviour
    {
        [SerializeField] [Range(5, 100)] private float _detectionRadius;
        [SerializeField] private LayerMask _targetLayerMask;

        private UpdateService _updateService;
        private List<Transform> _visibleTargets = new List<Transform>();
        private const int _closestTargetIndex = 0;

        public Transform Target { get; private set; }
        public float DetectionRadius => _detectionRadius;

        [Inject] private void Construct(UpdateService updateService) => _updateService = updateService;

        private void OnEnable() => _updateService.OnUpdate += FindTarget;

        private void FindTarget()
        {
            Detect();
            Sort();
            SetTarget();
        }

        private void SetTarget() => Target = _visibleTargets.Count > 0 ? _visibleTargets[_closestTargetIndex] : null;

        private void Sort()
        {
            for (int i = 0; i < _visibleTargets.Count; i++)
            {
                float distanceToVisibleTarget = Vector3.Distance(_visibleTargets[i].position, transform.position);
                float distanceToClosestTarget = Vector3.Distance(_visibleTargets[_closestTargetIndex].position, transform.position);

                if (distanceToVisibleTarget < distanceToClosestTarget)
                {
                    _visibleTargets[_closestTargetIndex] = _visibleTargets[i];
                }
            }
        }

        private void Detect()
        {
            Vector3 rayOffset = new Vector3(transform.position.x, 2.5f, transform.position.z);
            Collider[] targetsInRadius = Physics.OverlapSphere(transform.position, _detectionRadius, _targetLayerMask);
            _visibleTargets.Clear();

            foreach (Collider target in targetsInRadius)
            {
                Vector3 directionToTarget = (target.transform.position - transform.position).normalized;
                float distanceToTarget = Vector3.Distance(transform.position, target.transform.position);

                if (!Physics.Raycast(rayOffset, directionToTarget, distanceToTarget))
                {
                    _visibleTargets.Add(target.transform);
                }
            }
        }

        private void OnDisable() => _updateService.OnUpdate -= FindTarget;
    }

    [CustomEditor(typeof(TargetDetector))]
    public class TargetDetectorEditor : Editor
    {
        private void OnSceneGUI()
        {
            TargetDetector aimer = target as TargetDetector;
            Handles.color = Color.red;
            Handles.DrawWireArc(aimer.transform.position, Vector3.up, Vector3.forward, 360, aimer.DetectionRadius);
        }
    }
}