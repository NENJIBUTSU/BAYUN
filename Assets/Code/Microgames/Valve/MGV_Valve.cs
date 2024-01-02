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
            hotkeySpriteObjects[0].SetActive(true);
            hotkeySpriteObjects[1].SetActive(false);
            hotkeySpriteObjects[2].SetActive(false);
            hotkeySpriteObjects[3].SetActive(false);
        }
        if (valveInput == NextValveInput.Up)    {
            hotkeySpriteObjects[0].SetActive(false);
            hotkeySpriteObjects[1].SetActive(true);
            hotkeySpriteObjects[2].SetActive(false);
            hotkeySpriteObjects[3].SetActive(false);
        }
        if (valveInput == NextValveInput.Right) {
            hotkeySpriteObjects[0].SetActive(false);
            hotkeySpriteObjects[1].SetActive(false);
            hotkeySpriteObjects[2].SetActive(true);
            hotkeySpriteObjects[3].SetActive(false);
        }
        if (valveInput == NextValveInput.Down)  {
            hotkeySpriteObjects[0].SetActive(false);
            hotkeySpriteObjects[1].SetActive(false);
            hotkeySpriteObjects[2].SetActive(false);
            hotkeySpriteObjects[3].SetActive(true);
        }
    }

    void RotateValve(int progressPercent, bool isJammed) {
        if (progressPercent <= 0) {
            return;
        }

        Vector3 currentEuler = transform.rotation.eulerAngles;
        float rotation = currentEuler.z;

        float rotateDegrees = 360 / (100 / progressPercent);

        if (isJammed) {

            rotateDegrees *= 2;

            if (isJammedRight) {

                rotation -= rotateDegrees;
                isJammedRight = false;
            }
            else {
                rotation += rotateDegrees;
                isJammedRight = true;
            }

            valveSpriteObject.transform.RotateAround(valveSpriteObject.GetComponent<Renderer>().bounds.center, Vector3.back, rotation);
        }
        else {
            rotation += rotateDegrees;
            valveSpriteObject.transform.RotateAround(valveSpriteObject.GetComponent<Renderer>().bounds.center, Vector3.back, rotation);
        }
    }

    void UpdateDoor(int progressGained, bool isJammed) {
        Vector3 t = doorSpriteObject.transform.position;
        if (isJammed == false) {
            doorSpriteObject.transform.Translate(new Vector3(-((float)7.46 * progressGained * (float)0.01), 0, 0));
        }

    }

    public void OnValveTurn(NextValveInput nextInput, int progressGained, bool isJammed) {
        RotateValve(progressGained, isJammed);
        ShowHotkeySprite(nextInput);
        UpdateDoor(progressGained, isJammed);
    }
}
