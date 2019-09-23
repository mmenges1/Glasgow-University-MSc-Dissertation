using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class QuestionScript : MonoBehaviour
{
    public TreasureScript treasureScript;
    /*
     * Function called when student answers a question. Depending on the scene it does the following:
     * Solo:
     *      gets the solo Player collision game object
     *      calls it's onAnswer() function, passing this game objects text of the answer
     * Two-Player:
     *      gets the current Player collision game object (player 1 or player 2)
     *      calls it's onAnswer() function, passing this game objects text of the answer
     */
    public void onAnswer()
    {
        if (SceneManager.GetActiveScene().name.Contains("Solo"))
        {
            GameManager.instance.currentPlayerCollision.transform.GetComponent<SoloTreasureScript>().onAnswer(gameObject.transform.Find("Text").GetComponent<Text>().text);
        }
        else
        {
            treasureScript = GameManager.instance.currentPlayerCollision.transform.GetComponent<TreasureScript>();
            treasureScript.onAnswer(gameObject.transform.Find("Text").GetComponent<Text>().text);
        }
    }
}
