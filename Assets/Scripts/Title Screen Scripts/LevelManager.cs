using UnityEngine;
using System.Collections;


public class LevelManager : MonoBehaviour {


	public void LoadMenu(string name)
    {
		UnityEngine.SceneManagement.SceneManager.LoadScene(name) ;
		UnityEngine.SceneManagement.SceneManager.LoadScene("Main");
    }

	public void SelectionScreenMenu(string name)
	{
		UnityEngine.SceneManagement.SceneManager.LoadScene ("Mode Select");
	}

	public void TrackGame(string name)
	{
		UnityEngine.SceneManagement.SceneManager.LoadScene ("Track");
	}

	public void ArenaGame(string name)
	{
		UnityEngine.SceneManagement.SceneManager.LoadScene ("Islands");
	}

	public void QuitGame()
	{
		Application.Quit ();
	}
}
