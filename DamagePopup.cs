using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class DamagePopup : MonoBehaviour
{
    public int toDisplay;
    public TextMeshPro text1;
    public TextMeshPro text2;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        text1.text = "-"+toDisplay.ToString();
        text2.text = "-"+toDisplay.ToString();
    }
}
