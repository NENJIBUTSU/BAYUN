using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] Canvas mainMenuScreen;
    [SerializeField] Canvas optionsScreen;

    private void Start() {
        //yadda yadda add music thing
    }
    public void PlayGame() {
        StartCoroutine(BeginPlayGame());
    }

    IEnumerator BeginPlayGame() {
        StartCoroutine(FadeAudioSource.StartFade(AudioManager.Instance.musicAudioSource, 0.5f, 0f));
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene("MainGame");
    }

    public void OpenOptionsMenu() {
        mainMenuScreen.gameObject.SetActive(false);
        optionsScreen.gameObject.SetActive(true);
    }

    public void BackToMainMenu() {
        optionsScreen.gameObject.SetActive(false);
        mainMenuScreen.gameObject.SetActive(true);
    }

    IEnumerator BeginQuitGame() {
        StartCoroutine(FadeAudioSource.StartFade(AudioManager.Instance.musicAudioSource, 0.25f, 0f));
        yield return new WaitForSeconds(0.25f);
        Application.Quit();
    }
    public void ExitApplication() {
        StartCoroutine(BeginQuitGame());
    }
}
