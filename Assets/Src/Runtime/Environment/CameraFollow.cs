using UnityEngine;

namespace Hypergame.Environment
{
    public class CameraFollow : MonoBehaviour
    {
        public Transform target;
        public float smoothTime = .03f;

        private Vector3 velocity;

        private void LateUpdate()
        {
            transform.position = Vector3.SmoothDamp(transform.position, target.position, ref velocity, smoothTime);
        }
    }
}
