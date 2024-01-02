using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Transition : MonoBehaviour
{
    [Header("GameObjects")]
    [SerializeField] GameObject microGameContainer;

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

    [CustomAttributes.ReadOnly] public bool isTransitionFinished;
    private bool transitionEnding;
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

        isTransitionFinished = false;
    }

    public void UpdateTransition() {
        if (transitionTimeLeft <= 0 && transitionEnding == false) {
            StartCoroutine(EndTransition());
            transitionEnding = true;
        }
        else {
            transitionTimeLeft -= Time.deltaTime;
            textTransitionTimeLeft -= Time.deltaTime;
            UpdateTransitionText();

        }
    }

    void UpdateTransitionText() {
        if (textIndex >= 0) {
            if (textTransitionTimeLeft <= textTimeIndices[textIndex]) {
                textComponent.text = finalText.Substring(0, finalText.Length - textIndex);
                textIndex--;
            }
        }
    }

    IEnumerator EndTransition() {
        yield return new WaitForSeconds(endTransitionTime); //possibly replace with some form of tv static thing
        isTransitionFinished = true; 
        gameObject.SetActive(false);
        microGameContainer.SetActive(true);
        yield return null;
    }

    //method to display transition name (and optional hints?)
    //method to create cool text effect :)
    //method to end transition (and move to next game phase)
    /*IEnumerator EndTransition() {
        isTransitionFinished = true;
        gameObject.SetActive(false);
    }*/
}
