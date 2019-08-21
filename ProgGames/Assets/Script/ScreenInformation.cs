using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenInformation : MonoBehaviour
{
    public GameObject Name;
    // Start is called before the first frame update
    void Start()
    {
        Name.GetComponent<Text>().text = GameManager.instance.getUserName();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
