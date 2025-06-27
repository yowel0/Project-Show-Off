using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UISelectionHelper : MonoBehaviour
{
    [Tooltip("Plays when you move selection in a menu. DOES NOT INCLUDE NAME SELECTION KEYBOARD, THAT'S SEPERATE")]
    [SerializeField] SoundObject moveSelection;

    [Header("Debug")]
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
            else
            {
                bool playSound = false;
                if (currentSelected != null) playSound = currentSelected.activeInHierarchy;

                if (playSound) MusicManager.Instance?.PlaySound(moveSelection);
                currentSelected = EventSystem.current.currentSelectedGameObject;
            }
        }
    }
}
