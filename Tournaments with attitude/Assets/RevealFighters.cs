using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RevealFighters : MonoBehaviour
{

    public GameObject myLeftFighter;
    public GameObject myRightFighter;

    public GameObject myLeftSplatter;
    public GameObject myRightSplatter;

    // Start is called before the first frame update
    void Start()
    {

    }

    public void OnRevealFighters()
    {
        myLeftSplatter.SetActive(true);
        myRightSplatter.SetActive(true);
        myLeftFighter.SetActive(true);
        myRightFighter.SetActive(true);
    }
}
