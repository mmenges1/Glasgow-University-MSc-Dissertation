﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CreateTreasures : NetworkBehaviour
{
    public override void OnStartServer()
    {
       // NetworkServer.Spawn(GameManager.instance.gameObject);
        //Spawns All Treasures into the gameplay of all servers
        NetworkServer.Spawn(gameObject);
    }
}
