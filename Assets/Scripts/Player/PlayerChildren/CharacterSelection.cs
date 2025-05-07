using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

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

    // Start is called before the first frame update
    void Start()
    {
        playerInput = GetComponentInParent<PlayerInput>();
        playerShell = GetComponentInParent<PlayerShell>();
    }

    // Update is called once per frame
    void Update()
    {
        vectorInput.Update(playerInput.actions["Move"].ReadValue<Vector2>());
        switch (editingComponent){
            case component.Name:
                EditName();
                break;
            case component.Skin:
                EditSkin();
                break;
        }
    }

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

        if (playerInput.actions["Jump"].triggered){
            AddLastChar();
        }
        if (playerInput.actions["Cancel"].triggered){
            RemoveLastChar();
        }
        if (playerInput.actions["Interact"].triggered){
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

        if (playerInput.actions["Interact"].triggered){
            editingComponent = component.Ready;
        }
        if (playerInput.actions["Cancel"].triggered){
            editingComponent = component.Name;
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
                characters[i].SetActive(true);
            }
            else{
                characters[i].SetActive(false);
            }
        }
    }
}
