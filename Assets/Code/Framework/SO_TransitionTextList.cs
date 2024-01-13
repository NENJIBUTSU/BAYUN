using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Framework/Transition Text List")]
public class SO_TransitionTextList : ScriptableObject
{
    [SerializeField] public List<string> successList;
    [SerializeField] public List<string> failList;
    [SerializeField] public List<string> breakList;
}
