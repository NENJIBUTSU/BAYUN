using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MicroGameIntro : MonoBehaviour
{
    [Header("Text")]
    [SerializeField] string finalText;
    [SerializeField, CustomAttributes.ReadOnly] int textIndex;
    float[] textTimeIndices;
    [SerializeField] TMP_Text textComponent;

    [Header("Time")]
    [SerializeField] float transitionTime;
    [SerializeField, CustomAttributes.ReadOnly] float transitionTimeLeft;

    [SerializeField] float textTransitionTime;
    [SerializeField, CustomAttributes.ReadOnly] float textTransitionTimeLeft;

    [CustomAttributes.ReadOnly] public bool isIntroFinished;
    private bool introEnding;
    [SerializeField] float endTransitionTime;

    private void Awake() {
        textTransitionTimeLeft = textTransitionTime;
        transitionTimeLeft = transitionTime;
        textIndex = finalText.Length - 1;

        float t = textTransitionTime / finalText.Length;
        textTimeIndices = new float[finalText.Length];

        for (int i = 0;  i < finalText.Length; i++) {
            textTimeIndices[i] = t * i;
        }

        isIntroFinished = false;
        introEnding = false;
    }

    public void Initialize(string text, float timeToTransition, float textTransitionTime) {
        this.textComponent.text = string.Empty;
        this.finalText = text;
        textTransitionTimeLeft = textTransitionTime;
        this.transitionTime = timeToTransition;
        transitionTimeLeft = transitionTime;
        textIndex = finalText.Length - 1;

        float t = textTransitionTime / finalText.Length;
        textTimeIndices = new float[finalText.Length];

        for (int i = 0; i < finalText.Length; i++) {
            textTimeIndices[i] = t * i;
        }

        isIntroFinished = false;
        introEnding = false;
    }

    public void UpdateIntro() {
        if (transitionTimeLeft <= 0 && introEnding == false) {
            StartCoroutine(EndIntro());
            introEnding = true;
        }
        else {
            transitionTimeLeft -= Time.deltaTime;
            textTransitionTimeLeft -= Time.deltaTime;
            UpdateIntroText();

        }
    }

    void UpdateIntroText() {
        if (textIndex >= 0) {
            if (textTransitionTimeLeft <= textTimeIndices[textIndex]) {
                textComponent.text = finalText.Substring(0, finalText.Length - textIndex);
                textIndex--;
            }
        }
    }

    IEnumerator EndIntro() {
        yield return new WaitForSeconds(endTransitionTime); //possibly replace with some form of tv static thing
        isIntroFinished = true;
        yield return null;
    }
}
