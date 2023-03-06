using Hypergame.Entities.NPCs;
using UnityEngine;

namespace Hypergame.Entities.PlayerEntity
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        [SerializeField] private StackController stackController;
        [SerializeField] private Material skinMaterial;
        
        public float speed = 3;
        public float punchForce = 500;
        public int stackLimit = 3;

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
            if (moving)
            {
                Vector3 inputDirection = new Vector3(direction.x, transform.position.y, direction.y);

                transform.LookAt(transform.position + inputDirection);
                transform.Translate(0, 0, speed * Time.deltaTime);
            }
        }

        public void UpdateColor(Color color)
        {
            skinMaterial.color = color;
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.TryGetComponent(out BasicNPC npc))
                npc.ToggleRagdoll(transform.forward * punchForce);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (stackController.StackCount >= stackLimit)
                return;

            BasicNPC npc = other.GetComponentInParent<BasicNPC>();
            if (npc)
            {
                npc.PickUpNPC();
                stackController.AddToPile(npc.transform);
            }
        }
    }
}
