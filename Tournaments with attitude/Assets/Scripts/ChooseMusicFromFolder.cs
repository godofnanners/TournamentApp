using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class ChooseMusicFromFolder : MonoBehaviour
{
    private AudioSource myAudioSource;
    private string[] myAudioClipPaths;
    public Text mySongTitel;
    private List<int> myUnPlayedSongIndex;
    private AudioClip[] myLoadedAudioClips;

    public bool myShouldPlay = false;

    private int myAudioIndex = 0;
    // Start is called before the first frame update
    private void Awake()
    {
        myAudioSource = GetComponent<AudioSource>();
        myUnPlayedSongIndex = new List<int>();

        myLoadedAudioClips = Resources.LoadAll<AudioClip>("Music");

        myAudioClipPaths = Directory.GetFiles(Application.streamingAssetsPath + "/Music", "*.mp3", SearchOption.AllDirectories);

        for (int i = 0; i < myAudioClipPaths.Length; i++)
        {
            //int indexMp3 = myAudioClipPaths[i].IndexOf(".");
            //myAudioClipPaths[i] = myAudioClipPaths[i].Remove(indexMp3, 4);
            int index = myAudioClipPaths[i].IndexOf("Music");
            myAudioClipPaths[i] = myAudioClipPaths[i].Remove(0, index - 1);
            myAudioClipPaths[i] = myAudioClipPaths[i].Replace('\\', '/');

            myAudioClipPaths[i] = Path.Combine(Application.streamingAssetsPath + myAudioClipPaths[i]);
        }



        for (int i = 0; i < myAudioClipPaths.Length; i++)
        {
            myUnPlayedSongIndex.Add(i);
        }

        //myAudioSource.clip = LoadAudioClip();
        StartCoroutine(MusicPlayer(false));
    }

    // Update is called once per frame
    void Update()
    {
        if (myShouldPlay)
        {
            if (!myAudioSource.isPlaying || Input.GetKeyDown(KeyCode.F12))
            {
                StartCoroutine(MusicPlayer(true));

                if (myUnPlayedSongIndex.Count <= 0)
                {
                    for (int i = 0; i < myAudioClipPaths.Length; i++)
                    {
                        myUnPlayedSongIndex.Add(i);
                    }
                }
            }
        }
    }

    IEnumerator MusicPlayer(bool aShouldPlay)
    {
        string filepath = GetAudioFileLocation();

        using (UnityWebRequest uwr = UnityWebRequestMultimedia.GetAudioClip(filepath, AudioType.MPEG))
        {
            yield return uwr.SendWebRequest();

            if (uwr.isNetworkError || uwr.isHttpError)
            {
                Debug.Log(uwr.error);
            }
            else
            {
                if (myAudioSource.clip != null)
                {
                    myAudioSource.Stop();
                    AudioClip currentClip = myAudioSource.clip;
                    myAudioSource.clip = null;
                    currentClip.UnloadAudioData();
                    Destroy(currentClip);
                }

                myAudioSource.clip = DownloadHandlerAudioClip.GetContent(uwr);



                mySongTitel.text = GetFileNameFromFilePath(filepath);

                if (aShouldPlay)
                {
                    myAudioSource.Play();
                }
                yield return null;
            }
        }
    }

    private string GetFileNameFromFilePath(string filePath)
    {
        string returnString = filePath;

        int indexFormat = filePath.IndexOf(".");

        if (indexFormat != -1)
        {
            returnString = returnString.Remove(indexFormat, 4);

        }
        int index = returnString.LastIndexOf("/");
        if (index != -1)
        {
            returnString = returnString.Substring(index + 1);
        }

        return returnString;
    }

    private string GetAudioFileLocation()
    {
        string returnPath = "";

        int indexToPick = Random.Range(0, myUnPlayedSongIndex.Count - 1);

        int songIndex = myUnPlayedSongIndex[indexToPick];
        myUnPlayedSongIndex.RemoveAt(indexToPick);

        returnPath = myAudioClipPaths[songIndex];

        return returnPath;
    }

    private AudioClip LoadAudioClip()
    {
        int indexToPick = Random.Range(0, myUnPlayedSongIndex.Count - 1);

        int songIndex = myUnPlayedSongIndex[indexToPick];
        myUnPlayedSongIndex.RemoveAt(indexToPick);
        AudioClip audioClip;
        using (UnityWebRequest uwr = UnityWebRequestMultimedia.GetAudioClip(myAudioClipPaths[songIndex], AudioType.MPEG))
        {
            audioClip = DownloadHandlerAudioClip.GetContent(uwr);
        }

        return audioClip;
    }

    public void OnPauseOrPlay()
    {
        if (myAudioSource.isPlaying)
        {
            myAudioSource.Pause();
        }
        else
        {
            StartCoroutine(StartSongWithDelay(0.7f));
        }
    }

    private IEnumerator StartSongWithDelay(float aDelay)
    {
        if (mySongTitel != null)
        {
            mySongTitel.text = myAudioSource.clip.name;
        }

        yield return new WaitForSeconds(aDelay);

        myAudioSource.Play();
        myShouldPlay = !myShouldPlay;

    }

    public void StartSong()
    {
        myShouldPlay = true;
        myAudioSource.Play();
    }
}
