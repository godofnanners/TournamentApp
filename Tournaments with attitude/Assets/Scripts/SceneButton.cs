using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneButton : MonoBehaviour
{
    [SerializeField] string mySceneToLoadOnClick;

    public void LoadScene()
    {
        SceneManager.LoadScene(mySceneToLoadOnClick);
    }
}
