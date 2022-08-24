using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClosingPanel : MonoBehaviour
{
    public enum ePanelState
    {
        Closing,
        Opening,
        Resting
    }

    


    [SerializeField] private ePanelState myPanelState;
    [SerializeField] private float myCloseAcceleration = 1;
    [SerializeField] private float myOpenAcceleration = 1;
    private float myCloseSpeed = 0;
    private float myOpenSpeed = 0;
    [SerializeField] private float myDistanceEqualToZero = 0.1f;

    private Vector3 myOpenPos = new Vector3(0, 0, 0);
    private RectTransform myRectTransform;


    // Start is called before the first frame update
    void Start()
    {
        myRectTransform = GetComponent<RectTransform>();
        myOpenPos = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        switch (myPanelState)
        {
            case ePanelState.Closing:
                myCloseSpeed += myCloseAcceleration * Time.deltaTime;
                myRectTransform.anchoredPosition = Vector2.MoveTowards(myRectTransform.anchoredPosition, new Vector2(0, myRectTransform.anchoredPosition.y), myCloseSpeed * Time.deltaTime);

                if (Mathf.Abs(transform.position.x) <= myDistanceEqualToZero)
                {
                    myCloseSpeed = 0;
                    myRectTransform.anchoredPosition = new Vector2(0, myRectTransform.anchoredPosition.y);
                    myPanelState = ePanelState.Resting;
                }

                break;
            case ePanelState.Opening:
                myOpenSpeed += myOpenAcceleration * Time.deltaTime;
                myRectTransform.anchoredPosition = Vector3.MoveTowards(myRectTransform.anchoredPosition, new Vector3(0, myRectTransform.anchoredPosition.y, 0), myOpenSpeed * Time.deltaTime);

                if (Mathf.Abs(transform.localPosition.x - myOpenPos.x) <= myDistanceEqualToZero)
                {
                    myOpenSpeed = 0;
                    myRectTransform.anchoredPosition = myOpenPos;
                    myPanelState = ePanelState.Resting;
                }
                break;
            case ePanelState.Resting:
                break;
            default:
                break;
        }
    }

    public void SetPanelStateToClosing()
    {
        myPanelState = ePanelState.Closing;
    }

    public void SetPanelStateToOpening()
    {
        myPanelState = ePanelState.Opening;
    }
}
