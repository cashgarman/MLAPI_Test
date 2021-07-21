using System;
using UnityEngine;

namespace DitzelGames.FastIK
{
    public class FastIKLook : MonoBehaviour
    {
        /// <summary>
        /// Look at target
        /// </summary>
        public Transform Target;

        [SerializeField] private Vector3 ForwardDirection;

        /// <summary>
        /// Initial direction
        /// </summary>
        protected Vector3 StartDirection;

        /// <summary>
        /// Initial Rotation
        /// </summary>
        protected Quaternion StartRotation;

        private Transform _prevTarget;

        void Awake()
        {
            _prevTarget = Target;
            
            StartRotation = transform.rotation;

            if (Target != null)
            {
                StartDirection = Target.position - transform.position;
            }
        }

        void LateUpdate()
        {
            // If the target has changed to a valid target
            /*if (Target != _prevTarget && Target != null)
            {
                Debug.Log($"Found new target {Target.name}");
                
                // Calculate the initial direction to the new target
                StartDirection = Target.position - transform.position;
                
                _prevTarget = Target;
            }*/
            
            if (Target == null)
                return;

            transform.rotation = Quaternion.FromToRotation(ForwardDirection, Target.position - transform.position) * StartRotation;
            
            Debug.DrawRay(transform.position, Target.position - transform.position, Color.green);
        }

        private void OnDrawGizmos()
        {
            Debug.DrawLine(transform.position, transform.position + ForwardDirection, Color.blue);
        }
    }
}
