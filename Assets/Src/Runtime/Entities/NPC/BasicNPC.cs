using UnityEngine;

namespace Hypergame.Entities.NPCs
{
    public class BasicNPC : MonoBehaviour
    {
        private readonly string EnablePickup = nameof(EnablePickupCollider);

        [SerializeField] private GameObject[] headAcessories;
        [SerializeField] private GameObject[] faceAcessories;
        [SerializeField] private GameObject[] bodyAcessories;
        [SerializeField] private Material[] materials;
        [SerializeField] private Rigidbody hipsRb;
        [SerializeField] private SphereCollider pickupTrigger;
        [SerializeField] private Animator animator;
        [SerializeField] private float delayOnPuched = 1f;

        private Rigidbody[] rigidbodies;
        private Collider[] colliders;

        private void Start()
        {
            rigidbodies = GetComponentsInChildren<Rigidbody>();
            colliders = GetComponentsInChildren<Collider>();

            ToggleRagdoll(false);

            var renderers = GetComponentsInChildren<Renderer>();
            var material = materials[Random.Range(0, materials.Length)];

            foreach (var render in renderers)
                render.material = material;

            ToggleAcessories(headAcessories);
            ToggleAcessories(faceAcessories);
            ToggleAcessories(bodyAcessories);
        }

        private void ToggleAcessories(GameObject[] list)
        {
            int selected = Random.Range(0, list.Length);
            for (int i = 0; i < list.Length; i++)
                list[i].SetActive(i == selected);
        }

        public void ToggleRagdoll(Vector3 direction)
        {
            ToggleRagdoll(true);
            hipsRb.AddForce(direction);
        }

        public void ToggleRagdoll(bool active)
        {
            foreach(var rb in rigidbodies)
            {
                rb.useGravity = active;
                rb.isKinematic = !active;
            }

            foreach (var coll in colliders)
                coll.enabled = active;

            colliders[0].enabled = !active;
            pickupTrigger.enabled = false;
            animator.enabled = !active;

            if (active)
                Invoke(EnablePickup, delayOnPuched);
        }

        private void EnablePickupCollider() => pickupTrigger.enabled = true;

        public void PickUpNPC()
        {
            hipsRb.transform.localPosition = Vector3.zero;
            hipsRb.transform.localEulerAngles = new Vector3(0, 0, Random.Range(90f, 270f));
            hipsRb.useGravity = false;
            hipsRb.isKinematic = true;

            pickupTrigger.enabled = false;
        }
    }
}
