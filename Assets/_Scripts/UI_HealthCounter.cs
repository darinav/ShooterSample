using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UI_HealthCounter : MonoBehaviour {

    private Text HealthCounterText;

    void Awake()
    {
        HealthCounterText = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (TP_Health.Instance.CurrentPlayerHealth >= 100)
        {
            HealthCounterText.text = TP_Health.Instance.CurrentPlayerHealth.ToString("000") + "/" + TP_Health.Instance.StartingPlayerHealth.ToString("000");
        }
        else
        {
            HealthCounterText.text = TP_Health.Instance.CurrentPlayerHealth.ToString("00") + "/" + TP_Health.Instance.StartingPlayerHealth.ToString("000");
        }

    }
}
