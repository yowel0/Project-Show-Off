using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class SceneHandler : MonoBehaviour
{

    public void LoadScene(string name){
        SceneManager.LoadScene(name, LoadSceneMode.Single);
    }
}
