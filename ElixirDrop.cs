using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElixirDrop : MonoBehaviour
{
    public int amount;
    public float speed;

    public Transform target;
    public int colour;

    private float t;
    // Start is called before the first frame update
    void Start()
    {
        if(colour == 1)
        {
            target = GameObject.Find("ElixirBaseR").transform;
        }
        else
        {
            target = GameObject.Find("ElixirBaseB").transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        t += Time.deltaTime;

        // Calculate the percentage of the animation time that has elapsed.
        float t1 = t / speed;

        // Use Lerp to smoothly interpolate the game object's position between the start and end positions.
        transform.position = Vector3.Lerp(transform.position, target.position, t);

        if(transform.position == target.position)
        {
            GameObject.Find("InputManager").GetComponent<InputManager>().Add(amount, colour);
            Destroy(gameObject);
        }
    }
}
