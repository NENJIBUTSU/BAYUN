using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingManager : MonoBehaviour
{
    public static LoadingManager Instance { get; private set; }
    private void Awake() {

        if (Instance != null && Instance != this) {
            Destroy(this);
            Instance = null;
        }
        else { Instance = this; DontDestroyOnLoad(gameObject); }
    }

    AsyncOperation loadingOperation;

    public void LoadMicroGame(string sceneToLoad) {
        if (SceneManager.sceneCount > 1) {
            BeginUnloadSceneAsync(SceneManager.GetSceneAt(1));
        }

        BeginLoadAsync(sceneToLoad);
    }


    public void BeginLoadAsync(string sceneToLoad) {
        loadingOperation = LoadSceneAsync(sceneToLoad,true);
    }

    void EndLoading() {

    }

    void LoadScene(string sceneToLoad, bool isAdditive) {
        if (isAdditive) {
            SceneManager.LoadScene(sceneToLoad, LoadSceneMode.Additive);
        }
        else {
            SceneManager.LoadScene(sceneToLoad, LoadSceneMode.Single);
        }
    }

    AsyncOperation LoadSceneAsync(string sceneToLoad, bool isAdditive) {
        if (isAdditive) {
            return SceneManager.LoadSceneAsync(sceneToLoad, LoadSceneMode.Additive);
        }
        else {
            return SceneManager.LoadSceneAsync(sceneToLoad, LoadSceneMode.Single);
        }
    }

    void BeginUnloadSceneAsync(Scene scene) {
        SceneManager.UnloadSceneAsync(scene);
    }

    private void UpdateLoadManager() {
        if (loadingOperation != null) {
            while (!loadingOperation.isDone) {
                //do loading screen stuff!
            }
        }

    }
}
