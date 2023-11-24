using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MicroGame : MonoBehaviour {
    [Header("Game State")]
    [SerializeField] protected MicroGameState gameState;


    [Header("Timer")]
    [SerializeField] protected int timeLimit;
    [SerializeField, CustomAttributes.ReadOnly] protected float timeLeft;


    public abstract void UpdateMicroGame();

    public void SetGameState(MicroGameState state) {
        gameState = state;
    }

    public MicroGameState GetGameState() {
        return gameState;
    }

    public virtual void Initialize() {
        gameState = MicroGameState.Paused;

        if (GameManager.Instance != null) {
            timeLeft = timeLimit - GameManager.Instance.microGamesCompleted;
        }
        else {
            timeLeft = timeLimit;
        }
    }


    public abstract void OnMistake();

    public abstract void OnWin();
}

public enum MicroGameState
{
    Paused,
    Running,
    Failed,
    Won
}