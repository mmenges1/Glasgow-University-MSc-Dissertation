using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mono.Data.Sqlite;
using System.Data;
using System.IO;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class LoadScenes : MonoBehaviour
{
    public GameObject programDifficulty;
    int count = 0;

    //Loads a scene with name
    public void LoadScene(string level)
    {
        SceneManager.LoadScene(level);
        if (string.Equals(GameManager.instance.PreviousScene, "Login"))
        {
            //Gets all curriculum function after login scene
            getAllCurriculum();
        }
    }

    //Loads Student progress scene with student's ID
    public void LoadSceneStudentProgressList()
    {
        GameManager.instance.setStudentID(gameObject.transform.Find("Student ID").GetComponent<Text>().text);
        SceneManager.LoadScene("StudentProgressScene");
        if (string.Equals(GameManager.instance.PreviousScene, "Login"))
        {
            //Gets all curriculum function after login scene
            getAllCurriculum();
        }
    }

    /*
     * sets difficulty of game
     * gets curriculum
     * toggles the one-player and two-player options on screen
     */
    public void setDifficultyScene(GameObject playerCount)
    {
        GameManager.instance.difficulty = gameObject.GetComponent<Button>().name;
        getCurriculum();
        playerCount.SetActive(true);
    }

    /* 
     * Connect to ProgGames database to get Teacher's curriculum with 
     * specific difficulty and programming language chosen by player.
     * 
     * Sets <int,problems>, <problems,answers>, <problems,wrongAnswers>, and <problems,explanations> to Sorted Dictionaries in the GameManager for this game.
     * 
     * 
     * SQLite database connection reference: https://medium.com/@rizasif92/sqlite-and-unity-how-to-do-it-right-31991712190
     */

    private void getCurriculum()
    {
        string pathDB = System.IO.Path.Combine(Application.persistentDataPath, "ProgGames.db");
        string connectionURL = "URI=file:" + Application.dataPath + "/StreamingAssets/ProgGames.db";
        IDbConnection GetCurriculumConnection = new SqliteConnection(connectionURL);
        GetCurriculumConnection.Open();
        IDbCommand GetCurriculumCommand = GetCurriculumConnection.CreateCommand();
        GetCurriculumCommand.CommandText = "select problem_text, answer, otherOptions, explanation from curriculum where difficulty='"+GameManager.instance.difficulty+"' and teacher_id=" + GameManager.instance.getTeacherID() +" and language_id="+GameManager.instance.gameLanguage;
        IDataReader GetCurriculumReader = GetCurriculumCommand.ExecuteReader();
        while (GetCurriculumReader.Read())
        {
            GameManager.instance.problems[count] = GetCurriculumReader["problem_text"].ToString();
            count++;
            GameManager.instance.curriculum[GetCurriculumReader["problem_text"].ToString()] = GetCurriculumReader["answer"].ToString();
            GameManager.instance.wrongOptions[GetCurriculumReader["problem_text"].ToString()] = GetCurriculumReader["otherOptions"].ToString();
            GameManager.instance.explanation[GetCurriculumReader["problem_text"].ToString()] = GetCurriculumReader["explanation"].ToString();
        }

        GetCurriculumReader.Close();
        GetCurriculumReader = null;
        GetCurriculumCommand.Dispose();
        GetCurriculumCommand = null;
        GetCurriculumConnection.Close();
        GetCurriculumConnection = null;
    }

    /* 
     * Connect to ProgGames database to get all of Teacher's curriculum.
     * 
     * Sets <int,problems>, <problems,answers>, <problems,wrongAnswers>, and <problems,explanations> to Sorted Dictionaries in the GameManager for this game.
     * 
     * 
     * SQLite database connection reference: https://medium.com/@rizasif92/sqlite-and-unity-how-to-do-it-right-31991712190
     */
    private void getAllCurriculum()
    {
        string pathDB = System.IO.Path.Combine(Application.persistentDataPath, "ProgGames.db");
        string connectionURL = "URI=file:" + Application.dataPath + "/StreamingAssets/ProgGames.db";
        IDbConnection GetCurriculumConnection = new SqliteConnection(connectionURL);
        GetCurriculumConnection.Open();
        IDbCommand GetCurriculumCommand = GetCurriculumConnection.CreateCommand();
        GetCurriculumCommand.CommandText = "select problem_text, answer, otherOptions, explanation from curriculum where teacher_id=" + GameManager.instance.getUserID();
        IDataReader GetCurriculumReader = GetCurriculumCommand.ExecuteReader();
        while (GetCurriculumReader.Read())
        {
            GameManager.instance.problems[count] = GetCurriculumReader["problem_text"].ToString();
            count++;
            GameManager.instance.curriculum[GetCurriculumReader["problem_text"].ToString()] = GetCurriculumReader["answer"].ToString();
            GameManager.instance.wrongOptions[GetCurriculumReader["problem_text"].ToString()] = GetCurriculumReader["otherOptions"].ToString();
            GameManager.instance.explanation[GetCurriculumReader["problem_text"].ToString()] = GetCurriculumReader["explanation"].ToString();

        }

        GetCurriculumReader.Close();
        GetCurriculumReader = null;
        GetCurriculumCommand.Dispose();
        GetCurriculumCommand = null;
        GetCurriculumConnection.Close();
        GetCurriculumConnection = null;
    }
}
