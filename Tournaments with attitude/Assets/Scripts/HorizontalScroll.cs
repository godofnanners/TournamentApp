using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorizontalScroll : MonoBehaviour
{

    public enum ScrollDirection
    {
        Right,
        Left
    }

    public ScrollDirection myScrollDirection = ScrollDirection.Right;
    public float myScrollspeed = 1f;
    public float myDistanceThreshhold=2000f;

    private Vector3 myStartPos;
    // Start is called before the first frame update
    void Start()
    {
        myStartPos = transform.position;
        switch (myScrollDirection)
        {
            case ScrollDirection.Right:
                transform.position = new Vector3(transform.position.x - Screen.width, transform.position.y, transform.position.z);
                break;
            case ScrollDirection.Left:
                transform.position = new Vector3(transform.position.x + Screen.width, transform.position.y, transform.position.z);

                break;
            default:
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        float loopingDistance=myDistanceThreshhold + Screen.width;
        switch (myScrollDirection)
        {
            case ScrollDirection.Right:
                transform.position = new Vector3( transform.position.x + myScrollspeed * Time.deltaTime, transform.position.y, transform.position.z);

                if (loopingDistance < Vector3.Distance(transform.position, myStartPos))
                {
                    transform.position = new Vector3(myStartPos.x - loopingDistance + 1, transform.position.y, transform.position.z);
                }
                break;
            case ScrollDirection.Left:
                transform.position = new Vector3(transform.position.x - myScrollspeed * Time.deltaTime, transform.position.y, transform.position.z);

                if (loopingDistance < Vector3.Distance(transform.position,myStartPos))
                {
                    transform.position = new Vector3(myStartPos.x + loopingDistance - 1, transform.position.y, transform.position.z);
                }
                break;
            default:
                break;
        }
    }
}
