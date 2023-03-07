using UnityEngine;

namespace Hypergame.Entities.PlayerEntity.Stack
{
    public class StackController : MonoBehaviour
    {
        [SerializeField] private float speed = 2f;
        [SerializeField] private float minDistance = 0.1f;
        [SerializeField] private float maxDistance = 0.6f;
        [SerializeField] private Transform stackParent;

        private Knod[] knods;

        public int StackCount => stackParent.childCount;

        private void Start()
        {
            knods = stackParent.GetComponentsInChildren<Knod>();
            if (knods.Length == 0) return;

            Transform lastKnod = knods[0].transform;

            int i = 0;
            foreach (Knod knod in knods)
            {
                if (lastKnod != knod.transform)
                    knod.bellow = lastKnod;

                knod.target = transform;
                knod.speed = speed * i;
                knod.distance = minDistance;
                knod.maxDistance = maxDistance;

                lastKnod = knod.transform;
                i++;
            }
        }

        public void AddToPile(Transform transform)
        {
            int childCount = stackParent.childCount;
            Transform target = childCount == 0 ? this.transform : stackParent.GetChild(childCount - 1);

            Knod knod = transform.gameObject.AddComponent<Knod>();
            transform.parent = stackParent;
            knod.target = target;
            knod.speed = speed * childCount;
            knod.distance = minDistance;
            knod.maxDistance = maxDistance;
        }

        public void ClearStack()
        {
            foreach (Transform child in stackParent)
                Destroy(child.gameObject);
        }
    }
}