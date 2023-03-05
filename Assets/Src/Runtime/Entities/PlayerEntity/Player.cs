using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hypergame.Entities.PlayerEntity
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        [SerializeField] private StackController stackController;

        public float speed = 3;

        private Vector2 direction;
        private bool moving;

        public void Move(Vector2 direction)
        {
            moving = direction.magnitude > .01f;
            
            this.direction = moving? direction : Vector2.zero;
            animator.SetBool("Running", moving);
        }

        private void Update()
        {
            Vector3 inputDirection = new Vector3(direction.x, transform.position.y, direction.y);

            if (moving)
            {
                transform.LookAt(transform.position + inputDirection);
                transform.Translate(0, 0, speed * Time.deltaTime);
            }
        }
    }
}
