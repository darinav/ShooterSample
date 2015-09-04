using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TP_Health : MonoBehaviour {

    public int StartingPlayerHealth = 100;
    public int CurrentPlayerHealth;
    public Slider healthSlider;
    public static TP_Health Instance;


    private Animator anim;
    private bool isDead;
    private TP_Controller controller;

	void Awake ()
    {
        CurrentPlayerHealth = StartingPlayerHealth;
        anim = GetComponent<Animator>();
        controller = GetComponent<TP_Controller>();
        Instance = this;
    }
	
	void Update ()
    {
	
	}

    public void TakeDamage(int amount)
    {
        CurrentPlayerHealth -= amount;
        healthSlider.value = CurrentPlayerHealth;
        if (CurrentPlayerHealth <= 0 && !isDead)
        {
            Death();
        }
    }

    void Death()
    {
        isDead = true;
        anim.SetTrigger("isDead");
        controller.enabled = false;
    }
}
