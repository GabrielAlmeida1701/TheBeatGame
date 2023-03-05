using UnityEngine;

namespace Hypergame.Entities.PlayerEntity
{
    public class Knod : MonoBehaviour
    {
        public float speed;
        public float distance;
        public float maxDistance;
        public Transform target;
        public Transform bellow;

        private Vector3 velocity;

        void FixedUpdate()
        {
            if (!target) return;

            Vector3 childPosition = bellow ? bellow.position - (bellow.forward * distance) : target.position + (Vector3.up * distance);
            transform.position = Vector3.SmoothDamp(transform.position, childPosition, ref velocity, speed);

            if (bellow)
            {
                Vector3 direction = transform.position - bellow.position;
                float distance = direction.magnitude;
                if (distance > maxDistance)
                {
                    Vector3 clampedDirection = Vector3.ClampMagnitude(direction, maxDistance);
                    transform.position = bellow.position + clampedDirection;
                }
            }

            transform.LookAt(target);

            var euler = transform.eulerAngles;
            euler.y = target.eulerAngles.y;
            transform.eulerAngles = euler;
        }
    }
}