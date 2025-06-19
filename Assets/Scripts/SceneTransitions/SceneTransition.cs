using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using Cysharp.Threading.Tasks;
using System.Threading.Tasks;
using Unity.VisualScripting;

public class SceneTransition : MonoBehaviour
{
    private const string wrapperCls = "Overlay";
    private const string wrapperActiveCls = "Overlay--Active";
    static public SceneTransition Instance;

    [SerializeField]
    private UIDocument uiDoc;
    private VisualElement rootEl;
    private VisualElement wrapperEl;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnEnable()
    {
        rootEl = uiDoc.rootVisualElement;
        wrapperEl = rootEl.Q(className: wrapperCls);
    }

    public void ChangeScene(string sceneName)
    {
        ChangeSceneAsync(sceneName);
    }

    public async UniTask ChangeSceneAsync(string sceneName)
    {
        await DisplayOverlay();
        await SceneManager.LoadSceneAsync(sceneName);
        await HideOverlay();
    }

    public async UniTask DisplayOverlay()
    {
        wrapperEl.AddToClassList(wrapperActiveCls);
        await UniTask.Delay(500);
    }

    public async UniTask HideOverlay()
    {
        wrapperEl.RemoveFromClassList(wrapperActiveCls);
        await UniTask.Delay(500);
    }
}
