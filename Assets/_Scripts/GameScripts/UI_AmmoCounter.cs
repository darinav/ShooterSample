using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class UI_AmmoCounter : MonoBehaviour {

    private Text AmmoCounterText;

	void Awake ()
    {
        AmmoCounterText = GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        AmmoCounterText.text = Aiming.Instance.CurrentAmmoCount.ToString("00") + "/" + Aiming.Instance.TotalAmmoCount.ToString("00");
	}
}
