using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    public GameObject[] objectsToParallax;
    private Vector3 originalCamPosition;
    public Vector3[] originalPosition;
    public float absoluteParallaxThreshold;
    // Start is called before the first frame update
    void Start()
    {
        objectsToParallax = GameObject.FindGameObjectsWithTag("Parallax");
        originalCamPosition = Camera.main.gameObject.transform.position;
        originalPosition = new Vector3[objectsToParallax.Length];
        foreach (GameObject obj in objectsToParallax)
        {
            originalPosition[System.Array.IndexOf(objectsToParallax, obj)] = obj.transform.position;

        }
    }

    // Update is called once per frame
    void Update()
    {
        foreach(GameObject obj in objectsToParallax)
        {
            float zPos = obj.transform.position.z;
            float parallaxMultiplier = zPos / absoluteParallaxThreshold;
            Vector3 movement = (Camera.main.gameObject.transform.position - originalCamPosition) * parallaxMultiplier;
            obj.transform.position = originalPosition[System.Array.IndexOf(objectsToParallax, obj)] + movement;
        }
    }
}
