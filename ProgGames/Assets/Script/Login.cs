using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mono.Data.Sqlite;
using System.Data;
using System.IO;

public class Login : MonoBehaviour
{
    public GameObject username;
    public GameObject password;

    public void getLoginInput()
    {
        string UsernameInputField = username.GetComponent<InputField>().text;
        string PasswordInputField = password.GetComponent<InputField>().text;

        //Debug.Log(UsernameInputField);
       // Debug.Log(PasswordInputField);


        string pathDB = System.IO.Path.Combine(Application.persistentDataPath, "ProgGames.db");
        //  string connectionURL = Application.streamingAssetsPath+ "/ProgGames.db";
        string connectionURL = "URI=file:" + Application.dataPath + "/StreamingAssets/ProgGames.db";
        IDbConnection LoginConnection = new SqliteConnection(connectionURL);
        LoginConnection.Open();
        IDbCommand LoginCommand = LoginConnection.CreateCommand();
        LoginCommand.CommandText = "select * from users where user_id='" + UsernameInputField + "' and user_password='" + PasswordInputField+"'";
        IDataReader LoginReader = LoginCommand.ExecuteReader();
        string LoginUserID = LoginReader[3].ToString();
        string LoginUserNumber = LoginReader[0].ToString();
        string UserType = LoginReader[1].ToString();
        string UserName = LoginReader[2].ToString();
        string UserTeacherID = LoginReader[5].ToString();
      //  Debug.Log("id: " + LoginUserID);

        LoginReader.Close();
        LoginReader = null;
        LoginCommand.Dispose();
        LoginCommand = null;
        LoginConnection.Close();
        LoginConnection = null;

        if (LoginUserID != "")
        {
            if(int.TryParse(UsernameInputField, out int n))
            {
                bool lID = int.TryParse(LoginUserNumber, out int result);
                bool a = int.TryParse(UserTeacherID, out int num);
                bool b = int.TryParse(UserType, out int tID);
                GameManager.instance.createUser(result, num, UserName, tID);
                GameManager.instance.PreviousScene = "Login";
                Application.LoadLevel("StudentTitleScreen");
            }
            else
            {
                bool lID = int.TryParse(LoginUserNumber, out int result);
                bool a = int.TryParse(UserType, out int num);
                GameManager.instance.createUser(result, 0, UserName, num);
                GameManager.instance.PreviousScene = "Login";
                Application.LoadLevel("TeacherTitleScreen");
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
