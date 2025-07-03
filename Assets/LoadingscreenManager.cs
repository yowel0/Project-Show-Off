using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
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

    [SerializeField]
    InputActionReference test;

    private List<PlayerInput> playerInputs = new List<PlayerInput>();
    private bool loadingScreenActive;
    private string selectedSceneName;
    private Slider selectedSlider;

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
        List<PlayerShell> players = PlayerManager.Instance.players;
        foreach (PlayerShell p in players)
        {
            playerInputs.Add(p.GetComponent<PlayerInput>());
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (loadingScreenActive)
        {
            foreach (PlayerInput input in playerInputs)
            {
                if (input.actions["Interact"].triggered)
                {
                    StartCoroutine(LoadSceneAsync());
                }
            }
        }
    }

    public void EnableLoadingScreen(int monsterID)
    {
        switch (monsterID)
        {
            case 0:
                EnableLoadingScreen(Scene.Potato);
                break;
            case 1:
                EnableLoadingScreen(Scene.Cloudy);
                break;
            case 2:
                EnableLoadingScreen(Scene.MrScaryMouse);
                break;
            case 3:
                EnableLoadingScreen(Scene.Bibi);
                break;
        }
    }

    void EnableLoadingScreen(Scene scene)
    {
        switch (scene)
        {
            case Scene.Potato:
                EnableLoadingScreen("PotataParty", Potato, loadingbarPotato);
                break;
            case Scene.Cloudy:
                EnableLoadingScreen("CloudysColours", Cloudy, loadingbarCloudy);
                break;
            case Scene.MrScaryMouse:
                EnableLoadingScreen("ScaryMouseMayhem", MrScaryMouse, loadingbarMouse);
                break;
            case Scene.Bibi:
                EnableLoadingScreen("BibiBuilding", Bibi, loadingbarBibi);
                break;
        }
    }

    void EnableLoadingScreen(string sceneName, GameObject loadingScreen, Slider slider)
    {
        loadingScreenActive = true  ;
        PlayerManager.Instance?.SetControlScheme(ControlScheme.OnlyInteract);
        loadingScreen.SetActive(true);
        selectedSceneName = sceneName;
        selectedSlider = slider;
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

    IEnumerator LoadSceneAsync()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(selectedSceneName);

        while (!operation.isDone)
        {
            float progressVaule = Mathf.Clamp01(operation.progress / 0.9f);

            selectedSlider.value = progressVaule;

            yield return null;
        }
    }

    IEnumerator LoadSceneAsync(string sceneName, GameObject loadingScreen, Slider slider)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);

        while (!operation.isDone)
        {
            float progressVaule = Mathf.Clamp01(operation.progress / 0.9f);

            slider.value = progressVaule;

            yield return null;
        }
    }
}
