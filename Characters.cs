using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Characters : MonoBehaviour
{
    [Header("Live Positionings")]
    public string position;
    public enum Teams { Blue, Red };
    public Teams team;
    public enum MoveSets { Pawn, Rook, Knight, Bishop, King, Queen, Custom};
    public MoveSets moveSet;
    public string[] possibleMoves;

    [Header("Stats")]
    public int attack;
    public int defense;

    public GameObject statsPopup;
    [Header("Do Not Change")]
    public bool isClicked;
    public GameObject deathEffect;
    private Transform tile;
    public bool isPickedUp;
    public bool isRightClicked;
    public bool isHighlighted;
    public string positionToGrab;
    public Material defaultMat;
    public Material highlightMat;
    public Vector3 statsPopupOffset;
    public Vector3 characterTransformOffset;
    private InputManager inputManager;
    public float pickupHeight;
    public float pickUpSmoothing;
    public GameObject damageText;
    private int moveMult;
    private string previousPosition;
    public GameObject collisionEffect;
    private int[] moveSetString = null;
    public int originalDefense;
    private int defaultDir;
    private Animator anim;
    private bool hasMoved =false;
    public int moveCost = 2;
    private string[] alphabet = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };

    public GameObject[] drops;

    public int colour; // Blue = 0 , Red = 1

    private bool isTeam;

    private GameObject tempStats;
    // Start is called before the first frame update
    void Start()
    {
        positionToGrab = position;
        inputManager = GameObject.Find("InputManager").GetComponent<InputManager>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

        if(GameObject.Find("InputManager").GetComponent<GameManager>().team == team)
        {
            isTeam = true ;
        }
        else
        {
            isTeam = false;
        }
        if (defense <= 0)
        {
            Die();
        }
        // set player position to tile
        tile = GameObject.Find(position).transform;
        if (!isPickedUp)
        {
            transform.position = Vector3.Lerp(transform.position, tile.position + characterTransformOffset, pickUpSmoothing * Time.deltaTime*2);
            if(Vector2.Distance(transform.position,(GameObject.Find(position).transform.position + characterTransformOffset))<=0.1f)
            {
                if (!CheckIfMoveIsValid(position))
                {
                    position = previousPosition;
                    positionToGrab = previousPosition;
                }
                else
                {
                    positionToGrab = position;
                }
                if(GameObject.Find(position).GetComponent<Tile>().characterOnTile != gameObject && GameObject.Find(position).GetComponent<Tile>().characterOnTile != null)
                {
                    if (GameObject.Find(position).GetComponent<Tile>().characterOnTile.GetComponent<Characters>().team == team) { position = previousPosition; }
                    else 
                    {
                            int enemyDamage = GameObject.Find(position).GetComponent<Tile>().characterOnTile.GetComponent<Characters>().attack;
                            int enemyDefense = GameObject.Find(position).GetComponent<Tile>().characterOnTile.GetComponent<Characters>().defense;
                            int myAttack = attack;
                            int myDefense = defense;
                            enemyDefense -= myAttack;
                            myDefense -= enemyDamage;
                            if(myDefense <= 0 && enemyDefense <= 0)
                        {
                            GameObject.Find(position).GetComponent<Tile>().characterOnTile.GetComponent<Characters>().Die();
                            GameObject.Find(position).GetComponent<Tile>().characterOnTile.GetComponent<Characters>().defense = enemyDefense;
                            Die();
                        }
                        else
                        {
                            if (enemyDefense <= 0)
                            {
                                GameObject.Find(position).GetComponent<Tile>().characterOnTile.GetComponent<Characters>().Die();
                                GameObject dmg = Instantiate(damageText, transform.localPosition, Quaternion.identity);
                                dmg.transform.position = transform.position;
                                dmg.GetComponent<DamagePopup>().toDisplay = originalDefense - myDefense;
                            }
                            if (myDefense <= 0)
                            {
                                GameObject.Find(position).GetComponent<Tile>().characterOnTile.GetComponent<Characters>().defense = enemyDefense;
                                GameObject dmg = Instantiate(damageText, transform.localPosition, Quaternion.identity);
                                dmg.transform.position = transform.position;
                                dmg.GetComponent<DamagePopup>().toDisplay = GameObject.Find(position).GetComponent<Tile>().characterOnTile.GetComponent<Characters>().originalDefense - enemyDefense;
                                Die();
                            }
                            GameObject.Find(position).GetComponent<Tile>().characterOnTile.GetComponent<Characters>().defense = enemyDefense;
                            defense = myDefense;
                        }
                    }
                }
                else
                {
                    originalDefense = defense;
                }
            }
        }
        else
        {
            UpdatePickupPosition();
        }
        
        if (isHighlighted)
        {
            GetComponent<SpriteRenderer>().material = highlightMat;
            if (Input.GetAxis("Fire2") > 0)
            {
                OpenStats();
            }
            if(Input.GetAxis("Fire1")>0 && !inputManager.isLeftMouse && isTeam && inputManager.elixir[colour, 0] >= moveCost)
            {
                if (!isClicked)
                {
                    isHighlighted = false;
                    hasMoved = true;
                    isPickedUp = true;
                    anim.SetBool("isPicked", true);
                    inputManager.characterBeingHeld = gameObject;
                    CalculatePossibleMoves();
                    GetComponent<SpriteRenderer>().sortingOrder +=1;
                    
                }
                isClicked = true;
                inputManager.isLeftMouse = true;
            }

        }
        else
        {
            if(Input.GetAxis("Fire1") == 0)
            {
                if (isClicked)
                {
                    previousPosition = position;
                    position = inputManager.tileSelected.name;
                    isPickedUp=false;
                    anim.SetBool("isPicked", false);
                    inputManager.Spend(moveCost,colour);
                    inputManager.characterBeingHeld = null;
                    GetComponent<SpriteRenderer>().sortingOrder -= 1;
                    Instantiate(collisionEffect, inputManager.tileSelected.transform.position + characterTransformOffset, Quaternion.identity);
                }
                isClicked = false;
                GetComponent<SpriteRenderer>().material = defaultMat;
                

            }
                CloseStats();
        }
    }
    void OpenStats()
    {
        if (!tempStats)
        {
            tempStats = Instantiate(statsPopup, transform.position + statsPopupOffset, Quaternion.identity);
            tempStats.GetComponent<Animator>().SetBool("isOpen", true);
        }
        tempStats.GetComponent<StatsPopup>().attack.gameObject.GetComponent<TextMeshPro>().text = attack.ToString();
        tempStats.GetComponent<StatsPopup>().defense.gameObject.GetComponent<TextMeshPro>().text = defense.ToString();
    }
    void CloseStats()
    {
        if (tempStats)
        {
            tempStats.GetComponent<Animator>().SetBool("isOpen", false);
        }
    }
    void UpdatePickupPosition()
    {
        Vector3 targetPosition =  inputManager.tileSelected.transform.position + new Vector3(0, pickupHeight, Camera.main.ScreenToWorldPoint(Input.mousePosition).z*-1);
        transform.position = Vector3.Lerp(transform.position, targetPosition, pickUpSmoothing * Time.deltaTime);
    }
    public void Die()
    {
        Instantiate(deathEffect, inputManager.tileSelected.transform.position + characterTransformOffset, Quaternion.identity);
        if(team == Teams.Blue)
        {
            GameObject drop = Instantiate(drops[1], transform.position, transform.rotation);
            drop.GetComponent<ElixirDrop>().amount = moveCost;

        }
        else
        {
            GameObject drop = Instantiate(drops[0], transform.position, transform.rotation);
            drop.GetComponent<ElixirDrop>().amount = moveCost;
        }
        Destroy(gameObject);
    }
    public void CalculatePossibleMoves()
    {
        switch (moveSet)
        {
            case MoveSets.Pawn:
                moveSetString = new int[4] { 1, 1, 1, -1 };
                moveMult = 0;
                break;
            case MoveSets.Knight:
                moveSetString = new int[16]{1,2,2,1,2,-1,1,-2,-1,-2,-2,-1,-2,1,-1,2};
                moveMult = 0;
                break;
           case MoveSets.King:
                moveSetString = new int[16] {0,1,1,1,1,0,1,-1,0,-1,-1,-1,-1,0,-1,1};
                moveMult = 0;
                break;
            case MoveSets.Queen:
                moveSetString = new int[16] {0,1,1,1,1,0,1,-1,0,-1,-1,-1,-1,0,-1,1};
                moveMult = 4;
                break;
            case MoveSets.Bishop:
                moveSetString = new int[8] { 1,1,1,-1,-1,-1,-1,1};
                moveMult = 4;
                break;
            case MoveSets.Rook:
                moveSetString = new int[8] { 0,1,1,0,-1,0,0,-1};
                moveMult = 4;
                break;
        }
        if (moveMult > 0)
        {
            possibleMoves = new string[moveSetString.Length*moveMult];
            int y = 0;
            for (int x = 0; x < moveSetString.Length; x += 2)
            {
                Debug.Log(x);
                for (int z = 1; z <= moveMult; z++)
                {
                    int letterIndex1 = System.Array.IndexOf(alphabet, position[0].ToString()) + moveSetString[x]*z;
                    if (letterIndex1 >= 0 && letterIndex1 < alphabet.Length)
                    {
                        string letter = alphabet[System.Array.IndexOf(alphabet, position[0].ToString()) + moveSetString[x]*z];
                        string number = (int.Parse(position[1].ToString()) + moveSetString[x + 1]*z).ToString();
                        if (GameObject.Find(letter + number.ToString()) != null)
                        {
                            if (GameObject.Find(letter + number.ToString()).GetComponent<Tile>().characterOnTile == null || GameObject.Find(letter + number.ToString()).GetComponent<Tile>().characterOnTile.GetComponent<Characters>().team != team)
                            {
                                possibleMoves[y] = letter + number.ToString();
                                y++;
                                if(GameObject.Find(letter + number.ToString()).GetComponent<Tile>().characterOnTile != null)
                                {
                                    break;
                                }
                            }
                            else
                            {
                                break;
                            }

                        }
                        else
                        {
                            break;
                        }

                    }

                }
            }
        }
        else
        {
            if(team == Teams.Blue)
            {
                defaultDir = -1;
            }
            else
            {
                defaultDir = 1;
            }
            possibleMoves = new string[moveSetString.Length / 2];
            int y = 0;
            for (int x = 0; x < moveSetString.Length; x += 2)
            {
                Debug.Log(x);
                int letterIndex = System.Array.IndexOf(alphabet, position[0].ToString()) + moveSetString[x]*defaultDir;
                if (letterIndex > 0 && letterIndex < alphabet.Length)
                {
                    string letter = alphabet[System.Array.IndexOf(alphabet, position[0].ToString()) + moveSetString[x] * defaultDir   ];
                    string number = (int.Parse(position[1].ToString()) + moveSetString[x + 1]).ToString();
                    if (GameObject.Find(letter + number.ToString()) != null)
                    {
                        if (GameObject.Find(letter + number.ToString()).GetComponent<Tile>().characterOnTile == null || GameObject.Find(letter + number.ToString()).GetComponent<Tile>().characterOnTile.GetComponent<Characters>().team != team)
                        {
                            possibleMoves[y] = letter + number.ToString();
                        }

                    }
                }
                y++;
            }
        }
        foreach(string move in possibleMoves)
        {
            if (move != null) { GameObject.Find(move).GetComponent<Tile>().isPossible = true; }
        }
    }
    public bool CheckIfMoveIsValid(string move)
    {
        if (!hasMoved)
        {
            return true;
        }
        foreach (string moves in possibleMoves)
        {
            if(moves == move)
            {
                return true;
            }
        }
        return false;
    }
}
