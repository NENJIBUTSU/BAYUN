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
    [SerializeField] MicroGame currentMicroGame;
    //[SerializeField] GameState gameState;

    [Header("Player")]
    [SerializeField] int tiredness;


    private void Awake() {

        if (Instance != null && Instance != this) {
            Destroy(this);
            Instance = null;
        }
        else { Instance = this; DontDestroyOnLoad(gameObject); }
    }

    void LoadRandomMicroGame() {
        LoadingManager.Instance.LoadMicroGame(microGameList.list[Random.Range(0,microGameList.list.Count)]);
    }

    private void Update() {
        currentMicroGame?.UpdateMicroGame();

        if (Input.GetKeyDown(KeyCode.Space)) {
            LoadRandomMicroGame();
        }

    }

    public void OnMicroGameWin() {

    }
    public void OnMicroGameFail() {

    }
    public void OnMicroGameMistake() {

    }
}
