using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

public class EntryView : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI placement;
    [SerializeField]
    TextMeshProUGUI userName;
    [SerializeField]
    TextMeshProUGUI score;

    void Start()
    {
        
    }

    public void SetUI(int pPlacement, string pUserName, int pScore)
    {
        placement.text = pPlacement.ToString();
        userName.text = pUserName;
        score.text = pScore.ToString();
    }
}
