using UnityEngine;
using System.Collections;

public class EnemySpawn : MonoBehaviour
{
    public GameObject EnemyPrefab;
    public float SpawnStartDelay;
    public float SpawnRate;

    private GameObject player;
    private TP_Health playerHealth;
	private GameObject[] zombieCounter;
	private int zombieCount = 30;
	private bool limitReached = false;

    void Awake()
    {
        player = GameObject.Find("TP_Character");
        playerHealth = player.GetComponent<TP_Health>();
    }

    void Start()
    {
        InvokeRepeating("Spawn", SpawnStartDelay, SpawnRate);
    }

    void Update()
    {
		zombieCounter = GameObject.FindGameObjectsWithTag ("Zombie");
		if (playerHealth.CurrentPlayerHealth <= 0)
        {
            CancelInvoke("Spawn");
        }
        if (zombieCounter.Length >= zombieCount)
        {
            CancelInvoke("Spawn");
            limitReached = true;
        }
        if (zombieCounter.Length < zombieCount &&
            limitReached == true &&
            playerHealth.CurrentPlayerHealth > 0)
        {
            InvokeRepeating("Spawn", SpawnStartDelay, SpawnRate);
        }
    }

    void Spawn()
    {
        Instantiate(EnemyPrefab, transform.position, transform.rotation);
    }

    
}





