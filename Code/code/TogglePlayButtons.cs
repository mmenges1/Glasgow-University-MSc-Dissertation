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

    /*
     * Toggle difficulty buttons in Student's Game Options once a programming language has been chosen
     * Sets programming language in GameManager
     */
    public void ToggleGameObjects()
    {
        prefabBeginner.SetActive(true);
        prefabIntermediate.SetActive(true);
        prefabAdvanced.SetActive(true);
        GameManager.instance.programmingLanguage = programLanguage.GetComponent<Button>().name;
        GameManager.instance.setGameLanguage();

    }

    /*
     * Set player count in GameManager
     * Change to One-Player or Two-player scene based on user choice
     */
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
