using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerScript : MonoBehaviour
{
    // Start is called before the first frame update
    public void LoadingScene(string sceneName)
    {
        Debug.Log("BUTTON 'HOST' WAS PRESSED!");
        //SceneManager.LoadScene(sceneName);
    }
}
