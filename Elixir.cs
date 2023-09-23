using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elixir : MonoBehaviour
{

    public int colour; //Red = 1, Blue = 0
    public int maxElixir;
    public int elixir;

    public Transform elixirDisplay;

    public Vector2 yBounds;

    private InputManager inputManager;

    [Header("Do Not Change")]
    public float smoothing;
    // Start is called before the first frame update
    void Start()
    {
        inputManager = GameObject.Find("InputManager").GetComponent<InputManager>();
    }

    // Update is called once per frame
    void Update()
    {
        elixir = inputManager.elixir[colour,0];
        float perc = ((float)elixir / (float)maxElixir);
        float yBound = yBounds.y - yBounds.x;
        float axis = yBound *perc;
        Vector3 targetPosition = new Vector3(0,yBounds.x+axis,0);

        elixir = Mathf.Clamp(elixir,0, maxElixir);
        
        elixirDisplay.transform.localPosition = Vector3.Lerp(elixirDisplay.transform.localPosition, targetPosition, smoothing * Time.deltaTime); 
    }
    public void AddElixir(int amount)
    {
        inputManager.Add(amount,colour);
    }
}
