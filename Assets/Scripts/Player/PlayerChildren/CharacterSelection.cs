using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CharacterSelection : MonoBehaviour
{
    public enum Component{
        Name,
        Hat,
        //Character,
        Ready
    }
    public Component editingComponent;
    private PlayerShell playerShell;
    private Avatar avatar;
    private PlayerInput playerInput;
    private VectorInput vectorInput = new VectorInput();

    public List<GameObject> hats;
    public int selectedHat = 0;
    //private GameObject activeHat;
    public List<GameObject> characters;
    //public int selectedCharacter = 0;
    //private GameObject activeCharacter;

    [Header("UI")]
    [SerializeField]
    private TextMeshProUGUI nameText;
    [SerializeField]
    private TextMeshProUGUI stateText;
    [SerializeField]
    private TextMeshProUGUI controlText;
    [SerializeField]
    private GameObject[] StateUI;

    [Header("Keyboard")]
    [SerializeField]
    private Button firstSelected;
    private Button currentSelected;

    private const string kNamingHelpTextFormat = "Press {Accept} to Add a Letter\n Press {Cancel} to Remove a Letter\n Press {Confirm} to Confirm Your Name";
    private const string kHatHelpTextFormat = "Press {Confirm} to Confirm Hat\n Press {Cancel} to Go Back";
    //private const string kCharacterHelpTextFormat = "Press {Confirm} to Ready Up\n Press {Cancel} to Go Back";
    private const string kReadyHelpTextFormat = "Press {Cancel} to Unready";

    // Start is called before the first frame update
    void Start()
    {
        playerInput = GetComponentInParent<PlayerInput>();
        avatar = GetComponentInChildren<Avatar>();
        playerShell = GetComponentInParent<PlayerShell>();
        playerShell.SwitchControlScheme(ControlScheme.Menu);
        //selected character based on player ID
        ShowCharacter(PlayerManager.Instance.GetPlayerID(playerShell));
        ShowHat(selectedHat);
        SelectButton(firstSelected);
    }

    void SelectButton(Button button) {
        //visually deselect
        if (currentSelected)
        {
            ColorBlock _colorblock = currentSelected.colors;
            _colorblock.normalColor = Color.white;
            currentSelected.colors = _colorblock;
        }
        //visually select
        ColorBlock colorblock = button.colors;
        colorblock.normalColor = Color.green;
        button.colors = colorblock;
        //select
        currentSelected = button;
    }

    // Update is called once per frame
    void Update()
    {
        vectorInput.Update(playerInput.actions["Move"].ReadValue<Vector2>());
        switch (editingComponent){
            case Component.Name:
                EditName();
                UpdateNameUI();
                break;
            case Component.Hat:
                EditHat();
                break;
            // case Component.Character:
            //     EditCharacter();
            //     break;
            case Component.Ready:
                ReadyUpdate();
                break;
        }
        UpdateStateUI();
    }

    //State Updates
    void EditName()
    {
        /*
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
            AddNewChar();
        }
        if (playerInput.actions["Cancel"].triggered){
            RemoveLastChar();
        }
        if (playerInput.actions["Confirm"].triggered){
            SetEditingComponent(Component.Hat);
        }
        */
        if (vectorInput.north.pressed)
        {
            if (currentSelected.navigation.selectOnUp)
                SelectButton(currentSelected.navigation.selectOnUp as Button);
        }
        if (vectorInput.east.pressed)
        {
            if (currentSelected.navigation.selectOnRight)
                SelectButton(currentSelected.navigation.selectOnRight as Button);
        }
        if (vectorInput.south.pressed)
        {
            if (currentSelected.navigation.selectOnDown)
                SelectButton(currentSelected.navigation.selectOnDown as Button);
        }
        if (vectorInput.west.pressed)
        {
            if (currentSelected.navigation.selectOnLeft)
                SelectButton(currentSelected.navigation.selectOnLeft as Button);
        }
        if (playerInput.actions["Accept"].triggered)
        {
            currentSelected.onClick.Invoke();
        }
    }

    void EditHat(){
        if (vectorInput.east.pressed){
            selectedHat++;
            if (selectedHat >= hats.Count){
                selectedHat = 0;
            }
            ShowHat(selectedHat);
        }
        if (vectorInput.west.pressed){
            selectedHat--;
            if (selectedHat < 0){
                selectedHat = hats.Count - 1;
            }
            ShowHat(selectedHat);
        }

        if (playerInput.actions["Confirm"].triggered){
            playerShell.hatPrefab = hats[selectedHat];
            SetEditingComponent(Component.Ready);
        }
        if (playerInput.actions["Cancel"].triggered){
            SetEditingComponent(Component.Name);
        }
    }

    /*void EditCharacter(){
        if (vectorInput.east.pressed){
            selectedCharacter++;
            if (selectedCharacter >= characters.Count){
                selectedCharacter = 0;
            }
            ShowCharacter(selectedCharacter);
        }
        if (vectorInput.west.pressed){
            selectedCharacter--;
            if (selectedCharacter < 0){
                selectedCharacter = characters.Count - 1;
            }
            ShowCharacter(selectedCharacter);
        }

        if (playerInput.actions["Confirm"].triggered){
            playerShell.characterPrefab = characters[selectedCharacter];
            SetEditingComponent(Component.Ready);
        }
        if (playerInput.actions["Cancel"].triggered){
            SetEditingComponent(Component.Hat);
        }
    }*/

    void ReadyUpdate(){
        if (playerInput.actions["Cancel"].triggered){
            SetEditingComponent(Component.Hat);
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
            case Component.Name:
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
            case Component.Hat:
                stateText.text = "Selecting Hat";
                controlText.text = ReplaceControlText(kHatHelpTextFormat);
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
            // case Component.Character:
            //     stateText.text = "Selecting Character";
            //     controlText.text = ReplaceControlText(kCharacterHelpTextFormat);
            //     for (int i = 0; i < StateUI.Count(); i++){
            //         if (i == 2){
            //             StateUI[i].SetActive(true);
            //         }
            //         else
            //             StateUI[i].SetActive(false);
            //     }
            //     break;
            case Component.Ready:
                stateText.text = "Ready";
                controlText.text = ReplaceControlText(kHatHelpTextFormat);
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

    public void SetEditingComponent(string componentName)
    {
        componentName = componentName.ToLower();
        switch (componentName)
        {
            case "name":
                SetEditingComponent(Component.Name);
                break;
            case "hat":
                SetEditingComponent(Component.Hat);
                break;
            // case "character":
            //     SetEditingComponent(Component.Character);
            //     break;
            case "ready":
                SetEditingComponent(Component.Ready);
                break;
            
        }
    }

    void SetEditingComponent(Component component)
    {
        editingComponent = component;
    }

    void ReplaceLastChar(char _char){
        if (playerShell.userName.Length > 0){
            RemoveLastChar();
        }
        playerShell.userName += _char;
    }

    public void AddNewChar(string _char = "a"){
        playerShell.userName += _char;
    }

    public void RemoveLastChar(){
        if (playerShell.userName.Length > 0)
            playerShell.userName = playerShell.userName.Remove(playerShell.userName.Length - 1);
    }

    void ShowCharacter(int index)
    {
        avatar.SetCharacter(characters[index]);
        playerShell.characterPrefab = characters[index];
    }

    void ShowHat(int index) {
        avatar.SetHat(hats[index]);
        playerShell.hatPrefab = hats[index];
    }
}
