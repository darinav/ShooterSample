using UnityEngine;
using System.Collections;

public class EnemyAttacks : MonoBehaviour 
{
    public float timeBetweenAttacks = 0.8f;
    public int attackDamage = 10;
    public GameObject BloodSplat;
	public bool playerInRange = false;

    Animator anim;
    GameObject player;  
    TP_Health playerHealth;
    EnemyHealth enemyHealth;    
    float timer;

    void Awake ()
    {
        player = GameObject.Find("TP_Character");
        playerHealth = player.GetComponent <TP_Health> ();
        enemyHealth = GetComponent<EnemyHealth>();
        anim = GetComponent <Animator>();
    }

    void OnCollisionEnter (Collision other)
    {
        if (other.gameObject.tag == "Player")
        {
            playerInRange = true;
        }
    }

    void OnCollisionExit (Collision other)
    {
        if (other.gameObject.tag == "Player")
        {
            playerInRange = false;
        }
    }

    void Update ()
    {
        timer += Time.deltaTime;

        if (timer >= timeBetweenAttacks && playerInRange && enemyHealth.CurrentHealth > 0)
        {
            Attack ();
            GameObject bloodSplat = (GameObject)Instantiate(BloodSplat, player.transform.position + Vector3.up * 1.5f, player.transform.rotation);
            Destroy(bloodSplat, 1);
        }

        if(playerHealth.CurrentPlayerHealth <= 0)
        {
            anim.SetTrigger ("PlayerDead");
        }
    }

    void Attack ()
    {
        timer = 0f;
        if(playerHealth.CurrentPlayerHealth > 0)
        {
            playerHealth.TakeDamage(attackDamage);
        }
    }
}
