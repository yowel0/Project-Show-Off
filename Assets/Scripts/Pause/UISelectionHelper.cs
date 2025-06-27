using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UISelectionHelper : MonoBehaviour
{
    [SerializeField] GameObject currentSelected;

    public static UISelectionHelper Instance;

    private void Start()
    {
        if (Instance == null) Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
    }


    private void Update()
    {
        if (!EventSystem.current) return;
        if (EventSystem.current.currentSelectedGameObject != currentSelected || currentSelected == null)
        {
            if (EventSystem.current.currentSelectedGameObject == null)
            {
                EventSystem.current.SetSelectedGameObject(currentSelected);
            }
            else currentSelected = EventSystem.current.currentSelectedGameObject;
        }
    }
}
