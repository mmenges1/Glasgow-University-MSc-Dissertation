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
    public GameObject gameID;
    public GameObject incorrectPassword;
    /*
     * SQLite database connection reference: https://medium.com/@rizasif92/sqlite-and-unity-how-to-do-it-right-31991712190
     * Get username and password entered by player
     * get user from database who has the same username and password
     * if the user does not exist or the password is wrong
     *      display error message
     * else
     *      check if username is Char String or Integer
     *          if integer set user information and load student's title scene
     *          else set user data and load teacher's title scene
     */
    public void getLoginInput()
    {
        string UsernameInputField = username.GetComponent<InputField>().text;
        string PasswordInputField = password.GetComponent<InputField>().text;
        GameManager.instance.gameID = gameID.GetComponent<InputField>().text;

        string pathDB = System.IO.Path.Combine(Application.persistentDataPath, "ProgGames.db");
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
        else
        {
            incorrectPassword.SetActive(true);
        }
    }
}
