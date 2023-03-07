using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace Hypergame.Entities.NPC
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class BasicNPC : MonoBehaviour
    {
        private readonly string EnablePickup = nameof(EnablePickupCollider);

        [Header("Acessories")]
        [SerializeField] private GameObject[] headAcessories;
        [SerializeField] private GameObject[] faceAcessories;
        [SerializeField] private GameObject[] bodyAcessories;
        [SerializeField] private Material[] materials;

        [Header("Settings")]
        [SerializeField] private Rigidbody hipsRb;
        [SerializeField] private SphereCollider pickupTrigger;
        [SerializeField] private Animator animator;
        [SerializeField] private float delayOnPuched = 1f;

        [Header("Moviment Settings")]
        [SerializeField] private float minWaitTime = 1.0f;
        [SerializeField] private float maxWaitTime = 3.0f;
        [SerializeField] private float movementSpeed = 3.0f;
        [SerializeField] private NavMeshAgent agent;

        [HideInInspector]
        public NPCSpawner spawner;

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

            StartCoroutine(RandomMovement());
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

            ToggleCollider(!active);
            pickupTrigger.enabled = false;
            animator.enabled = !active;

            if (active)
            {
                Invoke(EnablePickup, delayOnPuched);
                StopAllCoroutines();
                agent.isStopped = true;
                agent.enabled = false;
            }
        }

        public void ToggleCollider(bool active) => colliders[0].enabled = active;

        private void EnablePickupCollider() => pickupTrigger.enabled = true;

        public void PickUpNPC()
        {
            hipsRb.transform.localPosition = Vector3.zero;
            hipsRb.transform.localEulerAngles = new Vector3(0, 0, Random.Range(90f, 270f));
            hipsRb.useGravity = false;
            hipsRb.isKinematic = true;

            pickupTrigger.enabled = false;
            spawner.RemoveEntity(this);
        }

        private IEnumerator RandomMovement()
        {
            while (true)
            {
                Vector3 randomPoint = RandomNavmeshLocation();
                agent.SetDestination(randomPoint);
                animator.SetBool("Walking", true);
                yield return new WaitUntil(() => !agent.pathPending && agent.remainingDistance < 0.1f);

                animator.SetBool("Walking", false);
                float waitTime = Random.Range(minWaitTime, maxWaitTime);
                yield return new WaitForSeconds(waitTime);
            }
        }

        private Vector3 RandomNavmeshLocation()
        {
            Vector3 randomDirection = (Random.insideUnitSphere * spawner.walkableArea) + spawner.transform.position;
            NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, 10.0f, NavMesh.AllAreas);
            return hit.position;
        }
    }
}
