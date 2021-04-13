using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public void OnValueChanged(float Value)
    {
        Debug.Log("New Value " + Value);
    }
}
