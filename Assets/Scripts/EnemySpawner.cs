using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;   
    public float enemyHeight = -0.5f;

    // Spawn an enemy in the corresponding area
    public void SpawnEnemies(GameObject segment)
    {
        if (enemyPrefab == null) return;

        GameObject wrapper = CreateWrapper(segment);
        var boxes = segment.GetComponentsInChildren<BoxCollider>();

        foreach (var box in boxes)
        {
            if (!box.isTrigger || !box.gameObject.name.Contains("Spawn Area Enemy"))
            {
                continue;
            }
            SpawnEnemyInBox(box, wrapper.transform);
        }
    }

    // Create a wrapper so that Enemy's proportions aren't influenced by the segment's scale
    private GameObject CreateWrapper(GameObject segment)
    {
        GameObject wrapper = new GameObject("Enemy Wrapper");
        wrapper.transform.SetParent(segment.transform, false);
        wrapper.transform.localPosition = Vector3.zero;
        wrapper.transform.localRotation = Quaternion.identity;

        Vector3 segmentScale = segment.transform.localScale;
        wrapper.transform.localScale = new Vector3(
            1f / segmentScale.x,
            1f / segmentScale.y,
            1f / segmentScale.z
        );

        return wrapper;
    }

    // Spawn an enemy in the center of the box area
    private void SpawnEnemyInBox(BoxCollider box, Transform parent)
    {
        Vector3 spawnPos = box.transform.position;
        spawnPos.y += enemyHeight;

        GameObject enemy = Instantiate(enemyPrefab, spawnPos, Quaternion.identity, parent);
        enemy.transform.localScale = Vector3.one * 1.6f;
        enemy.transform.rotation = Quaternion.Euler(0f, 180f, 0f);

        if (!enemy.CompareTag("Enemy"))
        {
            enemy.tag = "Enemy";
        }
    }
}
