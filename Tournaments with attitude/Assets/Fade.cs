using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Fade : MonoBehaviour
{
    public enum eSplatterPhase
    {
        Splatting,
        Fading
    }

    private float myAlpha = 1f;
    public float myFadeSpeed = 2f;
    public float myScaleSpeed = 2f;
    public Image myImage;
    [SerializeField] private eSplatterPhase mySplatterPhase;
    private Vector3 myOriginalSize;
    // Start is called before the first frame update
    void Start()
    {
        myAlpha = 1;
        myImage.color = new Color(myImage.color.r, myImage.color.g, myImage.color.b, myAlpha);
        myOriginalSize = transform.localScale;
        transform.localScale = transform.localScale * 0.85f;
        
    }

    // Update is called once per frame
    void Update()
    {
        switch (mySplatterPhase)
        {
            case eSplatterPhase.Splatting:
                transform.localScale = Vector3.LerpUnclamped(transform.localScale, myOriginalSize, myScaleSpeed * Time.deltaTime);
                if (myOriginalSize.magnitude - transform.localScale.magnitude < 1f)
                {
                    mySplatterPhase = eSplatterPhase.Fading;
                }
                break;
            case eSplatterPhase.Fading:
                myAlpha -= Time.deltaTime * myFadeSpeed;

                myImage.color = new Color(myImage.color.r, myImage.color.g, myImage.color.b, myAlpha);
                break;
            default:
                break;
        }

    }
}
