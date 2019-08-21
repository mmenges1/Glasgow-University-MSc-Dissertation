using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mono.Data.Sqlite;
using System.Data;
using System.IO;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class LoadScenes : NetworkBehaviour
{
    public GameObject programDifficulty;
    int count = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void LoadScene(string level)
    {
        if (string.Equals(SceneManager.GetActiveScene().name, "Results"))
        {
            
            SceneManager.LoadScene(NetworkManager.networkSceneName);
        }
         SceneManager.LoadScene(level);
        if (string.Equals(GameManager.instance.PreviousScene, "Login"))
        {
            getAllCurriculum();
        }
    }

    public void LoadScene2(string level)
    {
        if(isServer) GameObject.Find("LobbyManager").transform.GetComponent<NetworkLobbyManager>().StopServer();
        if(!isClient) GameObject.Find("LobbyManager").transform.GetComponent<NetworkLobbyManager>().StopHost();
        if(isClient) GameObject.Find("LobbyManager").transform.GetComponent<NetworkLobbyManager>().StopClient();

    }


    public void LoadSceneStudentProgressList()
    {
        GameManager.instance.setStudentID(gameObject.transform.Find("Student ID").GetComponent<Text>().text);
        SceneManager.LoadScene("StudentProgressScene");
        if (string.Equals(GameManager.instance.PreviousScene, "Login"))
        {
        //    Debug.Log("Previous was login");
            getAllCurriculum();
  //          Debug.Log(GameManager.instance.getUserName());
    //        Debug.Log(GameManager.instance.getUserType());
      //      Debug.Log(GameManager.instance.getTeacherID());
        //    Debug.Log(GameManager.instance.getUserID());
        }
    }

    public void setDifficultyScene(GameObject playerCount)
    {
     //   Debug.Log(gameObject.GetComponent<Button>().name);
        GameManager.instance.difficulty = gameObject.GetComponent<Button>().name;
     //   Debug.Log(GameManager.instance.programmingLanguage);
        getCurriculum();
        playerCount.SetActive(true);
    }

    private void getCurriculum()
    {
     //   Debug.Log("Getting Curriculum");
        string pathDB = System.IO.Path.Combine(Application.persistentDataPath, "ProgGames.db");
        //  string connectionURL = Application.streamingAssetsPath+ "/ProgGames.db";
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

    private void getAllCurriculum()
    {
     //   Debug.Log("Getting Curriculum");
        string pathDB = System.IO.Path.Combine(Application.persistentDataPath, "ProgGames.db");
        //  string connectionURL = Application.streamingAssetsPath+ "/ProgGames.db";
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

    // Update is called once per frame
    void Update()
    {
        
    }
}
