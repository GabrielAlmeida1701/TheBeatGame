using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hypergame.Entities.NPC
{
    public class NPCSpawner : MonoBehaviour
    {
        public float spawnRate = 1.0f;
        public int maxEntities = 10;
        public float walkableArea = 10;
        public GameObject prefab;

        private List<GameObject> entities = new List<GameObject>();

        private void Start() => StartCoroutine(SpawnCharacter());

        public void RemoveEntity(BasicNPC npc) => entities.Remove(npc.gameObject);

        private IEnumerator SpawnCharacter()
        {
            while (true)
            {
                yield return new WaitForSeconds(spawnRate);

                if(entities.Count < maxEntities)
                {
                    Transform spawnPoint = transform.GetChild(Random.Range(0, transform.childCount));
                    GameObject entity = Instantiate(prefab, spawnPoint.position, spawnPoint.rotation);
                    entity.GetComponent<BasicNPC>().spawner = this;

                    entities.Add(entity);
                }
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.white;
            foreach(Transform child in transform)
                Gizmos.DrawSphere(child.position, 2f);

            Gizmos.color = new Color(0, 1, 0, .3f);
            Gizmos.DrawSphere(transform.position, walkableArea);
        }
    }
}