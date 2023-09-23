using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Card : MonoBehaviour
{

    public CardInfo cardInformation;
    public enum Cards { Pawn, Knight, Bishop, Rook, King, Queen, Meteor, Empty};
    public Cards card;
    public enum Teams { Blue, Red };
    public Teams team;
    [Header("Do Not Change")]
    private Animator anim;
    public  string cardToSwitchTo;
    private InputManager inputManager;
    public GameObject labelPosition;
    public Material highlightedMat;
    private Material defaultMat;
    public bool isPickedUp;
    public float pickupHeight;
    public float pickUpSmoothing;
    private bool isHovering;
    private Transform labelTemp;
    private Vector3 targetPosition;
    public GameObject collisionEffect;
    public Vector3 characterTransformOffset;
    public string tileToGo;
    private bool isPicked;

    public int colour; // Blue = 0, Red = 1

    [Serializable]
    public class CardInfo
    {
        [SerializeField] public string name;
        [SerializeField] public GameObject spawnable;
        [SerializeField] public RuntimeAnimatorController animatorPath;
        [SerializeField] public int index;
        [SerializeField] public Cards card;
        [SerializeField] public Teams team;
        [SerializeField] public int cost;

    }
    [SerializeField] public CardInfo[] cardsInformation;
    private SpriteRenderer render;
    // Start is called before the first frame update
    void Start()
    {
        inputManager = GameObject.Find("InputManager").GetComponent<InputManager>();
        team = (Teams)(int)inputManager.team;
        defaultMat = labelPosition.GetComponent<SpriteRenderer>().material;
        render = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameObject.Find("InputManager").GetComponent<GameManager>().team == Characters.Teams.Blue)
        {
            team = Teams.Blue;
        }
        else
        {
            team = Teams.Red;
        }
        if(team == Teams.Blue)
        {
            colour = 0;
        }
        else
        {
            colour = 1;
        }
        if (isHovering)
        {
                if (Input.GetAxis("Fire1") > 0 && !inputManager.isLeftMouse && card != Cards.Empty && !isPickedUp)
                {
                    isPickedUp = true;
                    isPicked = true;
                    inputManager.isLeftMouse = true;
                }
        }
        if(isPickedUp && Input.GetAxis("Fire1") > 0)
        {
            if(labelTemp == null)
            {
                InstantiateSpawner();
            }
            else
            {
                if (inputManager.tileSelected)
                {
                    targetPosition = inputManager.tileSelected.transform.position + new Vector3(0, pickupHeight, Camera.main.ScreenToWorldPoint(Input.mousePosition).z * -1);
                }
                else
                {
                    targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition) + new Vector3(0, pickupHeight, Camera.main.ScreenToWorldPoint(Input.mousePosition).z * -1);
                }

                labelTemp.transform.position = Vector3.Lerp(labelTemp.transform.position, targetPosition, pickUpSmoothing * Time.deltaTime);
            }
        }
        else if (isPickedUp)
        {
            if (inputManager.tileSelected && isPicked && inputManager.tileSelected.GetComponent<Tile>().characterOnTile == null && inputManager.elixir[colour,0] >= cardInformation.cost)
            {
                tileToGo = inputManager.tileSelected.name;
                inputManager.Spend(cardInformation.cost,colour);
                targetPosition = inputManager.tileSelected.transform.position;
                isPicked = false;
                DestroySpawner();
            }
            else
            {
                if (isPicked) { targetPosition = transform.position; isPicked = false; }
                labelTemp.transform.position = Vector3.Lerp(labelTemp.transform.position, targetPosition, pickUpSmoothing * Time.deltaTime);
                if (Vector2.Distance(labelTemp.position, targetPosition) <= 0.05f)
                {
                    DestroySpawner();
                }
            }
        }
        else
        {
            isPicked = false;
            isPickedUp = false;
        }



        SetComponents();
        switch (card)
        {
            case Cards.Pawn:
                if(team == Teams.Blue) { cardInformation = cardsInformation[0]; } else { cardInformation = cardsInformation[6]; }
                break;
            case Cards.Knight:
                if (team == Teams.Blue) { cardInformation = cardsInformation[1]; } else { cardInformation = cardsInformation[7]; }
                break;
            case Cards.Bishop:
                if (team == Teams.Blue) { cardInformation = cardsInformation[2]; } else { cardInformation = cardsInformation[8]; }
                break;
            case Cards.Rook:
                if (team == Teams.Blue) { cardInformation = cardsInformation[3]; } else { cardInformation = cardsInformation[9]; }
                break;
            case Cards.King:
                if (team == Teams.Blue) { cardInformation = cardsInformation[4]; } else { cardInformation = cardsInformation[10]; }
                break;
            case Cards.Queen:
                if (team == Teams.Blue) { cardInformation = cardsInformation[5]; } else { cardInformation = cardsInformation[11]; }
                break;
            case Cards.Meteor:
                if (team == Teams.Blue) { cardInformation = cardsInformation[12]; } else { cardInformation = cardsInformation[13]; }
                break;
            case Cards.Empty:
                cardInformation = cardsInformation[14];
                break;
        }
    }
    void OnMouseEnter()
    {
        anim.SetBool("isRaised", true);
        labelPosition.GetComponent<SpriteRenderer>().material = highlightedMat;
        isHovering = true;
    }
    void OnMouseExit()
    {
        anim.SetBool("isRaised", false);
        labelPosition.GetComponent<SpriteRenderer>().material = defaultMat;
        isHovering = false;
    }
    public void SetComponents()
    {
        if(card == Cards.Empty)
        {
            Animator animator = labelPosition.GetComponent<Animator>();
            animator.runtimeAnimatorController = null;
        }
        else
        {
            Animator animator = labelPosition.GetComponent<Animator>();
            animator.runtimeAnimatorController = cardInformation.animatorPath;
        }
    }
    public void InstantiateSpawner()
    {
        labelTemp = Instantiate(labelPosition, transform.position, Quaternion.identity).transform;
        labelPosition.GetComponent<SpriteRenderer>().enabled = false;
        anim.enabled = false;
    }
    public void DestroySpawner()
    {
        if(tileToGo != "")
        {
            GameObject character = Instantiate(cardInformation.spawnable, labelTemp.position, Quaternion.identity);
            character.GetComponent<Characters>().position = tileToGo;
            Instantiate(collisionEffect, labelTemp.position + characterTransformOffset, Quaternion.identity);
            card = Cards.Empty;
        }
        else
        {
            Instantiate(collisionEffect, labelTemp.position, Quaternion.identity);
        }
        anim.enabled = true;
        Destroy(labelTemp.gameObject);
        labelTemp = null;
        isPickedUp = false;
        labelPosition.GetComponent<SpriteRenderer>().enabled = true;
        anim.SetBool("isRaised", false);
        labelPosition.GetComponent<SpriteRenderer>().material = defaultMat;
        isHovering = false;
    }
    public void Switch(string cardTo)
    {
        isPicked = false;
        tileToGo = "";
        foreach(CardInfo card1 in cardsInformation)
        {
            if(card1.name == cardTo)
            {
                card = card1.card;
                team = card1.team;
            }
        }
    }
}
