using UnityEngine;
using UnityEngine.UI;

public class StudentTitleScreen : MonoBehaviour
{
    public GameObject studentName;
    //Update UI with logged in Student's name
    void Start()
    {
        studentName.GetComponent<Text>().text = GameManager.instance.getUserName();
    }

}
