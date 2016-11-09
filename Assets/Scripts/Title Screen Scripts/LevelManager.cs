using UnityEngine;
using System.Collections;


public class LevelManager : MonoBehaviour {


	public void LoadMenu(string name)
    {
		UnityEngine.SceneManagement.SceneManager.LoadScene(name);
    }

	public void SelectionScreenMenu(string name)
	{
		UnityEngine.SceneManagement.SceneManager.LoadScene("Mode Select");
	}

	public void TrackGame(string name)
	{
		UnityEngine.SceneManagement.SceneManager.LoadScene("Track");
	}

	public void RingGame(string name)
	{
		UnityEngine.SceneManagement.SceneManager.LoadScene("Ring");
	}
    
    public void ArenaGame(string name)
	{
		UnityEngine.SceneManagement.SceneManager.LoadScene("Arena");
	}

	public void QuitGame()
	{
		Application.Quit();
	}
}
