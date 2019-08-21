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

    void Start()
    {
        treasureCollider = gameObject.GetComponent<Collider>();
        randomAnswerChoice = Random.Range(1, 4);
      //  GameManager.instance.treasureCount = 8;
    }

    public void onAnswer(string answerChos)
    {
        if (playerObject.transform.GetComponent<PlayerScript>().isLocalPlayer)
        {
            answerChosen = answerChos;
            Debug.Log(answerChosen);
            if (answerChos.Equals(GameManager.instance.curriculum[GameManager.instance.problems[questionNumber]]))
            {
                Debug.LogError("Correct Answer");

            }
            else
            {
                if (GameManager.instance.count < GameManager.instance.problemsSolved.Length)
                {
                    Debug.LogError("This is the treasure script count " + GameManager.instance.count);
                    GameManager.instance.problemsSolved[GameManager.instance.count] = GameManager.instance.problems[questionNumber];
                    GameManager.instance.count++;
                }
            }
            playerObject.transform.GetComponent<PlayerScript>().decreaseTreasureCount();
            playerObject.transform.GetComponent<PlayerScript>().CmdDestroyObject(this.gameObject.GetComponent<NetworkIdentity>().netId);
            playerObject.transform.Find("Canvas").gameObject.SetActive(false);


        }
    }

    private void OnTriggerEnter(Collider player)
    {
            if (player.gameObject.transform.GetComponent<PlayerScript>().isLocalPlayer)
            {
                GameManager.instance.questionNumber = this.gameObject;
                playerObject = player.gameObject;
                player.gameObject.transform.Find("Canvas").gameObject.SetActive(true);
                // Debug.Log("Enter");
                int wrongAnswer1 = 0;
                int wrongAnswer2 = 0;
                int wrongAnswer3 = 0;
                GameManager.instance.currentPlayerCollision = gameObject;
                player.gameObject.transform.Find("Canvas").transform.Find("Question").gameObject.SetActive(true);
                player.gameObject.transform.Find("Canvas").transform.Find("Question").transform.Find("Question").transform.Find("InputField").GetComponent<InputField>().text = GameManager.instance.problems[questionNumber];
                player.gameObject.transform.Find("Canvas").transform.Find("Question").transform.Find("Answer " + randomAnswerChoice).transform.Find("Text").GetComponent<Text>().text = GameManager.instance.curriculum[GameManager.instance.problems[questionNumber]];
                string otherOptions = GameManager.instance.wrongOptions[GameManager.instance.problems[questionNumber]];
                string[] otherOptionsList = otherOptions.Split('|');
                //.Replace(", ", "|")
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

            //  newQuestion.SetActive(true);
        }
    }
    private void OnTriggerExit(Collider player)
    {
            if (player.gameObject.transform.GetComponent<PlayerScript>().isLocalPlayer)
            {
            //  Debug.Log("Exit");
            playerObject = null;
            player.gameObject.transform.Find("Canvas").gameObject.SetActive(false);
                //questionBoard.SetActive(false);
            }
    }
}
