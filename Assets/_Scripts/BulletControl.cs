using UnityEngine;
using System.Collections;

public class BulletControl : MonoBehaviour 
{
    public int Damage = 50;
    public GameObject enemy;
    public GameObject BloodSplat;

    private float timeBeforeDestroy = 5f;
    private float timer = 0f;
    private EnemyHealth enemyHealth;    

	void Awake() 
    {        
    }
	
	void Update() 
    {
        timer += Time.deltaTime;
        if(timer > timeBeforeDestroy)
        {
            DestroyBullet();
        }        
    }

    void OnCollisionEnter (Collision col)
    {
        if(col.gameObject.tag == "Zombie")
        {
            enemy = col.gameObject;
            enemyHealth = col.gameObject.GetComponent<EnemyHealth>();
            enemyHealth.TakeDamage(Damage);
            GameObject bloodSplat = (GameObject)Instantiate(BloodSplat, col.transform.position + Vector3.up * 1.5f, col.transform.rotation);
            Destroy(bloodSplat, 1f);
        }
        DestroyBullet();

    }
    
    public void DestroyBullet()
    {
        Destroy(this.gameObject);
    }
    
}
