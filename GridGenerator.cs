using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridGenerator : MonoBehaviour
{
    public Vector2 spriteSize;
    public float columnPadding;
    public float rowPadding;
    public float pixelSize = 0.0625f;
    public GameObject[] tiles;

    public int rows;
    public int columns;
    // Start is called before the first frame update
    void Start()
    {
        GenerateGrid();
    }

    // Update is called once per frame
    void Update()
    {
    }
    void GenerateGrid()
    {
        for (int row = 0; row < rows; row++)
        {
            for (int column = 0; column < columns; column++)
            {
                Instantiate(tiles[Random.Range(0, tiles.Length - 1)], transform.position + new Vector3((column *spriteSize.x)+(column*columnPadding*pixelSize), (row * spriteSize.y) + (row * rowPadding*pixelSize), 0), Quaternion.identity);
            }
        }
    }
}
