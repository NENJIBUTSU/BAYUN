using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MG_TypingLetters : MicroGame
{
    [Header("Text")]
    [SerializeField] int baseStringLength;
    int currentStringLength;
    [CustomAttributes.ReadOnly, SerializeField] string textBuffer;
    [CustomAttributes.ReadOnly, SerializeField] string textToMatch;

    [Header("Visuals Object")]
    [SerializeField] MGV_TypingLetters visualsComponent;

    [CustomAttributes.ReadOnly, SerializeField]bool hasTypedLetter;

    [Header("Audio")]
    [SerializeField] AudioClip[] keyboardSounds;

    private void Awake() {
        Initialize();
    }


    public override void Initialize() {
        gameState = MicroGameState.Paused;

        if (GameManager.Instance != null) {
            timeLeft = timeLimit - GameManager.Instance.microGamesCompleted;
            currentStringLength = baseStringLength + GameManager.Instance.microGamesCompleted;
        }
        else {
            timeLeft = timeLimit;
            currentStringLength = 8;
        }



        ResetWord();

    }

    public override void UpdateMicroGame() {

        if (gameState == MicroGameState.Running) {

            if (timeLeft <= 0) {
                SetGameState(MicroGameState.Failed);
            }

            if (textToMatch.Length == textBuffer.Length && CheckForMatch()) { 
                OnWin();
            }

            if (Input.inputString.Length > 0) {
                char c = Input.inputString[0];

                if (char.IsLetter(c)) {
                    AudioManager.Instance.audioSource.PlayOneShot(keyboardSounds[Random.Range(0, keyboardSounds.Length - 1)]);
                    hasTypedLetter = true;
                    c = char.ToLower(Input.inputString[0]);
                    textBuffer += c;
                }
            }

            if (hasTypedLetter) {
                if (CheckForMatch()) {
                    visualsComponent.OnNextLetter();
                    hasTypedLetter = false;
                }
                else if (CheckForMatch() == false) {
                    hasTypedLetter = false;
                    OnMistake();
                }
            }
            

            timeLeft -= Time.deltaTime;

        }

        visualsComponent.UpdateVisual();

    }

    void ResetWord() {
        textBuffer  = string.Empty;
        textToMatch = string.Empty;

        for (int i = 0; i < baseStringLength; i++) {
            textToMatch += (char)('a' + UnityEngine.Random.Range(0, 26));
        }

        visualsComponent.ResetText(textToMatch);
    }

    bool CheckForMatch() {
        if (textToMatch.Substring(0, textBuffer.Length) == textBuffer) { 
            Debug.Log("Match returned true!"); 
            return true; 
        }
        else { 
            return false; 
        }
    }

    public override void OnMistake() {
        //tell game manager the player fucked up!
        if (GameManager.Instance != null) {
            SetGameState(MicroGameState.Failed);
        } else {
            ResetWord();
        }
    }

    public override void OnWin()  {
        //tell the game manager we won!
        SetGameState(MicroGameState.Won);
    }


}
