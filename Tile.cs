using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public GameObject characterOnTile;
    private Animator anim;
    public bool isActive;
    public bool isPossible;

    public GameObject[] characters;

    private InputManager inputManager;

    private float t = 0.2f; // Scan Characters every second

    void Start()
    {
        anim = GetComponent<Animator>();
        inputManager = GameObject.Find("InputManager").GetComponent<InputManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isActive)
        {
            if (characterOnTile && !inputManager.isLeftMouse) { characterOnTile.GetComponent<Characters>().isHighlighted = true; }
        }
        if (isPossible)
        {
            if(inputManager.characterBeingHeld == null)
            {
                isPossible = false;
            }
        }
        anim.SetBool("isActive", isActive);
        anim.SetBool("isPossible", isPossible);
        t += Time.deltaTime;
        if (t > 0.1f)
        {
            characters = GameObject.FindGameObjectsWithTag("Character");
            characterOnTile = null;
            foreach (GameObject character in characters)
            {
                if (character.GetComponent<Characters>().positionToGrab == gameObject.name)
                {
                    characterOnTile = character;
                }
            }
        }
    }
    void OnMouseEnter()
    {
        isActive= true;
        inputManager.tileSelected = gameObject;
        if (characterOnTile && !inputManager.isLeftMouse) { characterOnTile.GetComponent<Characters>().isHighlighted = true; }
    }
    void OnMouseStay()
    {
        isActive = true;
        inputManager.tileSelected = gameObject;
        if (characterOnTile && !inputManager.isLeftMouse) { characterOnTile.GetComponent<Characters>().isHighlighted = true; }
    }
    void OnMouseExit()
    {
        isActive = false;
        if (characterOnTile) { characterOnTile.GetComponent<Characters>().isHighlighted = false; }
    }
}
