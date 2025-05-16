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
        Hat,
        Avatar,
        Ready
    }
    public component editingComponent;
    private PlayerInput playerInput;
    private PlayerShell playerShell;
    private VectorInput vectorInput = new VectorInput();

    public List<GameObject> hats;
    public int selectedHat = 0;
    private GameObject activeHat;
    public List<GameObject> avatars;
    public int selectedAvatar = 0;
    private GameObject activeCharacter;

    [Header("UI")]
    [SerializeField]
    private TextMeshProUGUI nameText;
    [SerializeField]
    private TextMeshProUGUI stateText;
    [SerializeField]
    private TextMeshProUGUI controlText;
    [SerializeField]
    private GameObject[] StateUI;

    private const string kNamingHelpTextFormat = "Press {Accept} to Add a Letter\n Press {Cancel} to Remove a Letter\n Press {Confirm} to Confirm Your Name";
    private const string kHatHelpTextFormat = "Press {Confirm} to Confirm Hat\n Press {Cancel} to Go Back";
    private const string kAvatarHelpTextFormat = "Press {Confirm} to Ready Up\n Press {Cancel} to Go Back";
    private const string kReadyHelpTextFormat = "Press {Cancel} to Unready";

    // Start is called before the first frame update
    void Start()
    {
        playerInput = GetComponentInParent<PlayerInput>();
        playerShell = GetComponentInParent<PlayerShell>();
        playerShell.SwitchControlScheme(ControlScheme.Menu);
        ShowHat(selectedHat);
        ShowAvatar(selectedAvatar);
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
            case component.Hat:
                EditHat();
                break;
            case component.Avatar:
                EditAvatar();
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
            editingComponent = component.Avatar;
        }
    }

    void EditHat(){

    }

    void EditAvatar(){
        if (vectorInput.east.pressed){
            selectedAvatar++;
            if (selectedAvatar >= avatars.Count){
                selectedAvatar = 0;
            }
        }
        if (vectorInput.west.pressed){
            selectedAvatar--;
            if (selectedAvatar < 0){
                selectedAvatar = avatars.Count - 1;
            }
        }
        ShowAvatar(selectedAvatar);

        if (playerInput.actions["Confirm"].triggered){
            playerShell.hat = avatars[selectedAvatar];
            editingComponent = component.Ready;
        }
        if (playerInput.actions["Cancel"].triggered){
            editingComponent = component.Name;
        }
    }

    void ReadyUpdate(){
        if (playerInput.actions["Cancel"].triggered){
            editingComponent = component.Avatar;
        }
        //spawn after you're ready
        // if (playerInput.actions["Confirm"].triggered){
        //     playerShell.SpawnAvatar(Vector3.zero);
        // }
        //change scene after you're ready
        // if (playerInput.actions["Confirm"].triggered){
        //     SceneManager.LoadScene("MuPl Test scene 2");
        // }
    }
    //UI
    void UpdateNameUI(){
        nameText.SetText(playerShell.userName);
    }

    void UpdateStateUI(){
        switch(editingComponent){
            case component.Name:
                stateText.text = "Changing Name";
                controlText.text = ReplaceControlText(kNamingHelpTextFormat);
                for (int i = 0; i < StateUI.Count(); i++)
                {
                    if (i == 0)
                    {
                        StateUI[i].SetActive(true);
                    }
                    else
                        StateUI[i].SetActive(false);
                }
                break;
            case component.Hat:
                stateText.text = "Selecting Hat";
                controlText.text = kHatHelpTextFormat
                    .Replace("{Confirm}", playerInput.actions["Confirm"].GetBindingDisplayString())
                    .Replace("{Cancel}", playerInput.actions["Cancel"].GetBindingDisplayString());
                for (int i = 0; i < StateUI.Count(); i++)
                {
                    if (i == 1)
                    {
                        StateUI[i].SetActive(true);
                    }
                    else
                        StateUI[i].SetActive(false);
                }
                break;
            case component.Avatar:
                stateText.text = "Selecting Avatar";
                controlText.text = kAvatarHelpTextFormat
                    .Replace("{Confirm}", playerInput.actions["Confirm"].GetBindingDisplayString())
                    .Replace("{Cancel}", playerInput.actions["Cancel"].GetBindingDisplayString());
                for (int i = 0; i < StateUI.Count(); i++){
                    if (i == 1){
                        StateUI[i].SetActive(true);
                    }
                    else
                        StateUI[i].SetActive(false);
                }
                break;
            case component.Ready:
                stateText.text = "Ready";
                controlText.text = kHatHelpTextFormat
                    .Replace("{Cancel}", playerInput.actions["Cancel"].GetBindingDisplayString());
                for (int i = 0; i < StateUI.Count(); i++)
                {
                    StateUI[i].SetActive(false);
                }
                break;
        }
    }

    string ReplaceControlText(string _string)
    {
        return _string
                    .Replace("{Accept}", playerInput.actions["Accept"].GetBindingDisplayString())
                    .Replace("{Confirm}", playerInput.actions["Confirm"].GetBindingDisplayString())
                    .Replace("{Cancel}", playerInput.actions["Cancel"].GetBindingDisplayString());
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
    
    void ShowAvatar(int index){
        for (int i = 0; i < avatars.Count; i++){
            if (i == index){
                //avatars[i].SetActive(true);
                Destroy(activeCharacter);
                activeCharacter = Instantiate(avatars[i],transform);
            }
            else{
                //avatars[i].SetActive(false);
            }
        }
    }

    void ShowHat(int index){
        for (int i = 0; i < hats.Count; i++){
            if (i == index){
                //avatars[i].SetActive(true);
                Destroy(activeHat);
                activeHat = Instantiate(hats[i],transform);
            }
            else{
                //avatars[i].SetActive(false);
            }
        }
    }
}
