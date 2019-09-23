using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class GameTimer : MonoBehaviour
{
    /* 
     * In game timer created and updated to reflect a 200 second time for the One-Player
     * In game treasure count for player to know if they have found all objectives
     * Resource used and adapted to create an on screen Timer: https://techwithsach.com/how-to-add-a-simple-countdown-timer-in-unity/ 
     */
    private float playTime = 200f;
    public Text playTimeText;
    public Text treasureLeftUI;

    void Start()
    {
        treasureLeftUI.text = "Treasures Left: " + GameManager.instance.treasureCount.ToString();
    }
    void Update()
    {
        int.TryParse(treasureLeftUI.text.Split(':')[1], out int result);
        if (result != GameManager.instance.treasureCount)
        {
            treasureLeftUI.text = "Treasures Left: "+GameManager.instance.treasureCount.ToString();
        }

        playTime = playTime - Time.deltaTime;
        playTimeText.text = "Time Left: "+((int)playTime).ToString();
        if(playTime <= 0)
        {
            SceneManager.LoadScene("Results");
        }
    }
}
