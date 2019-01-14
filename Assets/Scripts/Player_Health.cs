using UnityEngine;
using System.Collections;

public class Player_Health : MonoBehaviour {
	
	public GameObject healthBar;
    GameObject spotlight;
	public int maxHealth = 3; //CHANGE TO PRIVATE FOR RELEASE
	int health;
	bool healthChanged = false;
	bool iframes = false;

    public Color damageColor;
    public Color defaultColor;
    public float smoothColor = 1f;

    public float timeBetweenTakingDamage = 1f;
	float lastDamageTaken = - 5f;
	
	HUD_UpdateHealthBar updateHealthBar;
	
	// Use this for initialization
	void Start () {
		updateHealthBar = healthBar.GetComponent<HUD_UpdateHealthBar> ();
		UpdateHealthBar ();
        health = maxHealth;

        spotlight = GameObject.Find("Spotlight");

    }

    // Update is called once per frame
    void Update()
    {
        if (healthChanged)
        {
            UpdateHealthBar();
            if (isDead())
                Die();
        }
        if (isInvincible() && Time.time > lastDamageTaken + timeBetweenTakingDamage)
        {
            iframes = false;
            gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
            Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Eyes"), false);
            Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Crawlers"), false);
        }

        if (spotlight != null)
        {
            spotlight.GetComponent<Light>().color = Color.Lerp(spotlight.GetComponent<Light>().color, defaultColor, Time.deltaTime * smoothColor);
        }
    }
	
	void Die(){
		gameObject.GetComponent<Animator> ().SetBool("Dead", true);
		gameObject.GetComponent<Animator> ().SetTrigger("Die");
		healthChanged = false;
	}
	
	public bool TakeDamage(int damage){
		bool res = false;
		if(!isDead()){
			if (Time.time > lastDamageTaken + timeBetweenTakingDamage) {
				lastDamageTaken = Time.time;
				if (damage >= health) {
					health = 0;
				} else{
					health -= damage;
					//invicibility frames
					iframes = true;
					gameObject.GetComponent<SpriteRenderer> ().color = new Color(1f, 1f, 1f, 0.3f);
					gameObject.GetComponent<Player_Movement> ().TakeDamage ();
                    
					Physics2D.IgnoreLayerCollision (LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Eyes"));
					Physics2D.IgnoreLayerCollision (LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Crawlers"));
                    
                    if(spotlight != null) spotlight.GetComponent<Light>().color = damageColor;
                }

				res = true;
				healthChanged = true;
			}
		}

		return res;
	}
	
	void UpdateHealthBar(){
		if (health < 0)
			health = 0;
		updateHealthBar.Draw(health);
		healthChanged = false;
	}
	
	public void GainHealth(int ammountGained){ // ammountGained (-1 = fullHealth, otherwise simple)
		if (health < maxHealth) {
			if (ammountGained == -1) {
				health = maxHealth;
			} else {
				health += ammountGained;
			}
			healthChanged = true;
		}
		if(health > maxHealth)
			health = maxHealth;
	}

	public int getHealth(){
		return health;
	}

	public int getMaxHealth(){
		return maxHealth;
	}

	public bool isDead(){
		return health <= 0;
	}

	public bool isInvincible(){
		return iframes;
	}
}
