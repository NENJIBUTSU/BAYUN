using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MG_Valve : MicroGame {

    [Header("Valve Setup")]
    [SerializeField] int progressGainedPerInput;

    [Header("Valve State")]
    [SerializeField, CustomAttributes.ReadOnly] ValveMode valveMode;
    [SerializeField, CustomAttributes.ReadOnly] NextValveInput nextValveInput;
    [SerializeField, CustomAttributes.ReadOnly] int valveProgress;

    [Header("Valve Heat")]
    [SerializeField] int heatDissipationRate;
    [SerializeField] int heatCapPercent;
    [SerializeField, CustomAttributes.ReadOnly] float currentHeat;
    [SerializeField] int heatGainedPerInput;
    [SerializeField, CustomAttributes.ReadOnly] int turnsUntilUnjammed;




    /*[Header("Visuals Object")]
    [SerializeField] MGV_Valve visualsComponent;
    */

    private void Awake() {
        Initialize();
    }

    public override void Initialize() {
        gameState = MicroGameState.Paused;
        currentHeat = 0;
        turnsUntilUnjammed = 0;
        valveProgress = 0;



        nextValveInput = (NextValveInput)Random.Range(0, 4);

        if (GameManager.Instance != null) {
            timeLeft = timeLimit - GameManager.Instance.microGamesCompleted;
            heatCapPercent = 75 - GameManager.Instance.microGamesCompleted;
        }
        else {
            timeLeft = timeLimit;
            heatCapPercent = 75;
        }
    }

    public override void UpdateMicroGame() {
        if (gameState == MicroGameState.Paused) {

        }
        else if (gameState == MicroGameState.Running) {
            if (valveProgress >= 100) {
                OnWin();
            }

            timeLeft -= Time.deltaTime;

            ValveModeCheck();
            GetNextInput();
            ReduceHeat();
        }
    }

    void ChangeValveMode(ValveMode mode) {
        if (mode == ValveMode.Normal) {
            valveMode = ValveMode.Normal;
        }
        else if (mode == ValveMode.Jammed) {
            if (GameManager.Instance != null) {
                valveMode = ValveMode.Jammed;
                SetJammedTurns();
                currentHeat = 0;
            }
            else {
                valveMode = ValveMode.Jammed;
                SetJammedTurns();
                currentHeat = 0;
            }

        }
    }

    public override void OnMistake() {
        throw new System.NotImplementedException();
    }

    public override void OnWin() {
        throw new System.NotImplementedException();
    }

    private void Update() {
        UpdateMicroGame();
    }

    void ValveModeCheck() {
        if (currentHeat > heatCapPercent && valveMode == ValveMode.Normal) {
            ChangeValveMode(ValveMode.Jammed);
        }
        else if (turnsUntilUnjammed <= 0 && valveMode == ValveMode.Jammed) {
            ChangeValveMode(ValveMode.Normal);
        }
    }

    void GetNextInput() {

        if (valveMode == ValveMode.Normal) {
            switch (nextValveInput) {
                case NextValveInput.Left:
                    if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) {
                        IncreaseValveProgress();
                        IncreaseHeat();
                        nextValveInput++;
                    }
                    break;
                case NextValveInput.Up:
                    if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) {
                        IncreaseValveProgress();
                        IncreaseHeat();
                        nextValveInput++;
                    }
                    break;

                case NextValveInput.Right:
                    if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) {
                        IncreaseValveProgress();
                        IncreaseHeat();
                        nextValveInput++;
                    }
                    break;
                case NextValveInput.Down:
                    if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) {
                        IncreaseValveProgress();
                        IncreaseHeat();
                        nextValveInput = NextValveInput.Left;
                    }
                    break;
            }

        }
        else if (valveMode == ValveMode.Jammed) {
            switch (nextValveInput) {
                case NextValveInput.Left:
                    if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) {
                        turnsUntilUnjammed = Mathf.Clamp(turnsUntilUnjammed - 1, 0, 30);
                        nextValveInput = NextValveInput.Right;
                    }

                    break;
                case NextValveInput.Up:
                    if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) {
                        turnsUntilUnjammed = Mathf.Clamp(turnsUntilUnjammed - 1, 0, 30);
                        nextValveInput = NextValveInput.Down;
                    }

                    break;
                case NextValveInput.Right:
                    if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) {
                        turnsUntilUnjammed = Mathf.Clamp(turnsUntilUnjammed - 1, 0, 30);
                        nextValveInput = NextValveInput.Left;
                    }
                    break;
                case NextValveInput.Down:
                    if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) {
                        turnsUntilUnjammed = Mathf.Clamp(turnsUntilUnjammed - 1, 0, 30);
                        nextValveInput = NextValveInput.Up;
                    }
                    break;
            }
        }
    }
    void IncreaseValveProgress() {
        valveProgress = Mathf.Clamp(valveProgress + progressGainedPerInput, 0, 100);
    }
    void IncreaseHeat() {
        currentHeat = Mathf.Clamp(currentHeat + heatGainedPerInput,0,100);
    }
    void ReduceHeat() {
        currentHeat = Mathf.Clamp(currentHeat - (Time.deltaTime * heatDissipationRate), 0, 100);
    }

    void SetJammedTurns() {
        int t = 0;
        if (GameManager.Instance != null) {
            t = Random.Range(3, 8 + GameManager.Instance.microGamesCompleted);

            if (t % 2 == 1) {
                turnsUntilUnjammed = t;
            }
            else {
                turnsUntilUnjammed = t + 1;
            }
        }
        else {
            t = turnsUntilUnjammed = Random.Range(3, 10);

            if (t % 2 == 1) {
                turnsUntilUnjammed = t;
            }
            else {
                turnsUntilUnjammed = t + 1;
            }
        }
    }

    enum ValveMode {
        Normal,
        Jammed
    }
}
enum NextValveInput {
    Left,
    Up,
    Right,
    Down
}
