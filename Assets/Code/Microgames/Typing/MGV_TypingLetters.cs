using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MGV_TypingLetters : MonoBehaviour {

    [Header("Text")]
    [SerializeField] TMP_Text textComponent;
    [SerializeField] int maxCharactersVisible;
    TMP_TextInfo textInfo;
    int[] characterHeightOffsets;

    [CustomAttributes.ReadOnly] public int currentCharacterIndex;
    [Header("Tweening")]
    [SerializeField] float timeToTween;
    [CustomAttributes.ReadOnly, SerializeField] float tweenTimer;

    private void Awake() {
        textInfo = textComponent.textInfo;
    }

    public void UpdateVisual() {
        tweenTimer = Mathf.Clamp(tweenTimer - (Time.deltaTime * 4), 0, timeToTween);

        UpdateTextMesh();
    }

    void InitializeText() {
        currentCharacterIndex = 0;
        characterHeightOffsets = new int[textComponent.text.Length];
        for (int i = 0; i < textComponent.text.Length; i++) {
            characterHeightOffsets[i] = Random.Range((int)-1, 2) * 3;
        }
    }

    void UpdateTextMesh() {
        textComponent.ForceMeshUpdate();

        for (int i = 0; i < textInfo.characterCount; i++) {
            TMP_CharacterInfo charInfo = textInfo.characterInfo[i];

            if (!charInfo.isVisible) { continue; }

            Vector3[] verts = textInfo.meshInfo[charInfo.materialReferenceIndex].vertices;

            for (int j = 0; j < 4; j++) {
                Vector3 origVerts = verts[charInfo.vertexIndex + j];
                verts[charInfo.vertexIndex + j] = origVerts + new Vector3(i - currentCharacterIndex, characterHeightOffsets[i], 0); //BOOKMARK: FIGURE OUT FONT SIZE CHANGE
            }

            Color32[] colors32 = textInfo.meshInfo[charInfo.materialReferenceIndex].colors32;

            if (i == currentCharacterIndex - 1) {
                for (int j = 0; j < 4; j++) {
                    Color32 origColor32 = colors32[charInfo.vertexIndex + j];

                    Color origColor = origColor32;

                    colors32[charInfo.vertexIndex + j] = origColor * new Color(1, 1, 1, 0 + Mathf.Clamp(tweenTimer, 0, 1));
                }
            }
            else if (i < currentCharacterIndex - 1) {

                for (int j = 0; j < 4; j++) {

                    Color32 origColor32 = colors32[charInfo.vertexIndex + j];

                    Color origColor = origColor32;

                    colors32[charInfo.vertexIndex + j] = origColor * new Color(1, 1, 1, 0);
                }
            }
            else if (i == currentCharacterIndex + maxCharactersVisible - 1) {

                for (int j = 0; j < 4; j++) {

                    Color32 origColor32 = colors32[charInfo.vertexIndex + j];

                    Color origColor = origColor32;

                    colors32[charInfo.vertexIndex + j] = origColor * new Color(1, 1, 1, 1 - Mathf.Clamp(tweenTimer, 0, 1));
                }
            }
            else if (i >= currentCharacterIndex + maxCharactersVisible) {

                for (int j = 0; j < 4; j++) {

                    Color32 origColor32 = colors32[charInfo.vertexIndex + j];

                    Color origColor = origColor32;

                    colors32[charInfo.vertexIndex + j] = origColor * new Color(1, 1, 1, 0);
                }
            }
        }

        //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!UPDATE!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!//

        for (int i = 0; i < textInfo.meshInfo.Length; i++) {
            TMP_MeshInfo meshInfo = textInfo.meshInfo[i];
            meshInfo.mesh.vertices = meshInfo.vertices;
            meshInfo.mesh.colors32 = meshInfo.colors32;

            textComponent.UpdateGeometry(meshInfo.mesh, i);
        }
    }

    public void ResetText(string newText) {
        textComponent.SetText(newText);
        InitializeText();
    }

    public void OnNextLetter() {
        currentCharacterIndex++;
        tweenTimer = timeToTween;
    }
}
