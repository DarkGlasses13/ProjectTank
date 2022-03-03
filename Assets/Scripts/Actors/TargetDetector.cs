using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Zenject;

namespace Assets.Scripts
{
    public class TargetDetector : MonoBehaviour
    {
        public event Action OnDetect;
        public event Action OnLose;

        [SerializeField] [Range(5, 100)] private float _detectionRadius;
        [SerializeField] private LayerMask _targetLayerMask;

        private UpdateService _updateService;

        public Transform Target { get; private set; }
        public float DetectionRadius => _detectionRadius;

        [Inject] private void Construct(UpdateService updateService) => _updateService = updateService;

        private void OnEnable() => _updateService.OnUpdate += FindTarget;

        private void FindTarget()
        {
            Vector3 rayOffset = new Vector3(transform.position.x, 2.5f, transform.position.z);
            Collider[] targetsInRadius = Physics.OverlapSphere(transform.position, _detectionRadius, _targetLayerMask);
            List<Transform> visibleTargets = new List<Transform>();
            visibleTargets.Clear();

            foreach (Collider target in targetsInRadius)
            {
                Vector3 directionToTarget = (target.transform.position - transform.position).normalized;
                float distanceToTarget = Vector3.Distance(transform.position, target.transform.position);

                if (!Physics.Raycast(rayOffset, directionToTarget, distanceToTarget))
                {
                    visibleTargets.Add(target.transform);
                }
            }

            if (visibleTargets.Count > 0)
            {
                SetTarget(visibleTargets[0]);
            }
            else
            {
                LooseTarget();
            }

            for (int i = 0; i < visibleTargets.Count; i++)
            {
                if (Vector3.Distance(visibleTargets[i].position, transform.position) < Vector3.Distance(Target.position, transform.position))
                    Target = visibleTargets[i];
            }
        }

        private void LooseTarget()
        {
            if (Target != null)
            {
                Target = null;
                OnLose?.Invoke();
            }
        }

        private void SetTarget(Transform target)
        {
            if (Target == null)
            {
                Target = target;
                OnDetect?.Invoke();
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