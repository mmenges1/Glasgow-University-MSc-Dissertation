using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Results : NetworkBehaviour
{
    public GameObject[] results;
    public string[] problemsNotSolved = new string[9];
    public GameObject ResultsPrefab;
    public GameObject scrollViewContentPanel;
    void Start()
    {
        /*
         * if Network Lobby Manager exists, destroy it from the game.
         */
        if (GameObject.Find("LobbyManager") != null)
        {
            Destroy(GameObject.Find("LobbyManager"));
        }

        int count = 0;
        problemsNotSolved = GameManager.instance.problemsSolved;
        /*
         * Create/Append game file using game id entered in login screen
         */
        StreamWriter stream = new StreamWriter(GameManager.instance.gameID+".txt",true);
        stream.WriteLine(GameManager.instance.gameID);
        /*
         * Check if problem was solved by player
         * Display the problem, answer, answer chosen, and explanation in UI scroll view
         * Write the problem solved incorrectly to file
         */
        foreach (string s in GameManager.instance.problemsSolved)
        {
            for (int i = 7; i > 0; i--)
            {
                
                if (GameManager.instance.problems[i].Contains(s) && !s.Equals(" "))
                {
                    GameObject newResultsForList = Instantiate(ResultsPrefab, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
                    newResultsForList.transform.SetParent(scrollViewContentPanel.transform, false);
                    stream.WriteLine(GameManager.instance.problems[i]);
                    count++;
                    foreach (Transform child in newResultsForList.transform.Find("ProblemInfo").transform)
                    {

                        switch (child.name)
                        {
                            case "Problem":
                                child.GetComponent<Text>().text = GameManager.instance.problems[i];
                                break;
                            case "Answer Chosen":
                                child.GetComponent<Text>().text = GameManager.instance.answerChosen[GameManager.instance.problems[i]];
                                break;
                            case "Answer Text":
                                child.GetComponent<Text>().text = GameManager.instance.curriculum[GameManager.instance.problems[i]];
                                break;
                            case "Explanation":
                                child.GetComponent<Text>().text = GameManager.instance.explanation[GameManager.instance.problems[i]];
                                break;
                            default:
                                break;
                        }
                        newResultsForList.transform.parent = scrollViewContentPanel.transform;
                    }
                    break;
                }
                /*
                 * If information for all questions previously on screen to be displayed, the following function handles this.
                 */
                /*  else if (s.Equals(" "))
                  {
                      results[count].SetActive(true);
                      stream.WriteLine(GameManager.instance.problems[count]);
                      results[count].GetComponent<Text>().text = GameManager.instance.problems[count] + "    Answer: " + GameManager.instance.curriculum[GameManager.instance.problems[count]];
                      results[count].transform.Find("text").GetComponent<Text>().text = GameManager.instance.explanation[GameManager.instance.problems[count]];
                      count++;

                  foreach (Transform child in newResultsForList.transform.Find("ProblemInfo").transform)
                  {

                      switch (child.name)
                      {
                          case "Problem":
                              child.GetComponent<Text>().text = item.Value;
                              break;
                          case "Answer Chosen":
                              child.GetComponent<Text>().text = item.Key.ToString();
                              break;
                          case "Answer Text":
                              child.GetComponent<Text>().text = GameManager.instance.curriculum[item.Value];
                              break;
                          default:
                              break;
                      }
                      newResultsForList.transform.parent = scrollViewContentPanel.transform;
                  }
                      break;
                  }*/
            }
        }
        stream.Close();
    }
}
