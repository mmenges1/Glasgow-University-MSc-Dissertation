using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Networking;

public class TreasureScript : MonoBehaviour
{
    Collider treasureCollider;
    public GameObject questionBoard;
    public GameObject QuestionsPrefab;
    private int randomAnswerChoice;
    public int questionNumber;
    public string answerChosen;
    private GameObject playerObject;

    /**
     * set the treasure collider to the 2D box collider of the game object this script is attached to
     * randonly choose the location the answer will appear for the player
     * set the initial treasure count to 8
     */
    void Start()
    {
        treasureCollider = gameObject.GetComponent<Collider>();
        randomAnswerChoice = Random.Range(1, 4);
        GameManager.instance.treasureCount = 8;
    }

    /*
 * from the Question.cs script an answer chosen string is sent here
 * This onAnswer method checks if the answer is correct correlating to the problem
 * from the GameManager's sorted dictionary.
 * if the player is the local player and not the other players clients
         * if it's correct: it does nothing at the moment but in future installments would like to display some sort of notification
         * else (it's wrong): 
         *      add the problem to problems solved
         *      add the answer to answerChosen
         *      increase the total count
     * call the player script to destroy this game object on in all clients games
     * close the question UI
 */
    public void onAnswer(string answerChos)
    {
        if (playerObject.transform.GetComponent<PlayerScript>().isLocalPlayer)
        {
            answerChosen = answerChos;
            if (answerChos.Equals(GameManager.instance.curriculum[GameManager.instance.problems[questionNumber]]))
            {
                //Correct Answer
               // Debug.LogError("Correct Answer");
            }
            else
            {
                if (GameManager.instance.count < GameManager.instance.problemsSolved.Length)
                {
                    GameManager.instance.problemsSolved[GameManager.instance.count] = GameManager.instance.problems[questionNumber];
                    GameManager.instance.answerChosen[GameManager.instance.problems[questionNumber]] = answerChos;
                    GameManager.instance.count++;
                }
            }
            playerObject.transform.GetComponent<PlayerScript>().CmdDestroyObject(this.gameObject.GetComponent<NetworkIdentity>().netId, GameObject.Find("SpawnPoint").GetComponent<NetworkIdentity>().netId);
            playerObject.transform.Find("Canvas").gameObject.SetActive(false);
        }
    }
    /*
      * When collision with player and treasure is detected
      * display the question for that treasure, set the answer to a 
      * random option and the wrong answers to the others
      * 
      */
    private void OnTriggerEnter(Collider player)
    {
            if (player.gameObject.transform.GetComponent<PlayerScript>().isLocalPlayer)
            {
                GameManager.instance.questionNumber = this.gameObject;
                playerObject = player.gameObject;
                player.gameObject.transform.Find("Canvas").gameObject.SetActive(true);
                int wrongAnswer1 = 0;
                int wrongAnswer2 = 0;
                int wrongAnswer3 = 0;
                GameManager.instance.currentPlayerCollision = gameObject;
                player.gameObject.transform.Find("Canvas").transform.Find("Question").gameObject.SetActive(true);
                player.gameObject.transform.Find("Canvas").transform.Find("Question").transform.Find("Question").transform.Find("InputField").GetComponent<InputField>().text = GameManager.instance.problems[questionNumber];
                player.gameObject.transform.Find("Canvas").transform.Find("Question").transform.Find("Answer " + randomAnswerChoice).transform.Find("Text").GetComponent<Text>().text = GameManager.instance.curriculum[GameManager.instance.problems[questionNumber]];
                string otherOptions = GameManager.instance.wrongOptions[GameManager.instance.problems[questionNumber]];
                string[] otherOptionsList = otherOptions.Split('|');
                switch (randomAnswerChoice)
                {
                    case 1:
                        wrongAnswer1 = 2;
                        wrongAnswer2 = 3;
                        wrongAnswer3 = 4;
                        break;
                    case 2:
                        wrongAnswer1 = 3;
                        wrongAnswer2 = 4;
                        wrongAnswer3 = 1;
                        break;
                    case 3:
                        wrongAnswer1 = 1;
                        wrongAnswer2 = 4;
                        wrongAnswer3 = 2;
                        break;
                    case 4:
                        wrongAnswer1 = 3;
                        wrongAnswer2 = 1;
                        wrongAnswer3 = 2;
                        break;
                    default:
                        break;
                }

            player.gameObject.transform.Find("Canvas").transform.Find("Question").transform.Find("Answer " + wrongAnswer1).transform.Find("Text").GetComponent<Text>().text = otherOptionsList[0];
            player.gameObject.transform.Find("Canvas").transform.Find("Question").transform.Find("Answer " + wrongAnswer2).transform.Find("Text").GetComponent<Text>().text = otherOptionsList[1];
            player.gameObject.transform.Find("Canvas").transform.Find("Question").transform.Find("Answer " + wrongAnswer3).transform.Find("Text").GetComponent<Text>().text = otherOptionsList[2];
        }
    }
    /*
     * on collision exit:
     * set player object to null
     * close the question UI
     */
    private void OnTriggerExit(Collider player)
    {
            if (player.gameObject.transform.GetComponent<PlayerScript>().isLocalPlayer)
            {
            playerObject = null;
            player.gameObject.transform.Find("Canvas").gameObject.SetActive(false);
            }
    }
}
