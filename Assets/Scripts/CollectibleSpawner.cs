using System.Collections.Generic;
using UnityEngine;

// Editable class containing weighted collectibles
[System.Serializable]
public class WeightedCollectible
{
    public GameObject prefab;
    public float weight = 1f;
    public float weightGrowth = 0f;
}

public class CollectibleSpawner : MonoBehaviour
{
    public List<WeightedCollectible> weightedCollectibles;
    public int maxCountPerArea = 4;
    public float collectibleHeight = 0.2f;
    public float spacing = 2f;
    public float difficultyGrowthRate = 0.5f;
    private float playerScore = ShowScore.score;

    // Spawn random collectibles inside designated areas in a certain segment 
    public void SpawnCollectibles(GameObject segment)
    {
        if (weightedCollectibles.Count == 0) return;

        var collectibleBoxes = new List<BoxCollider>();
        var boxes = segment.GetComponentsInChildren<BoxCollider>();

        foreach (var box in boxes)
        {
            if (!box.isTrigger) continue;

            if (box.gameObject.name.Contains("Collectible"))
            {
                collectibleBoxes.Add(box);
            }
        }

        if (collectibleBoxes.Count == 0) return;

        GameObject wrapper = CreateWrapper(segment);

        foreach (var box in collectibleBoxes)
        {
            SpawnInBox(box, wrapper.transform);
        }
    }

    // Create a Wrapper GameObject for collectibles
    private GameObject CreateWrapper(GameObject segment)
    {
        // Create Wrapper and set it as a child of Segment (when Segment gets destroyed, so will the collectibles instantiated above it) + "false" to maintain Wrapper's local position/rotation/scale
        GameObject wrapper = new GameObject("Collectible Wrapper");
        wrapper.transform.SetParent(segment.transform, false); 
        wrapper.transform.localPosition = Vector3.zero;
        wrapper.transform.localRotation = Quaternion.identity;

        // Set Wrapper local scale as the inverse of Segment local scale (20,1,50), so that the collectibles won't be deformed by Segment scale 
        Vector3 segmentScale = segment.transform.localScale;
        wrapper.transform.localScale = new Vector3(
            1f / segmentScale.x,
            1f / segmentScale.y,
            1f / segmentScale.z
        );

        return wrapper;
    }

    // Spawn collectibles inside a given area
    private void SpawnInBox(BoxCollider box, Transform parent)
    {
        // References to box orientation
        Vector3 center = box.transform.position;
        Vector3 forward = box.transform.TransformDirection(Vector3.forward).normalized; 
        Vector3 up = box.transform.up; 

        // How many objects to spawn according to box length and spacing
        float boxLength = Vector3.Scale(box.size, box.transform.lossyScale).z;
        int count = Mathf.Clamp(Mathf.FloorToInt(boxLength / spacing), 1, maxCountPerArea);
        Vector3 startPos = center - forward * ((count - 1) * spacing / 2f);

        // Spawn collectibles
        for (int i = 0; i < count; i++)
        {
            Vector3 worldPos = startPos + forward * i * spacing;
            worldPos += up * collectibleHeight;
            Vector3 localPos = parent.InverseTransformPoint(worldPos);

            GameObject selected = GetWeightedRandomCollectible(box);
            GameObject collectible = Instantiate(selected, parent);

            collectible.transform.localPosition = localPos;
            collectible.transform.localRotation = Quaternion.identity;
        }
    }

    // Get a random collectible according to its weight
    // Weight is dynamic for Mushroom and Power Up, since their growth factor is != 0
    // Weight is higher for Golden Apples on wooden platforms
    private GameObject GetWeightedRandomCollectible(BoxCollider box)
    {
        float difficultyFactor = ShowScore.score * difficultyGrowthRate;
        List<float> dynamicWeights = new List<float>();
        bool isOnPlatform = box.CompareTag("Ground") && box.gameObject.layer == LayerMask.NameToLayer("Obstacle");
        
        float weightsSum = 0f;

        foreach (var coll in weightedCollectibles)
        {
            float dynamicWeight = coll.weight + (coll.weightGrowth * difficultyFactor);
            if (isOnPlatform && coll.prefab.name == "Golden Apple")
            {
                dynamicWeight *= 70f;
            }

            dynamicWeights.Add(dynamicWeight);
            weightsSum += dynamicWeight;
        }

        float rand = Random.Range(0f, weightsSum);
        float temp_sum = 0f;

        for (int i = 0; i < weightedCollectibles.Count; i++)
        {
            temp_sum += dynamicWeights[i];
            if (rand <= temp_sum)
            {
                return weightedCollectibles[i].prefab;
            }
        }
        return weightedCollectibles[0].prefab;
    }

}
