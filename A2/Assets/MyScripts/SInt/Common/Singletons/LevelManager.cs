using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
	
	// Example Doxygen Documentation Convention
	/**
	 * @file
	 * @author  John Doe <jdoe@example.com>
	 * @version 1.0
	 * @section DESCRIPTION
	 *
	 * The time class represents a moment of time.
	 */
public class LevelManager : MonoBehaviour
{

	public static LevelManager instance;
	
	// Force mouse to be visible at all times
	public bool isForceMouseAlwaysVisible = false;
	

	public void Awake()
	{
		MakeInstance();

	}

	void Update()
	{

		// For quick level switching in development only
		/// Load first level by index or second level by index
		//if (Input.GetKeyDown(KeyCode.Alpha1))    
		//{
		//	LoadLevelByIndex(0);
		//}
		//else if (Input.GetKeyDown(KeyCode.Alpha2)) {
		//	LoadLevelByIndex(1);

		//}
		//Always show mouse on Menu

	    
	}
	void MakeInstance()
	{
		if (instance == null)
		{
			instance = this;
		}
	}
	public void LoadLevel(string Name)
	{
		gameObject.name = "Level Manager is trying to: " + "Load level name by " + Name ;

		Debug.Log("Level Load requeted for : " + Name);
		SceneManager.LoadScene(Name);

	}

	public void RestartLevel()
	{
		gameObject.name = "Level Manager is trying to: " + "Restart the level" ;

		Debug.Log("Level Load requeted for : " + "Restart Level");
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}


	public void RequestQuit()
	{
		Debug.Log("I want to quit");
		Application.Quit();
	}
	public void LoadNextLevel()
	{
		gameObject.name = "Level Manager is trying to: " + "Load next Level " ;
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
	}
	
	public int GetCurrentLevelIndex(){
		return SceneManager.GetActiveScene().buildIndex;
	}

	public void LoadLevelByIndex(int index)
	{
		gameObject.name = "Level Manager is trying to: " + "Load level index by " + index  ;

		Debug.Log("Level Load requeted for index : " + index);
		SceneManager.LoadScene(index);

		// show mouse on menu


	}

	public void SetMouseVisibility(bool isVisible)
	{

		if(isForceMouseAlwaysVisible == false)
			Cursor.visible = isVisible;
		else
			Cursor.visible = true;
	}
 
	public void PauseLevel()
	{
		gameObject.name = "Level Manager is trying to: " + "Pause the level" ;
		if(GameplayManager.instance){
			// You can only pause the game when Gameplay State is in running state
			if(GameplayManager.instance.CurrGameplayState == GameplayManager.GameplayState.GAME_RUNNING){
				GameplayManager.instance.CurrGameplayState = GameplayManager.GameplayState.GAME_PAUSE;
			}
		}
	}

	public void unPauseLevel()
	{
		gameObject.name = "Level Manager is trying to: " + "UnPause the level" ;

		if(GameplayManager.instance){
			GameplayManager.instance.CurrGameplayState = GameplayManager.GameplayState.GAME_RUNNING;
		}

	}
	


}