using UnityEngine.Networking;

public class StillPlayingCheck : NetworkBehaviour
{
    //Update count of Treasures
    [SyncVar] private int treasuresLeft = 8;
    
    //Set treasure count
    public void setTreasuresLeft(int treasures)
    {
        treasuresLeft = treasures;
    }

    //get treasure count
    public int getTreasuresLeft()
    {
        return treasuresLeft;
    }
}
