using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Framework/MicroGame List")]
public class SO_MicroGameList : ScriptableObject
{
    [SerializeField]public List<string> list;
}
