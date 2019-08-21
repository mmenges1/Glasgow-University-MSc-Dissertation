using UnityEngine;
using Mono.Data.Sqlite;
using System.Data;
using System.IO;
using UnityEngine.UI;

public class ConnectToDatabase : MonoBehaviour
{
    void Start(){
        string pathDB = System.IO.Path.Combine(Application.persistentDataPath, "ProgGames.db");
      //  string connectionURL = Application.streamingAssetsPath+ "/ProgGames.db";
        string connectionURL = "URI=file:" + Application.dataPath + "/ProgGames.db";
        Debug.Log(connectionURL);
        IDbConnection connection = new SqliteConnection(connectionURL);
        connection.Open();
        Debug.Log(connection.State);
        IDbCommand Command = connection.CreateCommand();
        Command.CommandText = "select * from user_type";
        IDataReader ThisReader = Command.ExecuteReader();
        while (ThisReader.Read())
        {
            Debug.Log("id: " + ThisReader[0].ToString());
            Debug.Log("name: " + ThisReader[1].ToString());
        }
        ThisReader.Close();
        ThisReader = null;
        Command.Dispose();
        Command = null;
        connection.Close();
        connection = null;
    }
}
