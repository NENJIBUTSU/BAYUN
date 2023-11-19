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

    private void Awake() {
        Initialize();
    }


    public override void Initialize() {
        gameState = MicroGameState.Paused;
        timeLeft = timeLimit; //TODO: add - microGamesCompleted;
        //currentStringLength = baseStringLength + microGamesCompleted;

        ResetWord();

    }

    private void Update() {
        UpdateMicroGame();
    }

    public override void UpdateMicroGame() {
        if (gameState == MicroGameState.Paused) {

        }
        else if (gameState == MicroGameState.Running) {

            if (textToMatch.Length == textBuffer.Length && CheckForMatch()) { 
                OnWin();
            }

            if (Input.inputString.Length > 0) {
                char c = Input.inputString[0];

                if (char.IsLetter(c)) {
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
            if (GameManager.Instance.noMistakes) {
                SetGameState(MicroGameState.Failed);
            }
            else {
                ResetWord();
            }
        } else {
            ResetWord();
        }
    }

    public override void OnWin()  {
        //tell the game manager we won!
        SetGameState(MicroGameState.Won);
    }


}
