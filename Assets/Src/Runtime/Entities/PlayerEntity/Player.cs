using Hypergame.Entities.NPCs;
using Hypergame.Entities.Persistance;
using Hypergame.Entities.PlayerEntity.Stack;
using UnityEngine;

namespace Hypergame.Entities.PlayerEntity
{
    public class Player : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Animator animator;
        [SerializeField] private StackController stackController;
        [SerializeField] private Material skinMaterial;
        
        [Header("Gameplay Values")]
        public float speed = 3;
        public float punchForce = 500;
        
        [SerializeField]
        private PlayerData data;

        private Vector2 direction;
        private bool moving;

        public int StackLimit => data.stackLimit;
        public int Points => data.points;
        public int ActiveColorId => data.activeColor;

        private void Awake()
        {
            data = PersistanceManager.LoadPlayerData(data);
        }

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

        public void UpdateColor(Color color, int activeId, int cost)
        {
            if (!data.colors.Contains(activeId))
                data.colors.Add(activeId);

            data.points -= cost;
            data.activeColor = activeId;
            skinMaterial.color = color;

            PersistanceManager.SavePlayerData(data);
        }

        public void IncreaseStackLimit(int cost)
        {
            data.points -= cost;
            data.stackLimit++;

            PersistanceManager.SavePlayerData(data);
        }

        public bool IsColorUnlocked(int colorId) => (bool) data.colors?.Contains(colorId);

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.TryGetComponent(out BasicNPC npc))
                npc.ToggleRagdoll(transform.forward * punchForce);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Shop"))
            {
                data.points += stackController.StackCount;
                stackController.ClearStack();
                PersistanceManager.SavePlayerData(data);
                return;
            }

            if (stackController.StackCount >= data.stackLimit)
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
