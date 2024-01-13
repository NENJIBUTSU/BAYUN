using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MG_Valve : MicroGame {

    [Header("Valve Setup")]
    [SerializeField] int progressGainedPerInput;

    [Header("Valve State")]
    [SerializeField, CustomAttributes.ReadOnly] ValveMode valveMode;
    [SerializeField, CustomAttributes.ReadOnly] NextValveInput nextValveInput;
    [SerializeField, CustomAttributes.ReadOnly] int valveProgress;

    [Header("Valve Heat")]
    [SerializeField] int heatDissipationRate;
    [SerializeField] float heatCapPercent;
    [SerializeField, CustomAttributes.ReadOnly] float currentHeat;
    [SerializeField] int heatRangeGainedPerInput;
    [SerializeField, CustomAttributes.ReadOnly] int turnsUntilUnjammed;

    [SerializeField] TimerImage heatBar;
    [SerializeField] Image heatBarBackground;
    [SerializeField] GameObject topHeatBarObject;
    [SerializeField] GameObject midHeatBarObject;
    [SerializeField] GameObject botHeatBarObject;
    [SerializeField] int heatBarsLeft;
    
    [Header("Visuals Object")]
    [SerializeField] MGV_Valve visualsComponent;

    [Header("Audio")]
    [SerializeField] AudioClip[] valveTurnSounds;
    [SerializeField] AudioClip valveUnjamSound;

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

            progressGainedPerInput = 2;

            heatCapPercent = 100;
            heatRangeGainedPerInput = 8 + Mathf.RoundToInt(GameManager.Instance.microGamesCompleted / 2);
        }
        else {
            timeLeft = timeLimit;
            heatCapPercent = 100;
        }

        visualsComponent.OnValveTurn(nextValveInput, 0, false);
    }

    public override void UpdateMicroGame() {
        if (gameState == MicroGameState.Running) {
            if (valveProgress >= 100) {
                OnWin();
            }

            timeLeft -= Time.deltaTime;

            ValveModeCheck();
            GetNextInput();
            ReduceHeat();

            if (timeLeft <= 0) {
                OnMistake();
            }
        }

        if (valveMode == ValveMode.Normal) {
            if (heatBarsLeft == 3) {
                heatBar.UpdateTimerImage(Mathf.Clamp(currentHeat, 0, 100), false);
            }
            else if (heatBarsLeft == 2) {
                heatBar.UpdateTimerImage(Mathf.Clamp(currentHeat, 0, 66.6f), false);
            }
            else if (heatBarsLeft == 1) {
                heatBar.UpdateTimerImage(Mathf.Clamp(currentHeat, 0, 33.3f), false);
            }

        }
        else {
            heatBar.UpdateTimerImage(heatCapPercent, false);
        }

    }

    void ChangeValveMode(ValveMode mode) {
        if (mode == ValveMode.Normal) {
            valveMode = ValveMode.Normal;
            AudioManager.Instance.audioSource.PlayOneShot(valveUnjamSound);
        }
        else if (mode == ValveMode.Jammed) {
            if (GameManager.Instance != null) {
                valveMode = ValveMode.Jammed;
                SetJammedTurns();

                if (heatBarsLeft == 3) {
                    topHeatBarObject.gameObject.SetActive(false);
                }
                else if (heatBarsLeft == 2) {
                    midHeatBarObject.gameObject.SetActive(false);
                }
                else if (heatBarsLeft == 1) {
                    botHeatBarObject.gameObject.SetActive(false);
                }
                heatBarsLeft--;

                heatBarBackground.fillAmount -= 0.333f;
                
                if (heatBarsLeft <= 0) {
                    OnMistake();
                }

                if (heatBarsLeft == 2) {
                    heatCapPercent = 66.6f;
                }
                else if (heatBarsLeft == 1) {
                    heatCapPercent = 33.3f;
                }
                else if (heatBarsLeft == 0) {
                    heatCapPercent = 0.0f;
                }

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
        SetGameState(MicroGameState.Failed);
    }

    public override void OnWin() {
        SetGameState(MicroGameState.Won);
    }

    void ValveModeCheck() {
        if (currentHeat >= heatCapPercent && valveMode == ValveMode.Normal) {
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
                        visualsComponent.OnValveTurn(nextValveInput, progressGainedPerInput, false);
                        AudioManager.Instance.audioSource.PlayOneShot(valveTurnSounds[Random.Range(0, valveTurnSounds.Length - 1)]);
                    }
                    break;
                case NextValveInput.Up:
                    if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) {
                        IncreaseValveProgress();
                        IncreaseHeat();
                        nextValveInput++;
                        visualsComponent.OnValveTurn(nextValveInput, progressGainedPerInput, false);
                        AudioManager.Instance.audioSource.PlayOneShot(valveTurnSounds[Random.Range(0, valveTurnSounds.Length - 1)]);
                    }
                    break;

                case NextValveInput.Right:
                    if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) {
                        IncreaseValveProgress();
                        IncreaseHeat();
                        nextValveInput++;
                        visualsComponent.OnValveTurn(nextValveInput, progressGainedPerInput, false);
                        AudioManager.Instance.audioSource.PlayOneShot(valveTurnSounds[Random.Range(0, valveTurnSounds.Length - 1)]);
                    }
                    break;
                case NextValveInput.Down:
                    if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) {
                        IncreaseValveProgress();
                        IncreaseHeat();
                        nextValveInput = NextValveInput.Left;
                        visualsComponent.OnValveTurn(nextValveInput, progressGainedPerInput, false);
                        AudioManager.Instance.audioSource.PlayOneShot(valveTurnSounds[Random.Range(0, valveTurnSounds.Length - 1)]);
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
                        visualsComponent.OnValveTurn(nextValveInput, progressGainedPerInput, true);
                        
                    }

                    break;
                case NextValveInput.Up:
                    if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) {
                        turnsUntilUnjammed = Mathf.Clamp(turnsUntilUnjammed - 1, 0, 30);
                        nextValveInput = NextValveInput.Down;
                        visualsComponent.OnValveTurn(nextValveInput, progressGainedPerInput, true);
                    }

                    break;
                case NextValveInput.Right:
                    if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) {
                        turnsUntilUnjammed = Mathf.Clamp(turnsUntilUnjammed - 1, 0, 30);
                        nextValveInput = NextValveInput.Left;
                        visualsComponent.OnValveTurn(nextValveInput, progressGainedPerInput, true);
                    }
                    break;
                case NextValveInput.Down:
                    if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) {
                        turnsUntilUnjammed = Mathf.Clamp(turnsUntilUnjammed - 1, 0, 30);
                        nextValveInput = NextValveInput.Up;
                        visualsComponent.OnValveTurn(nextValveInput, progressGainedPerInput, true);
                    }
                    break;
            }
        }
    }
    void IncreaseValveProgress() {
        valveProgress = Mathf.Clamp(valveProgress + progressGainedPerInput, 0, 100);
    }
    void IncreaseHeat() {
        int r = Random.Range(5, heatRangeGainedPerInput + 1);
        currentHeat = Mathf.Clamp(currentHeat + r,0,101);
    }
    void ReduceHeat() {
        currentHeat = Mathf.Clamp(currentHeat - (Time.deltaTime * heatDissipationRate), 0, 100);
    }

    void SetJammedTurns() {
        int t = 0;
        if (GameManager.Instance != null) {
            t = Random.Range(10, 15 + GameManager.Instance.microGamesCompleted);

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
public enum NextValveInput {
    Left,
    Up,
    Right,
    Down
}
