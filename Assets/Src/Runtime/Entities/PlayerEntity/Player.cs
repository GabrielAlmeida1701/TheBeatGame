using Hypergame.Entities.NPC;
using Hypergame.Entities.Persistance;
using Hypergame.Entities.PlayerEntity.Stack;
using UnityEngine;
using TMPro;
using Hypergame.Environment;

namespace Hypergame.Entities.PlayerEntity
{
    public class Player : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Animator animator;
        [SerializeField] private StackController stackController;
        [SerializeField] private Material skinMaterial;
        [SerializeField] private TextMeshProUGUI coinsLabel;
        [SerializeField] private new CameraFollow camera;
        
        [Header("Gameplay Values")]
        public float speed = 3;
        public float punchForce = 500;
        
        [SerializeField]
        private PlayerData data;

        private Vector2 direction;
        private bool moving;
        private BasicNPC targetNPC;

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
            if (!moving)
                return;

            Vector3 inputDirection = new Vector3(direction.x, transform.position.y, direction.y);
            transform.LookAt(transform.position + inputDirection);
            transform.Translate(0, 0, speed * Time.deltaTime);
        }

        public void UpdateColor(Color color, int activeId, int cost)
        {
            if (!data.colors.Contains(activeId))
                data.colors.Add(activeId);

            UpdatePoints(-cost);
            data.activeColor = activeId;
            skinMaterial.color = color;

            PersistanceManager.SavePlayerData(data);
        }

        public void IncreaseStackLimit(int cost)
        {
            UpdatePoints(-cost);
            data.stackLimit++;

            PersistanceManager.SavePlayerData(data);
        }

        public bool IsColorUnlocked(int colorId) => (bool) data.colors?.Contains(colorId);

        public void PunchNPC()
        {
            if (!targetNPC) return;

            camera.ShakeCamera();
            targetNPC.ToggleRagdoll(transform.forward * punchForce);
            targetNPC = null;
        }

        private void UpdatePoints(int ammount)
        {
            data.points += ammount;
            coinsLabel.text = data.points.ToString();
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.TryGetComponent(out BasicNPC npc))
            {
                if (targetNPC) targetNPC.ToggleCollider(true);
                npc.ToggleCollider(false);
                animator.SetTrigger("Punch");
                targetNPC = npc;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Shop"))
            {
                UpdatePoints(stackController.StackCount);
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
