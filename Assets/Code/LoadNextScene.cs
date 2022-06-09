using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadNextScene : MonoBehaviour
{
    public string sceneToLoad = "Name";

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Loading Scene: " + sceneToLoad);
        LoadScene(sceneToLoad);

    }

    private void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

}
