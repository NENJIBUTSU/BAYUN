using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MGV_Valve : MonoBehaviour
{
    [Header("Sprites")]
    [SerializeField] GameObject doorSpriteObject;
    [SerializeField] GameObject valveSpriteObject;
    [SerializeField] GameObject[] hotkeySpriteObjects;

    [Header("Rotation")]
    [SerializeField, CustomAttributes.ReadOnly] float currentRotation;
    [SerializeField, CustomAttributes.ReadOnly] bool isJammedRight;

    void ShowHotkeySprite(NextValveInput valveInput) {
        if (valveInput == NextValveInput.Left)  {

        }
        if (valveInput == NextValveInput.Up)    {

        }
        if (valveInput == NextValveInput.Right) {

        }
        if (valveInput == NextValveInput.Down)  {

        }
    }

    void RotateValve(int progressPercent, bool isJammed) {
        if (isJammed) {

        }
        else {
            Vector3 currentEuler = transform.rotation.eulerAngles;
            float rotation = currentEuler.z;

            float rotateDegrees = 360 / (100 / progressPercent);
            rotation += rotateDegrees;

            valveSpriteObject.transform.RotateAround(valveSpriteObject.GetComponent<Renderer>().bounds.center, Vector3.back, rotation);
        }
    }
}
