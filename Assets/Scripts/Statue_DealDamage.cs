using UnityEngine;
using System.Collections;

public class Statue_DealDamage : MonoBehaviour
{

    GameObject player;
    Collider2D playerPolygonCollider;
    Collider2D playerBoxCollider;

    bool playerTookDamage = false;

    bool isInLight;

    float lastDealt = 0f;
    public float timeBetweenDealing = 1f;

    public int damage = 1;

    // Use this for initialization
    void Start()
    {
        isInLight = GetComponent<Statue_Movement>().IsInLight();
        player = GameObject.Find("Player Prefab");
        playerPolygonCollider = player.GetComponent<PolygonCollider2D>();
        playerBoxCollider = player.GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        isInLight = GetComponent<Statue_Movement>().IsInLight();
        if (!isInLight && (playerPolygonCollider.IsTouching(GetComponent<BoxCollider2D>()) || playerBoxCollider.IsTouching(GetComponent<BoxCollider2D>())) && (Time.time > lastDealt + timeBetweenDealing))
        {
            DoDamage();
        }
    }

    void DoDamage()
    {
        playerTookDamage = GameObject.Find("Player Prefab").GetComponent<Player_Health>().TakeDamage(damage);
        if (playerTookDamage)
        {
            lastDealt = Time.time;
            Debug.Log(this.GetInstanceID() + " dealt damage");
        }
    }
}
