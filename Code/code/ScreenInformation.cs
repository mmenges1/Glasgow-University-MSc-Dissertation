using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenInformation : MonoBehaviour
{
    public GameObject Name;

    void Start()
    {
        //Set Teacher Name for UI
        Name.GetComponent<Text>().text = GameManager.instance.getUserName();
    }
}
