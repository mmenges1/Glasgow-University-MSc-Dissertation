using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class TogglePlayButtons : MonoBehaviour
{
    public GameObject prefabBeginner;
    public GameObject prefabIntermediate;
    public GameObject prefabAdvanced;
    public GameObject multiPlayer;
    public GameObject onePlayer;
    public GameObject programLanguage;

    public void ToggleGameObjects()
    {
        prefabBeginner.SetActive(true);
        prefabIntermediate.SetActive(true);
        prefabAdvanced.SetActive(true);
        GameManager.instance.programmingLanguage = programLanguage.GetComponent<Button>().name;
        GameManager.instance.setGameLanguage();

    }

    public void TogglePlayerCountObjects(int count)
    {
        GameManager.instance.playerCount = count;
        if (count == 2)
        {
          SceneManager.LoadScene("NetworkConnectionScene");
        }
        else if (count == 1)
        {
            SceneManager.LoadScene("SoloGamePlayScene");
        }
    }

}
