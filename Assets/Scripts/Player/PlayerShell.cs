using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShell : MonoBehaviour
{
    public enum ControlScheme{
        Menu,
        Movement
    };
    public ControlScheme controlScheme = ControlScheme.Menu;
    public string userName = "a";
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<PlayerInput>().SwitchCurrentActionMap("Menu");
        print(GetComponent<PlayerInput>().currentActionMap);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SwitchControlScheme(ControlScheme _controlScheme){
        controlScheme = _controlScheme;
        switch (controlScheme){
            case ControlScheme.Menu:
                break;
            case ControlScheme.Movement:
                break;
        };
    }
}
