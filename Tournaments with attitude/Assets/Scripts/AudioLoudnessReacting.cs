using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioLoudnessReacting : MonoBehaviour
{
    public AudioSource myAudioSource;
    public float myUpdateStep = 0.01f;
    public int sampleDataLenght = 1024;


    private float[] clipSampleData;

    public GameObject sprite;
    private float myCurrentScale;
    private float myOriginalScale;
    public float myScaleSpeed = 1f;
    public float sizeFactor = 1;


    private float minSize;
    public float maxSize = 500f;

    // Start is called before the first frame update
    private void Awake()
    {
        myOriginalScale = sprite.transform.localScale.x;
        minSize = sprite.transform.localScale.x;
        myCurrentScale = sprite.transform.localScale.x;
        clipSampleData = new float[sampleDataLenght];
    }

    // Update is called once per frame
    private void Update()
    {
        if (myAudioSource.clip != null)
        {
            float clipLoudness = 0f;

            myAudioSource.clip.GetData(clipSampleData, myAudioSource.timeSamples);

            foreach (var sample in clipSampleData)
            {
                clipLoudness += Mathf.Abs(sample);
            }
            clipLoudness /= sampleDataLenght;

            clipLoudness *= sizeFactor;

            clipLoudness += myOriginalScale;

            if (myCurrentScale < clipLoudness)
            {
                myCurrentScale += myScaleSpeed * Time.deltaTime;
            }
            else if (myCurrentScale > clipLoudness)
            {
                myCurrentScale -= myScaleSpeed * Time.deltaTime;
            }

            myCurrentScale = Mathf.Clamp(myCurrentScale, minSize, maxSize);


            sprite.transform.localScale = new Vector3(myCurrentScale, myCurrentScale, myCurrentScale);

        }

    }
}
