using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner_Blob : MonoBehaviour {

    public GameObject blob_prefab;
    public Transform active_blob;
    bool active = true;
    float outer_limit = 500;
    float inner_limit = 200;

    // Use this for initialization
    void Start ()
    {
        active_blob.GetComponent<Blob_SpawnerBehaviour>().setScript(this);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void spawnNewBlob()
    {
        if (active)
        {
            GameObject active_blob = Instantiate(blob_prefab, this.transform);
            active_blob.GetComponent<Blob_SpawnerBehaviour>().setScript(this);
        }
    }
}
