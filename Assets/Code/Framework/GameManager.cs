using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public static GameManager Instance { get; private set; }

    [SerializeField,CustomAttributes.ReadOnly] LoadingManager loadingManager;


    [Header("Microgame Settings")]
    [SerializeField] SO_MicroGameList microGameList;
    public bool noMistakes;

    [Header("End Conditions")]
    [SerializeField] int microGamesWon;
    [SerializeField] int microGamesNeededToWin;

    [SerializeField] GameObject winScreen;
    [SerializeField] GameObject lossScreen;

    public int maxLives;
    public int livesLeft;

    [Header("Game State")]
    [SerializeField] GameState gameState;
    [SerializeField] public int microGamesCompleted;
    [SerializeField] MicroGame currentMicroGame;

    [Header("Transition Screens")]
    [SerializeField] TransitionScreen transitionScreen;
    [SerializeField] MicroGameIntro currentIntro;

    [Header("UI")]
    [SerializeField] TimerImage timerImage;
    [SerializeField] TirednessLives tirednessBar;

    [Header("Music")]

    [SerializeField] AudioClip[] musicTracks;
    [SerializeField] int currentMusicIndex;


    private void Awake() {

        if (Instance != null && Instance != this) {
            Destroy(this);
            Instance = null;
        }
        else { Instance = this; DontDestroyOnLoad(gameObject); }
    }

    private void Start() {
        tirednessBar.Initialize();
        tirednessBar.gameObject.SetActive(false);
        timerImage.Initialize();
        timerImage.gameObject.SetActive(false);

        AudioManager.Instance.musicAudioSource.clip = musicTracks[0];
        PlayNextTrack(AudioManager.Instance.musicAudioSource);
        AudioManager.Instance.musicAudioSource.Play();
        StartCoroutine(FadeAudioSource.StartFade(AudioManager.Instance.musicAudioSource, 0.5f, 0.1f));

        StartCoroutine(LoadRandomMicroGame());
    }

    IEnumerator LoadRandomMicroGame() {
        gameState = GameState.Loading;
        yield return LoadingManager.Instance.LoadMicroGame(microGameList.list[Random.Range(0,microGameList.list.Count)]);
        currentMicroGame = GameObject.FindGameObjectWithTag("MicroGame").GetComponent<MicroGame>();
        currentMicroGame.transform.parent.gameObject.SetActive(false);
        currentIntro = GameObject.FindGameObjectWithTag("Intro").GetComponent<MicroGameIntro>();

        transitionScreen.gameObject.SetActive(true);
        transitionScreen.TriggerWaitingScreen();
        while (!transitionScreen.transitionComponent.isIntroFinished) {
            Debug.Log("Hits transition!");
            transitionScreen.transitionComponent.UpdateIntro();
            yield return null;
        }

        transitionScreen.transitionComponent.Initialize("",0,0);
        transitionScreen.gameObject.SetActive(false);
        timerImage.Initialize();
        gameState = GameState.Intro;

        while (!currentIntro.isIntroFinished) {
            yield return null;
        }

        currentIntro.gameObject.SetActive(false);
        currentMicroGame.transform.parent.gameObject.SetActive(true);

        tirednessBar.gameObject.SetActive(true);
        timerImage.gameObject.SetActive(true);
        currentMicroGame.SetGameState(MicroGameState.Running);
    }

    private void Update() {
        /*if (Input.GetKeyDown(KeyCode.Space)) {
            StopAllCoroutines();
            StartCoroutine(LoadRandomMicroGame());
        }*/

        if (gameState == GameState.Intro && currentMicroGame != null) {
            if (currentIntro.isIntroFinished) {
                gameState = GameState.Playing;
            }
            currentIntro?.UpdateIntro();
            Debug.Log("Updating intro.");
        }

        if (currentMicroGame != null && gameState == GameState.Playing) {
            currentMicroGame?.UpdateMicroGame();
            timerImage.UpdateTimerImage(currentMicroGame.GetTimeLeftPercentage(), true);

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
        microGamesWon++;
        
        if (microGamesWon >= microGamesNeededToWin) {
            //win da game!
            OnGameWin();
        }
        else {
            transitionScreen.TriggerSuccessScreen();
            StartCoroutine(TransitionWrapper());
        }
    }
    public void OnMicroGameFail() {
        Debug.Log("MicroGame LOST!!!!!!!!!!!!!!");
        livesLeft--;
        tirednessBar.UpdateLives(livesLeft);

        if (livesLeft <= 0) {
            //the game is lost.
            OnGameLoss();
        }
        else {
            transitionScreen.TriggerFailScreen();
            StartCoroutine(TransitionWrapper());
        }
    }

    IEnumerator TransitionWrapper() {
        currentMicroGame.transform.parent.gameObject.SetActive(false);
        tirednessBar.gameObject.SetActive(false);
        timerImage.gameObject.SetActive(false);

        yield return StartCoroutine(StartTransition());
        yield return StartCoroutine(LoadRandomMicroGame());
    }

    IEnumerator StartTransition() {
        transitionScreen.gameObject.SetActive(true);
        while (!transitionScreen.transitionComponent.isIntroFinished) {
            Debug.Log("Hits transition!");
            transitionScreen.transitionComponent.UpdateIntro();
            yield return null;
        }
    }

    void OnGameLoss() {
        currentMicroGame.transform.parent.gameObject.SetActive(false);
        timerImage.gameObject.SetActive(false);
        tirednessBar.gameObject.SetActive(false);
        lossScreen.gameObject.SetActive(true);

    }

    void OnGameWin() {
        currentMicroGame.transform.parent.gameObject.SetActive(false);
        timerImage.gameObject.SetActive(false);
        tirednessBar.gameObject.SetActive(false);
        winScreen.gameObject.SetActive(true);
    }

    enum GameState {
        Paused,
        Playing,
        Loading,
        Transition,
        Intro
    }

    private IEnumerator SequenceMusic(AudioSource source) {
        // Cache the WaitForSeconds call, this is done because
        // like any other class WaitForSeconds causes garbage because it is heap allocated.
        // Therefore we cache it to simply reuse the same class instance.
        var waitForClipRemainingTime = new WaitForSeconds(source.GetClipRemainingTime());
        yield return waitForClipRemainingTime;
        PlayNextTrack(source);
    }

    void PlayNextTrack(AudioSource source) {
        if (currentMusicIndex < musicTracks.Length - 1) {
            currentMusicIndex++;
        }
        else {
            currentMusicIndex = 0;
        }
        source.clip = musicTracks[currentMusicIndex];
        AudioManager.Instance.musicAudioSource.Play();
        StartCoroutine(SequenceMusic(source));
    }

    public void DestroySelf() {
        Destroy(this.gameObject);
        Instance = null;
    }
}
