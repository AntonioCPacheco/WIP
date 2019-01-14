using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class SaveManager : MonoBehaviour
{
    public static SaveManager sm;

    private static GameObject[] boxes;

    private static DialogueTrigger[] dts;
    private static JumpTrigger[] jts;
    void Start()
    {
    }

    void FixedUpdate()
    {
        string path = Application.dataPath + "/Saves";

        if (Application.isEditor)
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                if (Input.GetKeyDown(KeyCode.Alpha1))
                {
                    print("Loading Room1.");
                    SaveManager.saveGame(path + "/Room1.dat");
                }
                else if (Input.GetKeyDown(KeyCode.Alpha2))
                {
                    print("Loading Room2.");
                    SaveManager.loadGame(path + "/Room2.dat");
                }
                else if (Input.GetKeyDown(KeyCode.Alpha3))
                {
                    print("Loading Room3.");
                    SaveManager.loadGame(path + "/Room3.dat");
                }
                else if (Input.GetKeyDown(KeyCode.Alpha4))
                {
                    print("Loading Room4.");
                    SaveManager.loadGame(path + "/Room4.dat");
                }
                else if (Input.GetKeyDown(KeyCode.Alpha5))
                {
                    print("Loading Room5.");
                    SaveManager.loadGame(path + "/Room5.dat");
                }
            }
            else
            {
                if (Input.GetKeyDown(KeyCode.Alpha1))
                {
                    print("Loading Level1.");
                    SaveManager.loadGame(path + "/Level1.dat");
                }
                else if (Input.GetKeyDown(KeyCode.Alpha2))
                {
                    print("Loading Level2.");
                    SaveManager.loadGame(path + "/Level2.dat");
                }
                else if (Input.GetKeyDown(KeyCode.Alpha3))
                {
                    print("Loading Level3.");
                    SaveManager.loadGame(path + "/Level3.dat");
                }
                else if (Input.GetKeyDown(KeyCode.Alpha4))
                {
                    print("Loading Level4.");
                    SaveManager.loadGame(path + "/Level4.dat");
                }
                else if (Input.GetKeyDown(KeyCode.Alpha5))
                {
                    print("Loading Level5.");
                    SaveManager.loadGame(path + "/Level5.dat");
                }
                else if (Input.GetKeyDown(KeyCode.Alpha6))
                {
                    print("Loading Level6.");
                    SaveManager.loadGame(path + "/Level6.dat");
                }
            }
        }
    }

    // Use this for initialization
    void Awake()
    {
        if (sm == null)
        {
            DontDestroyOnLoad(gameObject);
            sm = this;
        }
        else if (sm != this)
        {
            Destroy(gameObject);
        }
        dts = FindObjectsOfType<DialogueTrigger>();
        jts = FindObjectsOfType<JumpTrigger>();

        boxes = GameObject.FindGameObjectsWithTag("Box");
    }

    public static void saveGame()
    {
        SaveManager.saveGame(Application.persistentDataPath + "/saveData.dat");
    }

    static void saveGame(string path)
    {
        Debug.Log("Game is being saved!");
        Dictionary<Vector3, bool> boxes = new Dictionary<Vector3, bool>();
        Dictionary<string, bool> triggers = new Dictionary<string, bool>();
        List<string> doors = new List<string>();

        foreach (GameObject b in SaveManager.boxes)
        {
            //if (boxes.ContainsKey(b.transform.position)) boxes.Remove(b.transform.position);
            if(b.transform.parent == null || !b.transform.parent.CompareTag("Player"))
                boxes.Add(b.transform.position, b.activeSelf);
        }

        foreach (GameObject t in GameObject.FindGameObjectsWithTag("Trigger"))
        {
            triggers.Add(t.name, t.GetComponent<Lock_BoxTrigger>().triggered);
        }

        foreach (GameObject d in GameObject.FindGameObjectsWithTag("Door"))
        {
            doors.Add(d.name);
        }

        Save save = new Save();

        save.playerHasBox = GameObject.FindGameObjectWithTag("Player").GetComponent<Player_Movement>().doesPlayerHaveBox();

        save.setPlayerPosition(GameObject.FindGameObjectWithTag("Player").transform.position);
        save.setNpcPosition(GameObject.FindGameObjectWithTag("NPC").transform.position);
        save.npcGoal = FindObjectOfType<NPC_Movement>().nextStep;
        save.npcVisible = GameObject.FindGameObjectWithTag("NPC").GetComponent<SpriteRenderer>().enabled;

        save.setBoxes(boxes);
        save.triggers = triggers;
        save.doors = doors;

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(path);

        bf.Serialize(file, save);
        file.Close();
    }

    public static void loadGame()
    {
        SaveManager.loadGame(Application.persistentDataPath + "/saveData.dat");
    }

    static void loadGame(string path)
    {
        if(File.Exists(path))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.OpenRead(path);
            Save save = (Save) bf.Deserialize(file);
            file.Close();

            GameObject.FindWithTag("Player").GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            GameObject.FindWithTag("Player").transform.position = save.getPlayerPosition();

            FindObjectOfType<NPC_Movement>().load(save.getNpcPosition(), save.npcGoal);
        
            GameObject.FindGameObjectWithTag("NPC").GetComponent<SpriteRenderer>().enabled = save.npcVisible;
            Camera.main.GetComponent<Camera_Movement>().followNPC = save.npcVisible;

            foreach(DialogueTrigger t in dts)
            {
                t.gameObject.SetActive(true);
            }
            foreach (JumpTrigger j in jts)
            {
                j.gameObject.SetActive(true);
            }

            int i = 0;
            if (save.playerHasBox)
            {
                Transform box = (boxes[0]).transform;
                FindObjectOfType<Player_Movement>().setBox(box);
                i++;
            }
            else
            {
                FindObjectOfType<Player_Movement>().dropBoxAnim();
            }

            foreach(KeyValuePair<Vector3, bool> item in save.getBoxes())
            {
                boxes[i].transform.parent = null;
                boxes[i].transform.position = item.Key;
                boxes[i].SetActive(item.Value);
                i++;
            }

            foreach (KeyValuePair<string, bool> item in save.triggers)
            {
                GameObject trigger = GameObject.Find(item.Key);
                trigger.GetComponent<Lock_BoxTrigger>().load(item.Value);
            }

            foreach (string item in save.doors)
            {
                GameObject door = GameObject.Find(item);

                door.GetComponent<Player_EnterDoors>().load();
            }
        }

    }

    public static void loadFirstLevel()
    {
        string path = Application.dataPath + "/Saves";
        SaveManager.loadGame(path + "/Room1.dat");
    }
}