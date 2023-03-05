using UnityEngine;

namespace Hypergame.Entities.PlayerEntity
{
    public class StackController : MonoBehaviour
    {
        [SerializeField] private float speed = 2f;
        [SerializeField] private float minDistance = 0.1f;
        [SerializeField] private float maxDistance = 0.6f;
        [SerializeField] private Transform stackParent;

        private Knod[] knods;

        private void Start()
        {
            knods = stackParent.GetComponentsInChildren<Knod>();
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

        private void Update()
        {
            int i = 0;
            foreach (Knod knod in knods)
            {
                knod.target = transform;
                knod.speed = speed * i;
                knod.distance = minDistance;
                knod.maxDistance = maxDistance;

                i++;
            }
        }
    }

}