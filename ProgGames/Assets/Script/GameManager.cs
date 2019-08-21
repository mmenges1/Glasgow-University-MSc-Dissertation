using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public string PreviousScene;
    string currentScene;
    public string programmingLanguage;
    public string difficulty;
    public int gameLanguage = 0;
    public GameObject currentPlayerCollision;
    public GameObject questionNumber;
    public bool isLadder = false;
    public int treasureCount = 0;
    public int playerCount = 1;
    public int count = 0;
    public string[] problemsSolved = { " ", " ", " ", " ", " ", " ", " ", " " };
    public SortedDictionary<int, string> problems = new SortedDictionary<int, string>();
    public SortedDictionary<string, string> curriculum = new SortedDictionary<string, string>();
    public SortedDictionary<string, string> wrongOptions = new SortedDictionary<string, string>();
    public SortedDictionary<string, string> explanation = new SortedDictionary<string, string>();

    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
        Screen.fullScreen = false;
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setGameLanguage()
    {
        switch (programmingLanguage)
        {
            case "Python":
                gameLanguage = 2;
                break;
            case "Java":
                gameLanguage = 1;
                break;
            default:
                gameLanguage = 0;
                break;
        }
    }

    /// <summary>
    /// Current Logged In User
    /// </summary>
    protected int userID;
    protected int teacherID;
    protected string userName;
    protected int userType;
    protected string studentID;
    public void createUser(int id, int tID, string name, int type)
    {
        setUserName(name);
        setUserType(type);
        setTeacherID(tID);
        setUserID(id);
    }
    public void setStudentID(string sID)
    {
        studentID = sID;
    }

    public string getStudentID()
    {
        return studentID;
    }

    public string getUserName()
    {
        return userName;
    }
    protected void setUserName(string usersname)
    {
        userName = usersname;
    }

    public int getUserID()
    {
        return userID;
    }
    protected void setUserID(int userid)
    {
        userID = userid;
    }

    public int getTeacherID()
    {
        return teacherID;
    }
    protected void setTeacherID(int teachID)
    {
        teacherID = teachID;
    }

    public int getUserType()
    {
        return userType;
    }
    protected void setUserType(int userTyp)
    {
        userType = userTyp;
    }
}