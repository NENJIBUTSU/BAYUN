using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public static GameManager Instance { get; private set; }

    [SerializeField,CustomAttributes.ReadOnly] LoadingManager loadingManager;


    [Header("Microgame Settings")]
    [SerializeField] SO_MicroGameList microGameList;
    public bool noMistakes;

    [Header("Game State")]
    [SerializeField] GameState gameState;
    [SerializeField] public int microGamesCompleted;
    [SerializeField] MicroGame currentMicroGame;
    [SerializeField] Transition currentTransition;

    [Header("Player")]
    public int maxLives;
    public int livesLeft;

    [SerializeField] TimerImage timerImage;
    [SerializeField] TirednessLives tirednessBar;


    private void Awake() {

        if (Instance != null && Instance != this) {
            Destroy(this);
            Instance = null;
        }
        else { Instance = this; DontDestroyOnLoad(gameObject); }

        tirednessBar.Initialize();
        //tirednessBar.gameObject.SetActive(false);
        timerImage.Initialize();
        //timerImage.gameObject.SetActive(false);
    }

    IEnumerator LoadRandomMicroGame() {
        gameState = GameState.Loading;
        yield return LoadingManager.Instance.LoadMicroGame(microGameList.list[Random.Range(0,microGameList.list.Count)]);
        currentMicroGame = GameObject.FindGameObjectWithTag("MicroGame").GetComponent<MicroGame>();
        currentMicroGame.transform.parent.gameObject.SetActive(false);
        currentTransition = GameObject.FindGameObjectWithTag("Transition").GetComponent<Transition>();
        timerImage.Initialize();
        gameState = GameState.Transition;

    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            StartCoroutine(LoadRandomMicroGame());
        }

        if (gameState == GameState.Transition && currentMicroGame != null) {
            if (currentTransition.isTransitionFinished) {
                gameState = GameState.Playing;
            }
            currentTransition?.UpdateTransition();
            Debug.Log("Updating transition.");
        }

        if (currentMicroGame != null && gameState == GameState.Playing) {
            currentMicroGame?.UpdateMicroGame();
            timerImage.UpdateTimerImage(currentMicroGame.GetTimeLeftPercentage());

            if (currentMicroGame.GetGameState() == MicroGameState.Failed) {
                OnMicroGameFail();
                gameState = GameState.Paused;
            }

            if (currentMicroGame.GetGameState() == MicroGameState.Won) {
                OnMicroGameWin();
                gameState = GameState.Paused;
            }
        }

    }

    public void OnMicroGameWin() {
        Debug.Log("MicroGame won!!!!!!!");
    }
    public void OnMicroGameFail() {
        Debug.Log("MicroGame LOST!!!!!!!!!!!!!!");
        livesLeft--;
        tirednessBar.UpdateLives(livesLeft);

        if (livesLeft <= 0) {
            //the game is lost.
        }
    }

    enum GameState {
        Paused,
        Playing,
        Loading,
        Transition
    }
}
