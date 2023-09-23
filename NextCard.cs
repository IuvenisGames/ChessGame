using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class NextCard : MonoBehaviour
{
    public CardInfo cardInformation;
    public enum Cards { Pawn, Knight, Bishop, Rook, King, Queen, Meteor, Empty };
    public Cards card;
    public enum Teams { Blue, Red };
    public Teams team;
    public int index;

    [Header("Do Not Change")]

    public GameObject collisionEffect;
    public Transform collisionPosition;
    public Animator anim;
    public Animator characterAnim;

    public bool cardIsEmpty = false;
    public GameObject platformToGo;

    public GameObject[] platforms;
    public GameObject labelPosition;
    private Transform labelTemp;
    private Vector3 targetPosition;
    private float pickUpSmoothing = 15;
    private InputManager inputManager;



    [Serializable]
    public class CardInfo
    {
        [SerializeField] public string name;
        [SerializeField] public GameObject spawnable;
        [SerializeField] public RuntimeAnimatorController animatorPath;
        [SerializeField] public int index;
    }
    [SerializeField] public CardInfo[] cardsInformation;


    // Start is called before the first frame update
    void Start()
    {
        index = UnityEngine.Random.Range(0, System.Enum.GetValues(typeof(Cards)).Length-1);
        inputManager = GameObject.Find("InputManager").GetComponent<InputManager>();
        team = (Teams)(int)inputManager.team;
        anim = GetComponent<Animator>();
        RandomCard();
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
        if (cardIsEmpty == false)
        {
            foreach (GameObject platform in platforms)
            {
                Card card = platform.GetComponent<Card>();
                if (platform.GetComponent<Card>().cardInformation.name == "Empty")
                {
                    platformToGo = platform;
                    cardIsEmpty = true;
                    InstantiateSpawner();
                    break;
                }
            }
        }
        else
        {
            targetPosition = platformToGo.transform.position;

            if (labelTemp) { labelTemp.transform.position = Vector3.Lerp(labelTemp.transform.position, targetPosition, pickUpSmoothing * Time.deltaTime); }

            if (labelTemp && Vector2.Distance(labelTemp.position, targetPosition) <= 0.05f)
            {
                DestroySpawner();
                SetAnim();
            }
        }


        switch (card)
        {
            case Cards.Pawn:
                if (team == Teams.Blue) { cardInformation = cardsInformation[0]; } else { cardInformation = cardsInformation[6]; }
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
            case Cards.Empty:
                cardInformation = cardsInformation[14];
                break;
            case Cards.Meteor:
                if (team == Teams.Blue) { cardInformation = cardsInformation[12]; } else { cardInformation = cardsInformation[13]; }
                break;
        }
        SetComponents();
    }
    public void SetComponents()
    {
        characterAnim = labelPosition.GetComponent<Animator>();
        characterAnim.runtimeAnimatorController = cardInformation.animatorPath;
    }
    public void SetAnim()
    {
        anim.SetTrigger("spawn");
    }
    public void SpawnCollision()
    {
        Instantiate(collisionEffect, collisionPosition.position, Quaternion.identity);
    }
    public void InstantiateSpawner()
    {
        labelTemp = Instantiate(labelPosition, transform.position, Quaternion.identity).transform;
        labelPosition.GetComponent<SpriteRenderer>().enabled = false;
    }
    public void DestroySpawner()
    {
        RandomCard();
        platformToGo.GetComponent<Card>().Switch(cardInformation.name);
        Destroy(labelTemp.gameObject);
        labelPosition.GetComponent<SpriteRenderer>().enabled = true;
    }
    public void DoNext()
    {
        cardIsEmpty = false;
    }
    public void RandomCard()
    {
        card = (Cards)index;
        index++;
        if(index>= System.Enum.GetValues(typeof(Cards)).Length - 2)
        {
            index = 0;
        }
    }
}
