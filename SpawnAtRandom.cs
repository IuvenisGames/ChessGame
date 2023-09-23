using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnAtRandom : MonoBehaviour
{

    public Vector2 xBounds;
    public Vector2 yBounds;
    public float spawnRate;
    public GameObject objectToSpawn;

    private float t;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        t += Time.deltaTime * spawnRate;
        if (t > 1)
        {
            t = 0;
            Instantiate(objectToSpawn, new Vector3(Random.Range(xBounds.x, xBounds.y), Random.Range(yBounds.x, yBounds.y), 0), Quaternion.identity);
        }
    }
}
