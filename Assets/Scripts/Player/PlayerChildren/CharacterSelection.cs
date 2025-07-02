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
    [Tooltip("Plays when you move selection in the keyboard. DOES NOT INCLUDE (PAUSE) MENU NAVIGATION, THAT'S SEPERATE")]
    [SerializeField] SoundObject moveSelection;
    [Tooltip("Plays when you click a button")]
    [SerializeField] SoundObject clickButtonSound;

    public enum Component{
        Hat,
        Name,
        //Character,
        Ready
    }
    public Component editingComponent;
    private Component previousEditingComponent;
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
    private TextMeshProUGUI controlText;

    [Header("State UI")]
    [SerializeField]
    private GameObject hatUI;
    [SerializeField]
    private GameObject nameUI;

    [Header("Keyboard")]
    [SerializeField]
    private Button firstSelected;
    private Button currentSelected;

    [Header("TextFormats")]
    [SerializeField]
    private string kNamingHelpTextFormat = "Press {Accept} to Add a Letter\n Press {Cancel} to Remove a Letter\n Press {Confirm} to Confirm Your Name";
    [SerializeField]
    private string kHatHelpTextFormat = "Press {Confirm} to Confirm Hat\n Press {Cancel} to Go Back";
    //private const string kCharacterHelpTextFormat = "Press {Confirm} to Ready Up\n Press {Cancel} to Go Back";
    [SerializeField]
    private string kReadyHelpTextFormat = "Press {Cancel} to Unready";

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
        SetEditingComponent(editingComponent);
    }

    void SelectButton(Button button) {
        //visually deselect
        if (currentSelected)
        {
            MusicManager.Instance?.PlaySound(moveSelection);
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
            case Component.Ready:
                ReadyUpdate();
                break;
        }
    }

    //State Updates
    void EditName()
    {
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
            MusicManager.Instance?.PlaySound(clickButtonSound);
            currentSelected.onClick.Invoke();
        }
    }

    void EditHat(){
        if (vectorInput.east.pressed){
            selectedHat++;
            MusicManager.Instance?.PlaySound(moveSelection);
            if (selectedHat >= hats.Count){
                selectedHat = 0;
            }
            ShowHat(selectedHat);
        }
        if (vectorInput.west.pressed){
            selectedHat--;
            MusicManager.Instance?.PlaySound(moveSelection);
            if (selectedHat < 0){
                selectedHat = hats.Count - 1;
            }
            ShowHat(selectedHat);
        }

        if (playerInput.actions["Accept"].triggered){
            playerShell.hatPrefab = hats[selectedHat];
            SetEditingComponent(Component.Ready);
        }
    }

    void ReadyUpdate(){
        if (playerInput.actions["Cancel"].triggered){
            SetEditingComponent(previousEditingComponent);
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
            case "ready":
                SetEditingComponent(Component.Ready);
                break;
            
        }
    }

    public void SetEditingComponent(Component component)
    {
        previousEditingComponent = editingComponent;
        editingComponent = component;
        hatUI.SetActive(false);
        nameUI.SetActive(false);
        switch (component)
        {
            case Component.Hat:
                hatUI.SetActive(true);
                break;
            case Component.Name:
                nameUI.SetActive(true);
                break;
            case Component.Ready:
                break;
        }
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
