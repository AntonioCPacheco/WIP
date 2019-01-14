using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System.IO;
using UnityEngine.SceneManagement;

public class Game_PauseManager : MonoBehaviour {

	bool _paused;
	GameObject auxObj;
    bool alreadyPressed = false;
    GameObject introScreen;
    
    //Controls
    public GameObject controlsScreen;

    public GameObject psControls;
    public GameObject xboxControls;
    public GameObject keyboardControls;

    // Use this for initialization
    void Start () {
        Time.timeScale = 0;
		_paused = false;
		
		auxObj = transform.GetChild(0).gameObject;
		auxObj.SetActive (false);

        transform.GetChild(1).gameObject.SetActive(true);

        introScreen = GameObject.Find("IntroScreen");

        bool isFile = File.Exists(Application.persistentDataPath + "/saveData.dat");
        GameObject.Find("Continue").SetActive(isFile);
    }
	
	// Update is called once per frame
	void Update () {
		if ((!alreadyPressed && MyInput.GetPause()) || (MyInput.GetUnpause() && _paused))
        {
            alreadyPressed = true;
			if (_paused) {
				Unpause ();
			} else {
				Pause ();
			}
		}
        else if(!MyInput.GetPause())
        {
            alreadyPressed = false;
        }


        MyInput.updateController();
        switch (MyInput.getControllerType())
        {
            case (-1):
                psControls.SetActive(false);
                xboxControls.SetActive(false);
                keyboardControls.SetActive(true);
                break;
            case (0):
                psControls.SetActive(false);
                xboxControls.SetActive(true);
                keyboardControls.SetActive(false);
                break;
            case (1):
                psControls.SetActive(true);
                xboxControls.SetActive(false);
                keyboardControls.SetActive(false);
                break;
        }

        GameObject.Find("EventSystem").GetComponent<GUI_FirstSelected>().setSubmitButton();
        GameObject.Find("EventSystem").GetComponent<GUI_FirstSelected>().setCancelButton();
    }
    
	public void Unpause(){
		Time.timeScale = 1f;
		_paused = false;
		auxObj.SetActive(false);
        //Cursor.visible = false;
	}

	public void Pause(){
		_paused = true;
		auxObj.SetActive(true);
        GameObject.Find("EventSystem").GetComponent<GUI_FirstSelected>().setFirstSelected(GameObject.Find("Pause_Resume"));
        Cursor.visible = true;
        Time.timeScale = 0f;
        VibrateController.stopVibrations();
    }

    public void Quit()
    {
        VibrateController.stopVibrations();
        SceneManager.LoadScene(0);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void restartLevel()
    {
        if(_paused) Unpause();
        SaveManager.loadGame();
    }

    public void MainMenu()
    {
        stopAtmosphericAndMusic();
        introScreen.SetActive(true);
        GameObject.Find("DeathScreen").GetComponent<Animator>().SetTrigger("StartGame");
        //GameObject.Find("EventSystem").GetComponent<GUI_FirstSelected>().setFirstSelected(GameObject.Find("Play"));
        Time.timeScale = 0;
        VibrateController.stopVibrations();
    }

    public void StartGame()
    {
        introScreen.SetActive(false);
        controlsScreen.SetActive(true);
        GameObject.Find("EventSystem").GetComponent<GUI_FirstSelected>().setFirstSelected(GameObject.Find("Ok"));
    }

    public void ControlsScreenContinue()
    {
        FindObjectOfType<DialogueManager>().initializeTriggers();
        StartCoroutine(muteAndWait());
        Time.timeScale = 1;
        SaveManager.loadFirstLevel();
        controlsScreen.SetActive(false);
        GameObject.Find("DeathScreen").GetComponent<Animator>().SetTrigger("StartGame");

        playAtmosphericAndMusic();
    }

    public void ContinueFromSave()
    {
        StartCoroutine(muteAndWait());
        introScreen.SetActive(false);
        SaveManager.loadGame();
        GameObject.Find("DeathScreen").GetComponent<Animator>().SetTrigger("StartGame");
        playAtmosphericAndMusic();
    }

    IEnumerator muteAndWait()
    {
        AudioListener.volume = 0;
        Time.timeScale = 1;
        yield return new WaitForSeconds(2f);
        float alpha = 0;
        float start = Time.realtimeSinceStartup;
        while (alpha < 1)
        {
            AudioListener.volume = Mathf.Lerp(0, 1, alpha);
            alpha = (Time.realtimeSinceStartup - start) / 3;
            yield return null;
        }
    }

    void playAtmosphericAndMusic()
    {
        foreach (AudioSource s in Camera.main.GetComponents<AudioSource>())
        {
            if(!s.isPlaying && s.clip.name != "FinalSceneMusic")
                s.Play();
        }
    }

    void stopAtmosphericAndMusic()
    {
        foreach (AudioSource s in Camera.main.GetComponents<AudioSource>())
        {
            if(s.isPlaying)
                s.Stop();
        }
    }
}
