using UnityEngine;
using UnityEngine.AI;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Collider floorCollider;
    public int enemyCount = 15;

    void Start()
    {
        if (floorCollider != null)
        {
            SpawnEnemies();
        }
        else
        {
            Debug.LogError("В спавнер не добавлен пол (Floor Collider)!");
        }
    }

    void SpawnEnemies()
    {
        Bounds bounds = floorCollider.bounds;
        int spawned = 0;
        int attempts = 0;

        while (spawned < enemyCount && attempts < 1000)
        {
            attempts++;

            float randomX = Random.Range(bounds.min.x, bounds.max.x);
            float randomZ = Random.Range(bounds.min.z, bounds.max.z);

            Vector3 origin = new Vector3(randomX, bounds.max.y + 30f, randomZ);
            RaycastHit hit;

            if (Physics.Raycast(origin, Vector3.down, out hit, 60f))
            {
                if (hit.collider == floorCollider)
                {
                    NavMeshHit navHit;
                    if (NavMesh.SamplePosition(hit.point, out navHit, 1f, NavMesh.AllAreas))
                    {
                        Instantiate(enemyPrefab, navHit.position, Quaternion.identity);
                        spawned++;
                    }
                }
            }
        }

        if (spawned < enemyCount)
        {
            Debug.LogWarning("Места хватило только на " + spawned + " врагов. Проверь размер пола.");
        }
    }
}