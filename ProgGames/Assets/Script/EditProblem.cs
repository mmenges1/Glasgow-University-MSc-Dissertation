using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mono.Data.Sqlite;
using System.Data;
using System.IO;

public class EditProblem : MonoBehaviour
{
    public GameObject editProblemScreen;
    public void editProblem()
    {
        GameObject editPanel = null;
        foreach (GameObject a in UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects())
        {
            Debug.Log(a.name);
            if(a.name.Contains("Canvas"))
            {
                Debug.Log("Heyo");
                editPanel = a.transform.Find("Edit Panel").gameObject;
            }
        }
        if (editPanel != null)
        {
            editPanel.SetActive(true);
            Transform panel = editPanel.transform.Find("Panel");
            Debug.Log(panel.Find("Problem").GetComponent<InputField>().text);
            panel.Find("Problem").GetComponent<InputField>().text = gameObject.transform.parent.Find("Problem").GetComponent<Text>().text;
            panel.Find("Answer").GetComponent<InputField>().text = gameObject.transform.parent.Find("Answer Text").GetComponent<Text>().text;
            panel.Find("Problem ID").GetComponent<Text>().text = gameObject.transform.parent.Find("Problem ID").GetComponent<Text>().text;

            Debug.Log("Edit Problem");
        }
    }
    public void saveEditProblem()
    {

        string problem = editProblemScreen.transform.Find("Panel").transform.Find("Problem").GetComponent<InputField>().text;
        string answer = editProblemScreen.transform.Find("Panel").transform.Find("Answer").GetComponent<InputField>().text;

        string editID = editProblemScreen.transform.Find("Panel").transform.Find("Problem ID").GetComponent<Text>().text;

        Debug.Log("save edit Problem " + editID);

        string pathDB = Path.Combine(Application.persistentDataPath, "ProgGames.db");
        string connectionURL = "URI=file:" + Application.dataPath + "/StreamingAssets/ProgGames.db";
        IDbConnection connection = new SqliteConnection(connectionURL);
        connection.Open();

        IDbCommand EditCommand = connection.CreateCommand();
        bool a = int.TryParse(editID, out int eID);
        EditCommand.CommandText = "update curriculum set problem_text='"+problem+"', answer='"+answer+"' where c_id="+(eID+1);
        IDataReader EditReader = EditCommand.ExecuteReader();

        EditCommand.Dispose();
        EditCommand = null;
        EditReader.Dispose();
        EditReader = null;
        connection.Close();
        connection = null;
        editProblemScreen.SetActive(false);

    }
    public void addNewProblem()
    {
        string problem = editProblemScreen.transform.Find("Panel").transform.Find("Problem").GetComponent<InputField>().text;
        string answer = editProblemScreen.transform.Find("Panel").transform.Find("Answer").GetComponent<InputField>().text;
        string languageText = editProblemScreen.transform.Find("Panel").transform.Find("Dropdown").GetComponent<Dropdown>().captionText.text;
        int language = 0;
        string difficulty = editProblemScreen.transform.Find("Panel").transform.Find("Difficulty").GetComponent<Dropdown>().captionText.text;
        Debug.Log("Difficulty = " + difficulty);
        Debug.Log("create new Problem ");
        
        switch (languageText)
        {
            case "Java":
                language = 1;
                break;
            case "Python":
                language = 2;
                break;
            default:
                //Debug.Log("None");
                break;
        }
        Debug.Log("Language = " + language);
        string pathDB = Path.Combine(Application.persistentDataPath, "ProgGames.db");
        string connectionURL = "URI=file:" + Application.dataPath + "/StreamingAssets/ProgGames.db";
        IDbConnection connection = new SqliteConnection(connectionURL);
        connection.Open();

        IDbCommand AddCommand = connection.CreateCommand();
        AddCommand.CommandText = "insert into curriculum(problem_text, language_id, teacher_id, answer, difficulty) values('" + problem + "',"+language+","+GameManager.instance.getUserID()+",'" + answer + "','"+difficulty+"')";
        IDataReader AddReader = AddCommand.ExecuteReader();

        AddCommand.Dispose();
        AddCommand = null;
        AddReader.Dispose();
        AddReader = null;

        IDbCommand CurriculumCommand = connection.CreateCommand();
        CurriculumCommand.CommandText = "select c_id from curriculum where problem_text='"+problem+"' and answer='" +answer+ "' and language_id="+language+" and difficulty='"+difficulty+"'";
        IDataReader CurriculumReader = CurriculumCommand.ExecuteReader();

        IDbCommand StudentCommand = connection.CreateCommand();
        StudentCommand.CommandText = "select id from users where user_type=1 and teacher='" + GameManager.instance.getUserID() + "'";
        IDataReader StudentReader = StudentCommand.ExecuteReader();


        while (StudentReader.Read())
        {
            bool parse = int.TryParse(StudentReader[0].ToString(), out int studentID);
            bool parseC = int.TryParse(CurriculumReader["c_id"].ToString(), out int cID);
            
            IDbCommand AddNotCompletedCommand = connection.CreateCommand();
            AddNotCompletedCommand.CommandText = "insert into notCompleted(curriculum_id, student_id) values("+ cID +","+ studentID +")";
            IDataReader AddNotCompletedReader = AddNotCompletedCommand.ExecuteReader();

            AddNotCompletedReader.Close();
            AddNotCompletedReader = null;
            AddNotCompletedCommand.Dispose();
            AddNotCompletedCommand = null;
        }


        CurriculumReader.Close();
        CurriculumReader = null;
        CurriculumCommand.Dispose();
        CurriculumCommand = null;
        StudentReader.Close();
        StudentReader = null;
        StudentCommand.Dispose();
        StudentCommand = null;
        connection.Close();
        connection = null;
    }
    public void cancelEditProblem()
    {
        editProblemScreen.SetActive(false);
        Debug.Log("cancel Problem");
    }
}
