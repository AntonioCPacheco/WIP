using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapManager : MonoBehaviour {

    public bool updateTilemap = false;

    public GameObject tilemapBackup;
    Transform child;

    ImageToTilemap tilemapScript;
    // Use this for initialization
    void Start () {
        if(tilemapBackup == null)
        {
            tilemapBackup = (GameObject)Resources.Load("TilemapBackup", typeof(GameObject));
        }
        if ((child = this.transform.Find("TilemapBackup")) == null)
        {
            child = Instantiate(tilemapBackup).transform;
            child.parent = this.transform;
        }
        tilemapScript = this.GetComponentInChildren<ImageToTilemap>();
    }
	
	// Update is called once per frame
	void Update () {
        if (updateTilemap)
        {
            updateTilemap = false;
            tilemapScript.UpdateTilemap();
            //UnityEditor.PrefabUtility.ReplacePrefab(child.gameObject, tilemapBackup);
            if (tilemapBackup == null)
            {
                tilemapBackup = (GameObject)Resources.Load("TilemapBackup", typeof(GameObject));
            }
        }
	}
}
