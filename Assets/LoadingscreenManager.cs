using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingscreenManager : MonoBehaviour
{
    [Header("Loadingscreens")]
    [SerializeField]
    GameObject Potato;
    [SerializeField]
    Slider loadingbarPotato;
    [SerializeField]
    GameObject Cloudy;
    [SerializeField]
    Slider loadingbarCloudy;
    [SerializeField]
    GameObject MrScaryMouse;
    [SerializeField]
    Slider loadingbarMouse;
    [SerializeField]
    GameObject Bibi;
    [SerializeField]
    Slider loadingbarBibi;

    public enum Scene
    {
        Potato,
        Cloudy,
        MrScaryMouse,
        Bibi
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void LoadMonsterScene(int monsterID)
    {
        switch (monsterID)
        {
            case 0:
                LoadScene(Scene.Potato);
                break;
            case 1:
                LoadScene(Scene.Cloudy);
                break;
            case 2:
                LoadScene(Scene.MrScaryMouse);
                break;
            case 3:
                LoadScene(Scene.Bibi);
                break;
        }
    }

    public void LoadScene(Scene scene)
    {
        switch (scene)
        {
            case Scene.Potato:
                StartCoroutine(LoadSceneAsync("PotataParty", Potato, loadingbarPotato));
                break;
            case Scene.Cloudy:
                StartCoroutine(LoadSceneAsync("CloudysColours", Cloudy, loadingbarCloudy));
                break;
            case Scene.MrScaryMouse:
                StartCoroutine(LoadSceneAsync("ScaryMouseMayhem", MrScaryMouse, loadingbarMouse));
                break;
            case Scene.Bibi:
                StartCoroutine(LoadSceneAsync("BibiBuilding", Bibi, loadingbarBibi));
                break;
        }
    }

    IEnumerator LoadSceneAsync(string sceneName, GameObject loadingScreen, Slider slider)
    {
        PlayerManager.Instance?.SetControlScheme(ControlScheme.OnlyInteract);

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);

        loadingScreen.SetActive(true);

        while (!operation.isDone)
        {
            float progressVaule = Mathf.Clamp01(operation.progress / 0.9f);

            slider.value = progressVaule;

            yield return null;
        }
    }
}
