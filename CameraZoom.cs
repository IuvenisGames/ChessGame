using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    public Vector2 originalResolution;
    public float zoom = 1;
    public Vector2 aspectRatio;

    public UnityEngine.Experimental.Rendering.Universal.PixelPerfectCamera ppc;
    // Start is called before the first frame update
    void Start()
    {
        originalResolution = new Vector2(ppc.refResolutionX, ppc.refResolutionY);
    }

    // Update is called once per frame
    void Update()
    {
        ppc.refResolutionX = Mathf.RoundToInt(originalResolution.x / zoom);
        ppc.refResolutionY = Mathf.RoundToInt(originalResolution.y / zoom);
    }
}
