using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StudentTitleScreen : MonoBehaviour
{
    public GameObject studentName;
    // Start is called before the first frame update
    void Start()
    {
        studentName.GetComponent<Text>().text = GameManager.instance.getUserName();
    }

    public void setDifficultyLevel()
    {

    }

    // Update is called once per frame
    void Update()
    {
    }
}
