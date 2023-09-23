using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public Characters.Teams team;
    public Animator anim;

    public Animator blueElixir;
    public Animator redElixir;

    private bool isDown;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxis("Jump") > 0)
        {
            if (!isDown)
            {
                Switch();
            }
            isDown = true;
        }
        else
        {
            isDown = false;
        }
    }
    public void Switch()
    {
        if(team == Characters.Teams.Red)
        {
            team = Characters.Teams.Blue;
            anim.SetBool("isRed",false);
            redElixir.SetBool("isActive",true);
            blueElixir.SetBool("isActive",false );
        }
        else
        {
            team = Characters.Teams.Red;
            anim.SetBool("isRed", true);
            redElixir.SetBool("isActive", false);
            blueElixir.SetBool("isActive", true);
        }
    }
}
