using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;

public class GUI_FirstSelected : MonoBehaviour {

    private EventSystem es;
	// Use this for initialization
	void Start () {
        es = this.GetComponent<EventSystem>();
	}
	
    public void setFirstSelected(GameObject firstSelected)
    {
        if (MyInput.getControllerType() != -1)
        {
            firstSelected.GetComponent<Button>().Select();
            firstSelected.GetComponent<Button>().OnSelect(null);

            es.SetSelectedGameObject(firstSelected);
        }
    }

    public void setSubmitButton()
    {
        switch (MyInput.getControllerType())
        {
            case (1):
                es.GetComponent<StandaloneInputModule>().submitButton = "AcceptPS";
                break;
            case (0):
                es.GetComponent<StandaloneInputModule>().submitButton = "AcceptXbox";
                break;
            case (-1):
                es.GetComponent<StandaloneInputModule>().submitButton = "AcceptKeyboard";
                break;
        }
    }

    public void setCancelButton()
    {
        switch (MyInput.getControllerType())
        {
            case (1):
                es.GetComponent<StandaloneInputModule>().cancelButton = "CancelPS";
                break;
            case (0):
                es.GetComponent<StandaloneInputModule>().cancelButton = "CancelXbox";
                break;
            case (-1):
                es.GetComponent<StandaloneInputModule>().cancelButton = "CancelKeyboard";
                break;
        }
    }
}
