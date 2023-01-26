using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System;

public class ChooseMusicFromFolder : MonoBehaviour
{
    private AudioSource myAudioSource;
    private AudioClip myAudioClip;
    private string[] myAudioClipPaths;
    public Text mySongTitel;
    private List<int> myUnPlayedSongIndex;
    private AudioClip[] myLoadedAudioClips;
    Queue<AudioClip> myAduioClips = new Queue<AudioClip>();

    public bool myShouldPlay = false;

    private int myAudioIndex = 0;
    // Start is called before the first frame update
    private void Awake()
    {
        myAudioSource = GetComponent<AudioSource>();
        myUnPlayedSongIndex = new List<int>();

        myLoadedAudioClips = Resources.LoadAll<AudioClip>("Music");

        myAudioClipPaths = Directory.GetFiles(Application.streamingAssetsPath + "/Music", "*.mp3", SearchOption.AllDirectories);

        //for (int i = 0; i < myAudioClipPaths.Length; i++)
        //{
        //    int indexMp3 = myAudioClipPaths[i].IndexOf(".");
        //    myAudioClipPaths[i] = myAudioClipPaths[i].Remove(indexMp3, 4);
        //    int index = myAudioClipPaths[i].IndexOf("Music");
        //    myAudioClipPaths[i] = myAudioClipPaths[i].Remove(0, index - 1);
        //    myAudioClipPaths[i] = myAudioClipPaths[i].Replace('\\', '/');

        //    myAudioClipPaths[i] = Path.Combine(Application.streamingAssetsPath + myAudioClipPaths[i]);
        //}

        for (int i = 0; i < myAudioClipPaths.Length; i++)
        {
            myUnPlayedSongIndex.Add(i);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (myShouldPlay)
        {
            if (!myAudioSource.isPlaying || Input.GetKeyDown(KeyCode.F12))
            {
                int songIndex;
                string filepath = GetRandomSongFileLocation(out songIndex);
                StartCoroutine(LoadMusic(filepath, (clip) => { PlayMusic(clip); }));
                RemoveSongIndexFromUnplayedSongs(songIndex);
                mySongTitel.text = GetFileNameFromFilePath(filepath);

                if (myUnPlayedSongIndex.Count <= 0)
                {
                    RefillUnplayedSongIndexList();
                }
            }
        }
    }

    private void PlayMusic(AudioClip anAudioClip)
    {
        if (myAudioSource.clip != null)
        {
            myAudioSource.Stop();
            AudioClip currentClip = myAudioSource.clip;
            myAudioSource.clip = null;
            currentClip.UnloadAudioData();
            Destroy(currentClip);
        }

        myAudioSource.clip = anAudioClip;
        myAudioSource.Play();
    }

    private IEnumerator LoadMusic(string filePath, Action<AudioClip> onLoadingCompleted)
    {
        using (UnityWebRequest request = UnityWebRequestMultimedia.GetAudioClip(filePath, AudioType.MPEG))
        {
            yield return request.SendWebRequest();

            if (request.isHttpError)
            {
                Debug.LogWarning(request.error + "\n" + filePath);
            }
            else
            {
                onLoadingCompleted(DownloadHandlerAudioClip.GetContent(request));
                // Assign to audio source here.
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
        int index = returnString.LastIndexOf("\\");
        if (index != -1)
        {
            returnString = returnString.Substring(index + 1);
        }

        return returnString;
    }

    private string GetRandomSongFileLocation(out int aReturnSongIndex)
    {
        string returnPath = "";

        aReturnSongIndex = UnityEngine.Random.Range(0, myUnPlayedSongIndex.Count - 1);

        int songIndex = myUnPlayedSongIndex[aReturnSongIndex];
        myUnPlayedSongIndex.RemoveAt(aReturnSongIndex);

        returnPath = myAudioClipPaths[songIndex];

        return returnPath;
    }

    private bool RemoveSongIndexFromUnplayedSongs(int aSongIndex)
    {
        if (aSongIndex < myUnPlayedSongIndex.Count)
        {
            myUnPlayedSongIndex.RemoveAt(aSongIndex);
            return true;
        }
        else
            return false;
    }

    private void RefillUnplayedSongIndexList()
    {
        for (int i = 0; i < myAudioClipPaths.Length; i++)
        {
            myUnPlayedSongIndex.Add(i);
        }
    }

    public void StartSong()
    {
        myShouldPlay = true;
        myAudioSource.Play();
    }
}
