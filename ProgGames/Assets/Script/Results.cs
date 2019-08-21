using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Results : NetworkBehaviour
{
    public GameObject[] results;
    public string[] problemsNotSolved = new string[9];
    // Start is called before the first frame update
    void Start()
    {
       // NetworkServer.Shutdown();
        //GameObject.Find("LobbyManager").transform.GetComponent<NetworkBehaviour>()
        int count = 0;
        problemsNotSolved = GameManager.instance.problemsSolved;
      //  Debug.LogError(GameManager.instance.problemsSolved.Length);

        foreach (string s in GameManager.instance.problemsSolved)
        {
            Debug.LogError(GameManager.instance.problems.Count);
            for (int i = 7; i > 0; i--)
            {
                //  Debug.LogError(b.Value);
                if (GameManager.instance.problems[i].Contains(s) && !s.Equals(" "))
                    {
                        Debug.LogError(s);
                        results[count].SetActive(true);
                        results[count].GetComponent<Text>().text = GameManager.instance.problems[i] + "    Answer: " + GameManager.instance.curriculum[GameManager.instance.problems[i]];
                        results[count].transform.Find("text").GetComponent<Text>().text = GameManager.instance.explanation[GameManager.instance.problems[i]];
                        count++;
                        break;
                    }
                else if (s.Equals(" "))
                {
                    Debug.LogError(s);
                    results[count].SetActive(true);
                    results[count].GetComponent<Text>().text = GameManager.instance.problems[count] + "    Answer: " + GameManager.instance.curriculum[GameManager.instance.problems[count]];
                    results[count].transform.Find("text").GetComponent<Text>().text = GameManager.instance.explanation[GameManager.instance.problems[count]];
                    count++;
                    break;
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
