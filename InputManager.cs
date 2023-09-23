using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public enum Teams { Blue, Red};
    public int[,] elixir;
    public Teams team;
    public GameObject tileSelected;
    public GameObject characterBeingHeld;
    public bool isLeftMouse;

    [Header("LiveMovements")]
    public string currentTile;
    public string targetTile;
    // Start is called before the first frame update
    void Start()
    {
        elixir = new int[2,2];
        elixir[0, 0] = 5;
        elixir[0, 1] = 15;
        elixir[1, 1] = 15;
        elixir[1, 0] = 5;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxis("Fire1") == 0)
        {
            isLeftMouse = false;
        }
        if (tileSelected && !tileSelected.GetComponent<Tile>().isActive)
        {
            tileSelected = null;
        }
        elixir[0,0] = Mathf.Clamp(elixir[0, 0], 0, elixir[0, 1]);
        elixir[1,0] = Mathf.Clamp(elixir[1, 0], 0, elixir[1, 1]);
    }
    public void Spend(int amount, int colour)
    {
        elixir[colour, 0] -= amount;
    }
    public void Add(int amount, int colour)
    {
        elixir[colour,0] += amount;
    }

    public void UpdateBoard()
    {

    }
}
