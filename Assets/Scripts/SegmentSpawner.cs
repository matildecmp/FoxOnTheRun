using System.Collections.Generic;
using UnityEngine;

public class SegmentSpawner : MonoBehaviour
{
    [Header("Segments references")]
    public GameObject[] segments;
    private float zSpawn = 0; 
    private float segmentLength = 50f; 
    private int maxSegments = 7;
    private Queue<GameObject> activeSegments = new Queue<GameObject>();

    [Header("Other references")]
    public Transform player;
    public EnemySpawner enemySpawner;
    public CollectibleSpawner collectibleSpawner;

    // Initially spawn all the different segments (from 00 to 006)  
    void Start()
    {
        for (int i = 0; i < maxSegments; i++)
        {
            SpawnSegment(i % segments.Length);
        }
    }

    // Spawn new segments
    void Update()
    {
        // When Player is getting close to the segments end, spawn a new segment
        if (player.position.z > zSpawn - (maxSegments * segmentLength))
        {
            SpawnSegment(Random.Range(0, segments.Length));
        }

        // If Player has crossed the first segment of the queue, it gets removed from the queue and destroyed
        if (activeSegments.Count > 0)
        {
            GameObject firstSegment = activeSegments.Peek();

            if (player.position.z > firstSegment.transform.position.z + segmentLength)
            {
                GameObject oldSegment = activeSegments.Dequeue();
                Destroy(oldSegment);
            }
        }
    }

    // Spawn a new segment in zSpawn position and place collectibles and enemies over it
    void SpawnSegment(int index)
    {
        GameObject newSegment = Instantiate(segments[index], new Vector3(0, 0, zSpawn), Quaternion.identity);
        
        enemySpawner.SpawnEnemies(newSegment);
        collectibleSpawner.SpawnCollectibles(newSegment);

        activeSegments.Enqueue(newSegment);

        zSpawn += segmentLength;
    }
}