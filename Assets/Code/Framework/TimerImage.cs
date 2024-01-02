using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerImage : MonoBehaviour
{
    public void Initialize() {
        timerImage.color = new Color(1,1,1,1);
        timerImage.fillAmount = 1;
    }
    [SerializeField] Image timerImage;
    public void UpdateTimerImage(float percentage) {
        timerImage.fillAmount = percentage / 100;

        if (percentage < 25) {
            timerImage.color = new Color(
                Mathf.Lerp(0.54f, 1, percentage / 100),
                Mathf.Lerp(0f, 1, percentage / 100),
                Mathf.Lerp(0f, 1, percentage / 100),
                1);
        }
    }
}
