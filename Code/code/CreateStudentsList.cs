using UnityEngine;
using Mono.Data.Sqlite;
using System.Data;
using System.IO;
using UnityEngine.UI;

public class CreateStudentsList : MonoBehaviour
{
    //Student list prefab game object for scroll view
    public GameObject CreateStudentsListPrefab;
    //Scroll view for student list
    public GameObject scrollViewContentPanel;
    /****** Called when the teacher goes to the student progress scene *******/
    void Start()
    {
        /*
         * Connects and opens SQLite Database ProgGames.db
         * SQLite database connection reference: https://medium.com/@rizasif92/sqlite-and-unity-how-to-do-it-right-31991712190
         */
        string pathDB = System.IO.Path.Combine(Application.persistentDataPath, "ProgGames.db");
        string connectionURL = "URI=file:" + Application.dataPath + "/StreamingAssets/ProgGames.db";
        IDbConnection connection = new SqliteConnection(connectionURL);
        connection.Open();
        //Creates SQL command to select and read all of the students with the teacher id of the logged in user
        IDbCommand StudentCommand = connection.CreateCommand();
        StudentCommand.CommandText = "select * from users where user_type=1 and teacher='" + GameManager.instance.getUserID() + "'";
        IDataReader StudentReader = StudentCommand.ExecuteReader();
        float PercentCorrect = -1;
        while (StudentReader.Read())
        {
            //Create prefab game object for the student list
            GameObject newStudentForList = Instantiate(CreateStudentsListPrefab, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
            /*for each UI component in the student list prefab update their information with the following:
                Student's Name
                Programming Language
                Percent Correct
                Percent Not Completed
                Student's Unique ID
            */
            foreach (Transform child in newStudentForList.transform)
            {
                string StudentId = StudentReader[0].ToString();
                string TeacherId = StudentReader[5].ToString();
                //get total count of Teacher's Java curriculum
                IDbCommand CurriculumJavaCommand = connection.CreateCommand();
                CurriculumJavaCommand.CommandText = "select COUNT(c_id) from curriculum where language_id=1 and teacher_id="+ TeacherId;
                IDataReader CurriculumReader = CurriculumJavaCommand.ExecuteReader();
                string JavaCurriculum = CurriculumReader[0].ToString();

                //get total count of Teacher's Python curriculum
                IDbCommand CurriculumPythonCommand = connection.CreateCommand();
                CurriculumPythonCommand.CommandText = "select COUNT(c_id) from curriculum where language_id=2 and teacher_id="+ TeacherId;
                IDataReader CurriculumPythonReader = CurriculumPythonCommand.ExecuteReader();
                string PythonCurriculum = CurriculumPythonReader[0].ToString();

                //get total count of Teacher's Java curriculum where the Student has completed the problem
                IDbCommand CorrectJavaCommand = connection.CreateCommand();
                CorrectJavaCommand.CommandText = "select COUNT(c_id) from curriculum where language_id=1 and teacher_id=" + TeacherId +" and c_id=(select curriculum_id from completed where student_id="+StudentId+")";
                IDataReader CorrectJavaReader = CorrectJavaCommand.ExecuteReader();
                string JavaCorrect = CorrectJavaReader[0].ToString();

                //get total count of Teacher's Python curriculum where the Student has completed the problem
                IDbCommand CorrectPythonCommand = connection.CreateCommand();
                CorrectPythonCommand.CommandText = "select COUNT(c_id) from curriculum where language_id=2 and teacher_id=" + TeacherId + " and c_id=(select curriculum_id from completed where student_id=" + StudentId + ")";
                IDataReader CorrectPythonReader = CorrectPythonCommand.ExecuteReader();
                string PythonCorrect = CorrectPythonReader[0].ToString();

                switch (child.name)
                {
                    case "Student Name":
                        child.name = StudentReader[2].ToString();
                        Text StudentText = child.GetComponent<Text>();
                        StudentText.text = StudentReader[2].ToString();
                        break;
                    case "Language 1":
                        child.name = "Java";
                        Text JavaText = child.GetComponent<Text>();
                        JavaText.text = "Java";
                        PercentCorrect = float.Parse(JavaCorrect) / float.Parse(JavaCurriculum);
                        break;
                    case "Language 2":
                        child.name = "Python";
                        Text PythonText = child.GetComponent<Text>();
                        PythonText.text = "Python";
                        PercentCorrect = float.Parse(PythonCorrect) / float.Parse(PythonCurriculum);
                        break;
                    case "% Correct":
                        child.name = PercentCorrect+"% Correct" ;
                        Text CorrectText = child.GetComponent<Text>();
                        CorrectText.text = PercentCorrect+"% Correct";
                        break;
                    case "Not Completed":
                        child.name = "% Not Completed";
                        Text NotCompletedText = child.GetComponent<Text>();
                        NotCompletedText.text = (100.0 - PercentCorrect) + "% Not Completed";
                        break;
                    case "Student ID":
                        child.name = "Student ID";
                        Text studentID = child.GetComponent<Text>();
                        studentID.text = StudentId;
                        break;
                    default:
                        break;
                }
                newStudentForList.transform.parent = scrollViewContentPanel.transform;
                CurriculumPythonReader.Close();
                CurriculumPythonReader = null;
                CurriculumPythonCommand.Dispose();
                CurriculumPythonCommand = null;

                CurriculumReader.Close();
                CurriculumReader = null;
                CurriculumJavaCommand.Dispose();
                CurriculumJavaCommand = null;

                CorrectJavaReader.Close();
                CorrectJavaReader = null;
                CorrectJavaCommand.Dispose();
                CorrectJavaCommand = null;

                CorrectPythonReader.Close();
                CorrectPythonReader = null;
                CorrectPythonCommand.Dispose();
                CorrectPythonCommand = null;
            }
        }

        StudentReader.Close();
        StudentReader = null;
        StudentCommand.Dispose();
        StudentCommand = null;
        connection.Close();
        connection = null;
    }
}
