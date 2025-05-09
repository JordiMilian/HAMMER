using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaOca_Pseudocode : MonoBehaviour
{
    
}
public class OcaGameController
{
    Board gameBoard;
    public enum GameState
    {
        StartBoard, RollingDice, MovingPlayer, FreeMode, ReachedEnd, KilledEnemy, PlayerDied
    }
    public GameState currentGameState;
    void SetUpBoard()
    {
        const int boardWidth = 10;
        const int boardHeight = 10;
    }
    public void ChangeGameState(GameState newState)
    {
        switch (currentGameState)
        {
            case GameState.StartBoard:
                
                break;
            case GameState.RollingDice:
                //hide dices UI
                break;
            case GameState.MovingPlayer:
                break;
            case GameState.FreeMode:
                break;
            case GameState.ReachedEnd:
                break;
            case GameState.KilledEnemy:
                break;
            case GameState.PlayerDied:
                break;
        }

        switch (newState)
        {
            case GameState.StartBoard:
                //Set up the game board
                //place player in start position
                //introduce the enemy 
                //introduce the player
                //change to freemode
                break;
            case GameState.RollingDice:
                //called from UI I guess
                //Show the dices and roll them
                //change to moving player mode and start moving it
                break;
            case GameState.MovingPlayer:
                //focus camera on player peace
                break;
            case GameState.FreeMode:
                //zoom out the camera
                //enable shop items purchase
                break;
            case GameState.ReachedEnd:
                //deal damage to the enemy
                //if it dies change to killed
                //else, return to start and change to free mode (or not?)
                break;
            case GameState.KilledEnemy:
                //Receive money
                //Display some victory screen
                //return to start and change to free mode
                break;
            case GameState.PlayerDied:
                //called when not killing the dude before X turns
                //SHow game over screen
                //restart game
                break;
        }
        currentGameState = newState;
    }
}
public class Board
{
    public List<BoardTile> tiles;
}
public abstract class BoardTile : ScriptableObject
{
    public Vector3 WorldPosition;
    public int Index;
    public abstract void OnPlayerCrossed();
    public abstract void OnPlayerLanded();

}
