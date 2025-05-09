using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class CharacterSelection : MonoBehaviour
{
    public enum component{
        Name,
        Skin,
        Ready
    }
    public component editingComponent;
    private PlayerInput playerInput;
    private PlayerShell playerShell;
    private VectorInput vectorInput = new VectorInput();

    public List<GameObject> characters;
    public int selectedCharacter = 0;
    private GameObject activeCharacter;

    [Header("UI")]
    [SerializeField]
    private TextMeshProUGUI nameText;
    [SerializeField]
    private TextMeshProUGUI stateText;

    // Start is called before the first frame update
    void Start()
    {
        playerInput = GetComponentInParent<PlayerInput>();
        playerShell = GetComponentInParent<PlayerShell>();
        playerShell.SwitchControlScheme(ControlScheme.Menu);
        ShowSkin(selectedCharacter);
    }

    // Update is called once per frame
    void Update()
    {
        vectorInput.Update(playerInput.actions["Move"].ReadValue<Vector2>());
        switch (editingComponent){
            case component.Name:
                EditName();
                UpdateNameUI();
                break;
            case component.Skin:
                EditSkin();
                break;
            case component.Ready:
                ReadyUpdate();
                break;
        }
        UpdateStateUI();
    }

    //State Updates
    void EditName(){
        char lastChar = 'a';
        if (playerShell.userName.Length > 0){
            lastChar = playerShell.userName[playerShell.userName.Length - 1];
        }
        
        if (vectorInput.north.pressed){
            //increase
            if (lastChar == 'z'){
                lastChar = 'a';
            }
            else{
                lastChar++;
            }
            ReplaceLastChar(lastChar);
        }
        if (vectorInput.south.pressed){
            //decrease
            if (lastChar == 'a'){
                lastChar = 'z';
            }
            else{
                lastChar--;
            }
            ReplaceLastChar(lastChar);
        }

        if (playerShell.userName.Length > 0){
            playerShell.userName.Remove(playerShell.userName.Length - 1);
        }
        //playerShell.userName += lastChar;

        if (playerInput.actions["Accept"].triggered){
            AddLastChar();
        }
        if (playerInput.actions["Cancel"].triggered){
            RemoveLastChar();
        }
        if (playerInput.actions["Confirm"].triggered){
            editingComponent = component.Skin;
        }
    }

    void EditSkin(){
        if (vectorInput.east.pressed){
            selectedCharacter++;
            if (selectedCharacter >= characters.Count){
                selectedCharacter = 0;
            }
        }
        if (vectorInput.west.pressed){
            selectedCharacter--;
            if (selectedCharacter < 0){
                selectedCharacter = characters.Count - 1;
            }
        }
        ShowSkin(selectedCharacter);

        if (playerInput.actions["Confirm"].triggered){
            playerShell.skin = characters[selectedCharacter];
            editingComponent = component.Ready;
        }
        if (playerInput.actions["Cancel"].triggered){
            editingComponent = component.Name;
        }
    }

    void ReadyUpdate(){
        if (playerInput.actions["Cancel"].triggered){
            editingComponent = component.Skin;
        }
        //spawn after you're ready
        // if (playerInput.actions["Confirm"].triggered){
        //     playerShell.SpawnAvatar(Vector3.zero);
        // }
        //change scene after you're ready
        if (playerInput.actions["Confirm"].triggered){
            SceneManager.LoadScene("MuPl Test scene 2");
        }
    }
    //UI
    void UpdateNameUI(){
        nameText.SetText(playerShell.userName);
    }

    void UpdateStateUI(){
        switch(editingComponent){
            case component.Name:
                stateText.text = "Changing Name";
                break;
            case component.Skin:
                stateText.text = "Selecting Skin";
                break;
            case component.Ready:
                stateText.text = "Ready";
                break;
        }
    }

    void ReplaceLastChar(char _char){
        if (playerShell.userName.Length > 0){
            RemoveLastChar();
        }
        playerShell.userName += _char;
    }

    void AddLastChar(char _char = 'a'){
        playerShell.userName += _char;
    }

    void RemoveLastChar(){
        playerShell.userName = playerShell.userName.Remove(playerShell.userName.Length - 1);
    }
    
    void ShowSkin(int index){
        for (int i = 0; i < characters.Count; i++){
            if (i == index){
                //characters[i].SetActive(true);
                Destroy(activeCharacter);
                activeCharacter = Instantiate(characters[i],transform);
            }
            else{
                //characters[i].SetActive(false);
            }
        }
    }
}
