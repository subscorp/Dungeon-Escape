using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class QuitScreen : MonoBehaviour
{
    [SerializeField]
    private AudioSource _sfx;
    [SerializeField]
    private AudioSource _sfxAlternate;


    private void Start()
    {
        _sfx.volume = PlayerPrefs.GetFloat(GameManager.Instance.UserIdentifier + "_" + "SFX", 0.6f);
    }

    IEnumerator QuitCoRoutine()
    {
        if (PlayerPrefs.GetInt(GameManager.Instance.UserIdentifier + "_" + "alternateSFXToggle", 0) == 1)
            _sfxAlternate.Play();
        else
            _sfx.Play();
        yield return new WaitForSeconds(0.4f);

        SceneManager.LoadScene("Main_Menu");
    }

    public void QuitButton()
    {
        Time.timeScale = 1;
        StartCoroutine(QuitCoRoutine());
    }

    public void KeepPlayingButton()
    {
        if (PlayerPrefs.GetInt(GameManager.Instance.UserIdentifier + "_" + "alternateSFXToggle", 0) == 1)
            _sfxAlternate.Play();
        else
            _sfx.Play();

        Time.timeScale = 1;
        GameManager.Instance.DisplayReturnToGame(false);
    }
}
