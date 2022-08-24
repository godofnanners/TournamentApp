using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateWithDelay : MonoBehaviour
{
    [SerializeField] float[] myTimes = new float[2];

    [SerializeField] GameObject[] myObjects = new GameObject[2];
    float[] myCurrentTimes = new float[2];

    public bool myShouldStartTimer = false;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < myTimes.Length; i++)
        {
            myCurrentTimes[i] = myTimes[i];
        }
    }

    private void Update()
    {
        if (myShouldStartTimer)
        {
            for (int i = 0; i < myTimes.Length; i++)
            {
                myCurrentTimes[i] -= Time.deltaTime;

                if (myCurrentTimes[i] <= 0)
                {
                    myObjects[i].SetActive(true);
                }
            }
        }
    }
    // Update is called once per frame
    public void OnRevealChildren()
    {
        for (int i = 0; i < myTimes.Length; i++)
        {
            myCurrentTimes[i] = myTimes[i];
        }
        myShouldStartTimer = true;
    }
}
