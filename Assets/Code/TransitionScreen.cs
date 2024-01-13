using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionScreen : MonoBehaviour
{
    [SerializeField] public MicroGameIntro transitionComponent;
    [SerializeField] SO_TransitionTextList transitionTextList;
    public void TriggerFailScreen() {
        transitionComponent.Initialize(transitionTextList.failList[Random.Range(0, transitionTextList.failList.Count)],2.5f, 1f);   
    }

    public void TriggerSuccessScreen() {
        transitionComponent.Initialize(transitionTextList.successList[Random.Range(0, transitionTextList.successList.Count)], 2.5f, 1f);
    }

    public void TriggerWaitingScreen() {
        transitionComponent.Initialize(transitionTextList.breakList[Random.Range(0, transitionTextList.breakList.Count)], 2.5f, 1.5f);
    }
}
