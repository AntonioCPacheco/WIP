using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blob_SpawnerBehaviour : MonoBehaviour {

    Spawner_Blob script;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void setScript(Spawner_Blob s)
    {
        this.script = s;
    }

    public void triggerSpawner()
    {
        this.script.spawnNewBlob();
    }
}
