using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mono.Data.Sqlite;
using System.Data;
using UnityEngine.UI;

public class ShowTeacherCurriculum : MonoBehaviour
{
    public GameObject CreateTeacherCurriculumPrefab;
    public GameObject scrollViewContentPanel;

    void Start()
    {
        foreach (KeyValuePair<int, string> item in GameManager.instance.problems)
        {
          //  Debug.Log("Item: " + item.Value);
            //create list of students
            GameObject newCurriculumForList = Instantiate(CreateTeacherCurriculumPrefab, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
            newCurriculumForList.transform.SetParent(scrollViewContentPanel.transform, false);
            foreach (Transform child in newCurriculumForList.transform.Find("ProblemInfo").transform)
            {
                switch (child.name)
                {
                    case "Problem":
                        child.GetComponent<Text>().text = item.Value;
                        break;
                    case "Problem ID":
                        child.GetComponent<Text>().text =  item.Key.ToString();
                        break;
                    case "Answer Text":
                        child.GetComponent<Text>().text = GameManager.instance.curriculum[item.Value];
                        break;
                    case "EditB":
                        break;
                    default:
                        Debug.Log("None");
                        break;
                }
                newCurriculumForList.transform.parent = scrollViewContentPanel.transform;
            }
        }
    }
}
