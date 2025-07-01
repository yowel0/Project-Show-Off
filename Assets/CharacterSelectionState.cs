using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelectionState : MonoBehaviour
{
    [SerializeField]
    bool onAwake = true;
    [SerializeField]
    CharacterSelection.Component editingComponent;
    // Start is called before the first frame update
    void OnLevelWasLoaded()
    {
        if (onAwake)
        {
            SetEditingComponent(editingComponent);
        }
    }

    public void SetEditingComponent(CharacterSelection.Component component)
    {
        List<PlayerShell> players = PlayerManager.Instance.players;
        for (int i = 0; i < players.Count; i++)
        {
            print(players[i]);
            CharacterSelection characterSelection = players[i].GetComponentInChildren<CharacterSelection>();
            characterSelection.SetEditingComponent(component);
        }
    }
    
    public void SetEditingComponent(string componentName)
    {
        List<PlayerShell> players = PlayerManager.Instance.players;
            for (int i = 0; i < players.Count; i++)
            {
                print(players[i]);
                CharacterSelection characterSelection = players[i].GetComponentInChildren<CharacterSelection>();
                characterSelection.SetEditingComponent(componentName);
            }
    }
}
