using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestionScript : MonoBehaviour
{
    public TreasureScript treasureScript;
    // Start is called before the first frame update
    void Start()
    {
    }

    public void onAnswer()
    {
        Debug.Log(gameObject.name);
        treasureScript = GameManager.instance.currentPlayerCollision.transform.GetComponent<TreasureScript>();
        treasureScript.onAnswer(gameObject.transform.Find("Text").GetComponent<Text>().text);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
