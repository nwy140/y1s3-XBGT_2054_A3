using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Handles gameplay state enum and switches UI based on Gameplay state
public class GameplayManager : MonoBehaviour
{
	public static GameplayManager instance;

	public GameplayState CurrGameplayState;
	public float GameplayTimeScale = 1;

	// UI Objects to toggle relative to the current gameplay state
	public List<GameObject> UIObjs;
	public GameObject currUIObj;
	

	


	public enum GameplayState{
		GAME_RUNNING,
		GAME_PAUSE,
		GAME_GAMEOVER,
		GAME_GAMEWIN,
		GAME_ENDCLOSE
        
	}
    
	void Awake()
	{
		MakeInstance();
		// default gameplay timeScale
		Time.timeScale =  GameplayTimeScale;
		
		//Disable all UI objs	
		foreach (GameObject uiObj in UIObjs)
		{	
			 
			uiObj.SetActive(false);
		
		}
	}
	public void ToggleUIByIndex (int index)
	{	

		if(UIObjs[index]){
			
			if(UIObjs[index].activeInHierarchy==false){
				
				foreach (GameObject uiObj in UIObjs)
				{	
					uiObj.SetActive(false);
		
				}	
				UIObjs[index].SetActive(true);
			}
		}
	
	}

	void MakeInstance()
	{
		if (instance == null)
		{
			instance = this;
		}
	}
	
	void Update(){
		switch (CurrGameplayState)
		{
		case GameplayState.GAME_RUNNING:
			OnGameRunning();
			break;

		case GameplayState.GAME_PAUSE:
			OnGamePause();
			break;

		case GameplayState.GAME_GAMEOVER:
			OnGameOver();
			break;

		case GameplayState.GAME_GAMEWIN:
			OnGameWin();
			break;

		case GameplayState.GAME_ENDCLOSE:
			OnGameEndClose();
			break;

		}
		
		
		// cheats //  Increase time scale for debug purposes
		if(Input.GetKeyDown(KeyCode.RightBracket)){
			cheatIncreaseTimeScale();
		} else if(Input.GetKeyDown(KeyCode.LeftBracket)){
			cheatDecreaseTimeScale();
		}
	}
	

	
	// enum Game State
	
	void OnGameRunning(){
		gameObject.name = "Gameplay Manager State: " + CurrGameplayState.ToString() ;
		Time.timeScale = GameplayTimeScale;
		ToggleUIByIndex( (int)CurrGameplayState ) ;
		
		// hide mouse
		if(LevelManager.instance){
			LevelManager.instance.SetMouseVisibility(false);
		}
	}
	
	void OnGamePause(){
		gameObject.name = "Gameplay Manager State: " + CurrGameplayState.ToString() ;
		//Toggle UI by index of gameplay state enum
		ToggleUIByIndex( (int)CurrGameplayState ) ;

		Time.timeScale = 0;
		// show mouse
		if(LevelManager.instance){
			LevelManager.instance.SetMouseVisibility(true);
		}
	}
	
	void OnGameOver(){
		gameObject.name = "Gameplay Manager State: " + CurrGameplayState.ToString() ;
		
		ToggleUIByIndex( (int)CurrGameplayState ) ;
		
		Time.timeScale = 0;
		
		// show mouse
		if(LevelManager.instance){
			LevelManager.instance.SetMouseVisibility(true);
		}

	}
	
	void OnGameWin(){
		gameObject.name = "Gameplay Manager State: " + CurrGameplayState.ToString() ;
		ToggleUIByIndex( (int)CurrGameplayState ) ;
		
		Time.timeScale = 0;

		// show mouse
		if(LevelManager.instance){
			LevelManager.instance.SetMouseVisibility(true);
			if(LevelManager.instance.GetCurrentLevelIndex() != 2 ){
				LevelManager.instance.LoadLevelByIndex(2);
			}
		}


	}
	void OnGameEndClose(){
		gameObject.name = "Gameplay Manager State: " + CurrGameplayState.ToString() ;
		ToggleUIByIndex( (int)CurrGameplayState ) ;
	}
	
	
	// TODO Optional State, onAnyStateChange state
	
	
	//  Changing framerate/time scale is A commong debug feature found in game console emulators
	
	public void setGameplayTimeScale(float timeScaleVal){
		GameplayTimeScale = timeScaleVal;
	}

	public void cheatIncreaseTimeScale(){
		GameplayTimeScale++;
		Time.timeScale = GameplayTimeScale;
	}
	public void cheatDecreaseTimeScale(){
		if(GameplayTimeScale-1>=0){
			GameplayTimeScale--;
			Time.timeScale = GameplayTimeScale;
		}
	}
}
