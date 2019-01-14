using UnityEngine;
using System.Collections;

public class Game_SceneManager : MonoBehaviour {

//	static SceneManager instance;
//
//	// Use this for initialization
//	void Start () {
//		if (instance != null) {
//			GameObject.Destroy (gameObject);
//		} else {
//			GameObject.DontDestroyOnLoad (gameObject);
//			instance = this;
//		}
//	}

	void Start(){
        Application.targetFrameRate = 60;

		Physics2D.IgnoreLayerCollision (LayerMask.NameToLayer("Platforms"), LayerMask.NameToLayer("Eyes"));
		Physics2D.IgnoreLayerCollision (LayerMask.NameToLayer("Statues"), LayerMask.NameToLayer("Eyes"));
		Physics2D.IgnoreLayerCollision (LayerMask.NameToLayer("Crawlers"), LayerMask.NameToLayer("Eyes"));
        Physics2D.IgnoreLayerCollision (LayerMask.NameToLayer("Eyes"), LayerMask.NameToLayer("Eyes"));
		Physics2D.IgnoreLayerCollision (LayerMask.NameToLayer("Crawlers"), LayerMask.NameToLayer("Crawlers"));
        Physics2D.IgnoreLayerCollision (LayerMask.NameToLayer("Crawlers"), LayerMask.NameToLayer("Statues"));
    }
	
	// Update is called once per frame
	void Update () {
		
	}

	public void MainMenu()
    {
		UnityEngine.SceneManagement.SceneManager.LoadScene (0);
        Cursor.visible = true;
        //Application.LoadLevel (0);
    }

	public void NewGame()
    {
        //Cursor.visible = false;
        Time.timeScale = 1f;
		UnityEngine.SceneManagement.SceneManager.LoadScene (1);
		//Application.LoadLevel (1);
	}

	public void Quit(){
		Application.Quit ();
	}
}
