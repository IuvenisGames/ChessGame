using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationFunctions : MonoBehaviour
{
    public float shakeAmount;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void DestroyObject()
    {
        Destroy(gameObject);
    }
    public void ShakeCamera()
    {
        Camera.main.gameObject.GetComponent<CameraMovement>().Shake(shakeAmount);
    }
}
