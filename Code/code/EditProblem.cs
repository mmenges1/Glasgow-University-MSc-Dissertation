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
    protected GameObject temporaryEdit;
    /*
     * Find UI Game object in game scene and replace the information with the problem the user selected's question and answer.
     * Save the problem ID as in inactive gameobject in order to save it after edits are complete.
     */
    public void editProblem()
    {
        GameObject editPanel = null;
        foreach (GameObject a in UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects())
        {
            if(a.name.Contains("ProgGamesUIManager"))
            {
                editPanel = a.transform.Find("MainPanel").transform.Find("EditPanel").gameObject;
                temporaryEdit = editPanel;
            }
        }
        if (temporaryEdit != null)
        {
            temporaryEdit.SetActive(true);
            temporaryEdit.transform.Find("ProblemToEdit").GetComponent<InputField>().text = gameObject.transform.parent.Find("Problem").GetComponent<Text>().text;
            temporaryEdit.transform.Find("Answer").GetComponent<InputField>().text = gameObject.transform.parent.Find("Answer Text").GetComponent<Text>().text;
            temporaryEdit.transform.Find("Problem ID").GetComponent<Text>().text = gameObject.transform.parent.Find("Problem ID").GetComponent<Text>().text;
        }
    }
    /*
     * Get the current panels information and update it's question and answer to to the database correlating with the question's ID
     * saved in the editProblem() function.
     * 
     * SQLite database connection reference: https://medium.com/@rizasif92/sqlite-and-unity-how-to-do-it-right-31991712190
     */
    public void saveEditProblem()
    {
        GameObject tempEditPanel = GameObject.Find("EditPanel");
        string problem = tempEditPanel.transform.Find("ProblemToEdit").GetComponent<InputField>().text;
        string answer = tempEditPanel.transform.Find("Answer").GetComponent<InputField>().text;

        string editID = tempEditPanel.transform.Find("Problem ID").GetComponent<Text>().text;

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
        tempEditPanel.SetActive(false);

    }

    /*
     * Once the player selects the add new problem button in the scene, the information entered into the inputs are gathered below.
     * They are then saved to the database as a new entry in the curriculum table. All new problems are then saved in the notComplete table 
     * of the database to ensure all student's get the chance to answer those questions.
     */
    public void addNewProblem()
    {
        string problem = editProblemScreen.transform.Find("Panel").transform.Find("Problem").GetComponent<InputField>().text;
        string answer = editProblemScreen.transform.Find("Panel").transform.Find("Answer").GetComponent<InputField>().text;
        string languageText = editProblemScreen.transform.Find("Panel").transform.Find("Dropdown").GetComponent<Dropdown>().captionText.text;
        int language = 0;
        string difficulty = editProblemScreen.transform.Find("Panel").transform.Find("Difficulty").GetComponent<Dropdown>().captionText.text;
        
        switch (languageText)
        {
            case "Java":
                language = 1;
                break;
            case "Python":
                language = 2;
                break;
            default:
                break;
        }
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

    //Cancel out of the editing screen and set it's UI to inactive
    public void cancelEditProblem()
    {
        editProblemScreen.SetActive(false);
    }
}
