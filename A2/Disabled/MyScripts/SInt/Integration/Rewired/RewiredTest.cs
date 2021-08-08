using System.Collections;
using System.Collections.Generic;
using Rewired;
using UnityEngine;

// Rewired Documentation: https://guavaman.com/projects/rewired/docs/QuickStart.html
public class RewiredTest : MonoBehaviour
{
    private int playerID = 0; //Player1 uses the playerID 0 
    private Player player;

    // Use Awake in parent/base classes, use Start in derived/child classes
    void Awake()
    {
        initRewiredInputSystem();
    }

    void Update()
    {
        rewiredBindInputControls();
    }


    // Override this method and put all input initalisation code here
    void initRewiredInputSystem()
    {
        //Init Rewired input system
        player = ReInput.players.GetPlayer(playerID);
    }

    // Override this method and put all input binding related code here
    void rewiredBindInputControls() //TODO: convert to interface
    {
        /// Rewired Input Bindings
        // if(GetRewiredPlayer().Ge)
    }

    // Gets the Rewired Player Reference // To be called in child classes
    Player GetRewiredPlayer()
    {
        return player;
    }
}