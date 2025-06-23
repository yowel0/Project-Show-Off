using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelectionState : MonoBehaviour
{
    [SerializeField]
    CharacterSelection.Component editingComponent;
    // Start is called before the first frame update
    void OnLevelWasLoaded()
    {
        List<PlayerShell> players = PlayerManager.Instance.players;
        for (int i = 0; i < players.Count; i++)
        {
            print(players[i]);
            CharacterSelection characterSelection = players[i].GetComponentInChildren<CharacterSelection>();
            characterSelection.SetEditingComponent(editingComponent);
            print("test");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
