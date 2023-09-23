using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Vector2 bounds;
    private Vector3 origin;

    public float shakeDamp;

    public  float shakeAmount;
    private Vector3 originalPos;
    private Vector3 targetPos;
    // Start is called before the first frame update
    void Start()
    {
        origin = transform.position;
    }

    // Update is called once per frame
    void Update()
    {

            float screenHeight = Screen.height;
            float screenWidth = Screen.width;

            float xMovement = (Input.mousePosition.x / screenWidth) * bounds.x;
            float yMovement = (Input.mousePosition.y / screenHeight) * bounds.y;

            targetPos = origin + new Vector3(xMovement, yMovement, 0);

            transform.localPosition = targetPos + Random.insideUnitSphere * shakeAmount;
            shakeAmount = Mathf.Lerp(shakeAmount, 0, Time.deltaTime * shakeDamp);
    }
    public void Shake(float shake)
    {
        shakeAmount = shake;
        originalPos = transform.position;
    }
}
