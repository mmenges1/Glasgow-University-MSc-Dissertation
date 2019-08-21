using UnityEngine;
using Mono.Data.Sqlite;
using System.Data;
using System.IO;
using UnityEngine.UI;

public class CreateStudentWork : MonoBehaviour
{
    public GameObject CreateStudentsListPrefab;
    public GameObject scrollViewContentPanel;

    void Start()
    {
        int languageID = 1;
        if (gameObject.name.Contains("Python"))
        {
            languageID = 2;
        }
        string pathDB = Path.Combine(Application.persistentDataPath, "ProgGames.db");
        //  string connectionURL = Application.streamingAssetsPath+ "/ProgGames.db";
        string connectionURL = "URI=file:" + Application.dataPath + "/StreamingAssets/ProgGames.db";
        IDbConnection connection = new SqliteConnection(connectionURL);
        connection.Open();

        IDbCommand NotCompletedCommand = connection.CreateCommand();
        bool a = int.TryParse(GameManager.instance.getStudentID(), out int sID);
        NotCompletedCommand.CommandText = "select curriculum_id from notCompleted where student_id=" + sID;

        IDataReader NorCompletedReader = NotCompletedCommand.ExecuteReader();
        while (NorCompletedReader.Read())
        {

            IDbCommand CurriculumCommand = connection.CreateCommand();
            CurriculumCommand.CommandText = "select problem_text from curriculum where c_id="+ NorCompletedReader[0].ToString()+" and language_id="+languageID +" and teacher_id="+GameManager.instance.getUserID();
            Debug.Log(NorCompletedReader[0].ToString());

            IDataReader CurriculumReader = CurriculumCommand.ExecuteReader();
            while (CurriculumReader.Read())
            {
                //create list of students
                GameObject newStudentForList = Instantiate(CreateStudentsListPrefab, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
                foreach (Transform child in newStudentForList.transform)
                {
                    Debug.Log(CurriculumReader[0].ToString());
                    switch (child.name)
                    {
                        case "Problem":
                            child.transform.GetComponent<Text>().text = CurriculumReader[0].ToString();
                            break;
                        case "Correct Or Not":
                            child.transform.GetComponent<Text>().text = "Not Completed";
                            break;
                        default:
                            //Debug.Log("None");
                            break;
                    }
                    newStudentForList.transform.SetParent(scrollViewContentPanel.transform, false);
                    newStudentForList.transform.parent = scrollViewContentPanel.transform;
                }
            }
            CurriculumCommand.Dispose();
            CurriculumCommand = null;
        }
        IDbCommand CompletedCommand = connection.CreateCommand();
        CompletedCommand.CommandText = "select curriculum_id from completed where student_id=" + sID;

        IDataReader CompletedReader = CompletedCommand.ExecuteReader();
        while (CompletedReader.Read())
        {

            IDbCommand CurriculumCommand2 = connection.CreateCommand();
            CurriculumCommand2.CommandText = "select problem_text from curriculum where c_id=" + CompletedReader[0].ToString() + " and language_id=" + languageID + " and teacher_id=" + GameManager.instance.getUserID();
            IDataReader CurriculumReader = CurriculumCommand2.ExecuteReader();
            while (CurriculumReader.Read())
            {
                //create list of students
                GameObject newStudentForList = Instantiate(CreateStudentsListPrefab, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
                foreach (Transform child in newStudentForList.transform)
                {
                    Debug.Log(CurriculumReader[0].ToString());
                    switch (child.name)
                    {
                        case "Problem":
                            child.transform.GetComponent<Text>().text = CurriculumReader[0].ToString();
                            break;
                        case "Correct Or Not":
                            child.transform.GetComponent<Text>().text = "Completed";
                            break;
                        default:
                            //Debug.Log("None");
                            break;
                    }
                    newStudentForList.transform.SetParent(scrollViewContentPanel.transform, false);
                    newStudentForList.transform.parent = scrollViewContentPanel.transform;
                }
            }
            CurriculumCommand2.Dispose();
            CurriculumCommand2 = null;
        }

        NorCompletedReader.Close();
        NorCompletedReader = null;
        NotCompletedCommand.Dispose();
        NotCompletedCommand = null;
        connection.Close();
        connection = null;
    }
}
