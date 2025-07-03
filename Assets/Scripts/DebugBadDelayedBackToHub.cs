using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DebugBadDelayedBackToHub : MonoBehaviour
{
    public string hubName;
    

    public void SwitchInSeconds(float seconds)
    {
        StartCoroutine(SwitchIn(seconds));
    }

    private IEnumerator SwitchIn(float seconds)
    {
        yield return new WaitForSeconds(seconds);

        PlayerManager.Instance?.SetControlScheme(ControlScheme.OnlyInteract);

        SceneManager.LoadScene(hubName);
    }


}
