using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class CameraMover : MonoBehaviour
{
    [SerializeField]
    CameraPosition[] camPositions;
    int selectedPosition;
    [SerializeField]
    float t = .1f;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            SetSelectedPositionIndex(selectedPosition++);
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            SetSelectedPositionIndex(selectedPosition--);
        }

        transform.position = Vector3.Lerp(transform.position, camPositions[selectedPosition].transform.position, t);
    }

    public void SetSelectedPositionIndex(int index)
    {
        index = Mathf.Clamp(index, 0, camPositions.Count() - 1);
        camPositions[selectedPosition].OnDepart.Invoke();
        selectedPosition = index;
        camPositions[selectedPosition].OnTravelTo.Invoke();
    }
}

[Serializable]
public class CameraPosition
{
    [SerializeField]
    public Transform transform;
    [SerializeField]
    public UnityEvent OnTravelTo;
    [SerializeField]
    public UnityEvent OnDepart;
}
