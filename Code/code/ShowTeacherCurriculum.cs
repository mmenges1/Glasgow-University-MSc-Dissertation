using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowTeacherCurriculum : MonoBehaviour
{
    //Curriculum prefab game object
    public GameObject CreateTeacherCurriculumPrefab;
    //Scroll view for curriculum prefab
    public GameObject scrollViewContentPanel;

    void Start()
    {
        /*
         * for each pair in the sorted dictionary containing the teacher's curriculum do the following:
             * Create prefab in game
             * Set prefabs parent to the scroll view
             * for each UI element in prefab:
                * change UI information based on name
         */
        foreach (KeyValuePair<int, string> item in GameManager.instance.problems)
        {
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
