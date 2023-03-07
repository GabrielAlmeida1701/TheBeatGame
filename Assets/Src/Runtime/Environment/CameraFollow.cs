using UnityEngine;

namespace Hypergame.Environment
{
    public class CameraFollow : MonoBehaviour
    {
        [Header("Follow Settings")]
        public Transform target;
        public float smoothTime = .03f;

        [Header("Shake Settings")]
        public float shakeAmount = 0.1f;
        public float decreaseFactor = 1.0f;

        private float shakeDuration = 0f;
        private Vector3 velocity;
        private Vector3 originalPos;

        private void LateUpdate()
        {
            transform.position = Vector3.SmoothDamp(transform.position, target.position, ref velocity, smoothTime);

            if (shakeDuration > 0)
            {
                transform.localPosition = originalPos + Random.insideUnitSphere * shakeAmount;
                shakeDuration -= Time.deltaTime * decreaseFactor;
            }
        }

        public void ShakeCamera()
        {
            originalPos = transform.localPosition;
            shakeDuration = 0.2f;
        }
    }
}
