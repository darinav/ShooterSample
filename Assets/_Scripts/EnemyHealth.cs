using UnityEngine;
using System.Collections;

public class EnemyHealth : MonoBehaviour {

    public float MaxHealth = 100f;
    public float CurrentHealth;

    private GameObject bullet;
    private BulletControl bulletControl;
    private Animator anim;
    private AnimatorStateInfo currentBaseState;
    private EnemyMovement enemyMovement;
    //private Rigidbody enemyRB;

	void Awake() 
    {
        anim = GetComponent<Animator>();
        enemyMovement = GetComponent<EnemyMovement>();
        CurrentHealth = MaxHealth;
        //enemyRB = GetComponent<Rigidbody>();
	}

    public void TakeDamage(int damage)
    {
        CurrentHealth -= damage;
    }

    void Update()
    {
        currentBaseState = anim.GetCurrentAnimatorStateInfo(0);

        if (CurrentHealth <= 0)
        {
            anim.SetTrigger("Dead");
            enemyMovement.enabled = false;
            //enemyRB.angularVelocity = Vector3.zero;
            Destroy(gameObject, 3f);
        }
    }
}
