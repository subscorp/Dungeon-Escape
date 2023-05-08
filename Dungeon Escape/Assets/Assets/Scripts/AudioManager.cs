using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private static AudioManager _instance;
    public static AudioManager Instance
    {
        get
        {
            if(_instance == null)
            {
                Debug.LogError("Instance is NULL");
            }
            return _instance;
        }
    }

    [SerializeField]
    private AudioSource _music;
    [SerializeField]
    private AudioSource _playerAttackSFX;
    [SerializeField]
    private AudioSource _playerAttackSFXAlternate;
    [SerializeField]
    private AudioSource _playerHitSFX;
    [SerializeField]
    private AudioSource _collectibleSFXAlternate;
    [SerializeField]
    private AudioSource _collectibleSFX;
    [SerializeField]
    private AudioSource _playerHitSFXAlternate;
    [SerializeField]
    private AudioSource _mossGiantHitSFX;
    [SerializeField]
    private AudioSource _mossGiantHitSFXAlternate;
    [SerializeField]
    private AudioSource _skeletonHitSFX;
    [SerializeField]
    private AudioSource _spiderHitSFX;
    [SerializeField]
    private AudioSource _skeletonHitSFXAlternate;
    [SerializeField]
    private AudioSource _spiderHitSFXAlternate;
    [SerializeField]
    private AudioSource _merchentDialogAlternate;
    [SerializeField]
    private AudioSource _merchentDialog;
    [SerializeField]
    private AudioSource _biteSFX;
    [SerializeField]
    private AudioSource _biteSFXAlternate;
    [SerializeField]
    private AudioSource _jumpSFX;
    [SerializeField]
    private AudioSource _jumpSFXAlternate;
    [SerializeField]
    private AudioSource _impaleSFX;
    [SerializeField]
    private AudioSource _impaleAlternate;
    [SerializeField]
    private AudioSource _playerDeathSFX;
    [SerializeField]
    private AudioSource _playerDeathSFXAlternate;
    [SerializeField]
    private AudioSource _mossGiantDeathSFX;
    [SerializeField]
    private AudioSource _mossGiantDeathSFXAlternate;
    [SerializeField]
    private AudioSource _skeletonDeathSFX;
    [SerializeField]
    private AudioSource _skeletonDeathSFXAlternate;
    [SerializeField]
    private AudioSource _homeButtonSFX;
    [SerializeField]
    private AudioSource _homeButtonSFXAlternate;
    [SerializeField]
    private AudioSource _soldOutSFX;
    [SerializeField]
    private AudioSource _soldOutSFXAlternate;
    [SerializeField]
    private AudioSource _selectionSwitchSFXAlternate;
    [SerializeField]
    private AudioSource _selectionSwitchSFX;
    [SerializeField]
    private AudioSource _spiderSpitSFX;
    [SerializeField]
    private AudioSource _spiderSpitSFXAlternate;
    [SerializeField]
    private AudioSource _winMusic;
    [SerializeField]
    private AudioSource _gameOverMusic;

    // Konami Code related
    [SerializeField]
    private AudioSource _konamiCorrectSFX;
    [SerializeField]
    private AudioSource _konamiCorrectSFXAlternative;
    [SerializeField]
    private AudioSource _konamiWrongSFX;
    [SerializeField]
    private AudioSource _konamiWrongSFXAlternative;
    [SerializeField]
    private AudioSource _konamiCodeHint;
    [SerializeField]
    private AudioSource _fireSwordSFX;
    [SerializeField]
    private AudioSource _fireSwordSFXAlternative;

    private void Awake()
    {
        _instance = this;
    }

    public void Start()
    {
        _music.volume = PlayerPrefs.GetFloat(GameManager.Instance.UserIdentifier + "_" + "Music", 0.6f);
        _winMusic.volume = PlayerPrefs.GetFloat(GameManager.Instance.UserIdentifier + "_" + "Music", 0.6f);
        _gameOverMusic.volume = PlayerPrefs.GetFloat(GameManager.Instance.UserIdentifier + "_" + "Music", 0.6f);
        _playerAttackSFXAlternate.volume = PlayerPrefs.GetFloat(GameManager.Instance.UserIdentifier + "_" + "SFX", 0.6f);
        _playerAttackSFX.volume = PlayerPrefs.GetFloat(GameManager.Instance.UserIdentifier + "_" + "SFX", 0.6f);
        _playerHitSFXAlternate.volume = PlayerPrefs.GetFloat(GameManager.Instance.UserIdentifier + "_" + "SFX", 0.6f);
        _playerHitSFX.volume = PlayerPrefs.GetFloat(GameManager.Instance.UserIdentifier + "_" + "SFX", 0.6f);
        _collectibleSFXAlternate.volume = PlayerPrefs.GetFloat(GameManager.Instance.UserIdentifier + "_" + "SFX", 0.6f);
        _collectibleSFX.volume = PlayerPrefs.GetFloat(GameManager.Instance.UserIdentifier + "_" + "SFX", 0.6f);
        _mossGiantHitSFX.volume = PlayerPrefs.GetFloat(GameManager.Instance.UserIdentifier + "_" + "SFX", 0.6f);
        _mossGiantHitSFXAlternate.volume = PlayerPrefs.GetFloat(GameManager.Instance.UserIdentifier + "_" + "SFX", 0.6f);
        _skeletonHitSFXAlternate.volume = PlayerPrefs.GetFloat(GameManager.Instance.UserIdentifier + "_" + "SFX", 0.6f);
        _skeletonHitSFX.volume = PlayerPrefs.GetFloat(GameManager.Instance.UserIdentifier + "_" + "SFX", 0.6f);
        _spiderHitSFXAlternate.volume = PlayerPrefs.GetFloat(GameManager.Instance.UserIdentifier + "_" + "SFX", 0.6f);
        _spiderHitSFX.volume = PlayerPrefs.GetFloat(GameManager.Instance.UserIdentifier + "_" + "SFX", 0.6f);
        _merchentDialogAlternate.volume = PlayerPrefs.GetFloat(GameManager.Instance.UserIdentifier + "_" + "SFX", 0.6f);
        _merchentDialog.volume = PlayerPrefs.GetFloat(GameManager.Instance.UserIdentifier + "_" + "SFX", 0.6f);
        _biteSFX.volume = PlayerPrefs.GetFloat(GameManager.Instance.UserIdentifier + "_" + "SFX", 0.6f);
        _biteSFXAlternate.volume = PlayerPrefs.GetFloat(GameManager.Instance.UserIdentifier + "_" + "SFX", 0.6f);
        _jumpSFXAlternate.volume = PlayerPrefs.GetFloat(GameManager.Instance.UserIdentifier + "_" + "SFX", 0.6f);
        _jumpSFX.volume = PlayerPrefs.GetFloat(GameManager.Instance.UserIdentifier + "_" + "SFX", 0.6f);
        _impaleAlternate.volume = PlayerPrefs.GetFloat(GameManager.Instance.UserIdentifier + "_" + "SFX", 0.6f);
        _impaleSFX.volume = PlayerPrefs.GetFloat(GameManager.Instance.UserIdentifier + "_" + "SFX", 0.6f);
        _playerDeathSFXAlternate.volume = PlayerPrefs.GetFloat(GameManager.Instance.UserIdentifier + "_" + "SFX", 0.6f);
        _playerDeathSFX.volume = PlayerPrefs.GetFloat(GameManager.Instance.UserIdentifier + "_" + "SFX", 0.6f);
        _mossGiantDeathSFXAlternate.volume = PlayerPrefs.GetFloat(GameManager.Instance.UserIdentifier + "_" + "SFX", 0.6f);
        _mossGiantDeathSFX.volume = PlayerPrefs.GetFloat(GameManager.Instance.UserIdentifier + "_" + "SFX", 0.6f);
        _skeletonDeathSFX.volume = PlayerPrefs.GetFloat(GameManager.Instance.UserIdentifier + "_" + "SFX", 0.6f);
        _skeletonDeathSFXAlternate.volume = PlayerPrefs.GetFloat(GameManager.Instance.UserIdentifier + "_" + "SFX", 0.6f);
        _homeButtonSFX.volume = PlayerPrefs.GetFloat(GameManager.Instance.UserIdentifier + "_" + "SFX", 0.6f);
        _homeButtonSFXAlternate.volume = PlayerPrefs.GetFloat(GameManager.Instance.UserIdentifier + "_" + "SFX", 0.6f);
        _soldOutSFXAlternate.volume = PlayerPrefs.GetFloat(GameManager.Instance.UserIdentifier + "_" + "SFX", 0.6f);
        _soldOutSFX.volume = PlayerPrefs.GetFloat(GameManager.Instance.UserIdentifier + "_" + "SFX", 0.6f);
        _selectionSwitchSFX.volume = PlayerPrefs.GetFloat(GameManager.Instance.UserIdentifier + "_" + "SFX", 0.6f);
        _selectionSwitchSFXAlternate.volume = PlayerPrefs.GetFloat(GameManager.Instance.UserIdentifier + "_" + "SFX", 0.6f);

        // Konami Code Related
        _konamiCorrectSFX.volume = PlayerPrefs.GetFloat(GameManager.Instance.UserIdentifier + "_" + "SFX", 0.6f);
        _konamiCorrectSFXAlternative.volume = PlayerPrefs.GetFloat(GameManager.Instance.UserIdentifier + "_" + "SFX", 0.6f);
        _konamiWrongSFX.volume = PlayerPrefs.GetFloat(GameManager.Instance.UserIdentifier + "_" + "SFX", 0.6f);
        _konamiWrongSFXAlternative.volume = PlayerPrefs.GetFloat(GameManager.Instance.UserIdentifier + "_" + "SFX", 0.6f);
        _konamiCodeHint.volume = PlayerPrefs.GetFloat(GameManager.Instance.UserIdentifier + "_" + "SFX", 0.6f);
        _fireSwordSFX.volume = PlayerPrefs.GetFloat(GameManager.Instance.UserIdentifier + "_" + "SFX", 0.6f);
        _fireSwordSFXAlternative.volume = PlayerPrefs.GetFloat(GameManager.Instance.UserIdentifier + "_" + "SFX", 0.6f);

        Debug.Log("In AudioManager::Start");
        Debug.Log("GameManager.Instance.UserIdentifier: " + GameManager.Instance.UserIdentifier);
        Debug.Log("PlayerPrefs.GetFloat(GameManager.Instance.UserIdentifier + '_' + 'Music', 0.6f);" + PlayerPrefs.GetFloat(GameManager.Instance.UserIdentifier + "_" + "Music", 0.6f));
        Debug.Log("PlayerPrefs.GetFloat(GameManager.Instance.UserIdentifier + '_' + 'SFX', 0.6f);" + PlayerPrefs.GetFloat(GameManager.Instance.UserIdentifier + "_" + "SFX", 0.6f));
        Debug.Log("_homeButtonSFX.volume: " + _homeButtonSFX.volume);
        Debug.Log("music.volume: " + _music.volume);
    }

    public void PlayMusic()
    {
        _music.Play();
    }

    public void PlayAttackSound()
    {
        if (GameManager.Instance.GotKonamiCode)
        {
            if (PlayerPrefs.GetInt(GameManager.Instance.UserIdentifier + "_" + "alternateSFXToggle", 0) == 1)
                _fireSwordSFXAlternative.Play();
            else
                _fireSwordSFX.Play();
        }
        else
        {
            if (PlayerPrefs.GetInt(GameManager.Instance.UserIdentifier + "_" + "alternateSFXToggle", 0) == 1)
                _playerAttackSFXAlternate.Play();
            else
                _playerAttackSFX.Play();
        }
    }

    internal void StopGameSounds()
    {
        _music.mute = true;
        _playerAttackSFX.mute = true;
        _playerAttackSFXAlternate.mute = true;
        _playerHitSFX.mute = true;
        _collectibleSFXAlternate.mute = true;
        _collectibleSFX.mute = true;
        _playerHitSFXAlternate.mute = true;
        _mossGiantHitSFX.mute = true;
        _mossGiantHitSFXAlternate.mute = true;
        _skeletonHitSFX.mute = true;
        _spiderHitSFX.mute = true;
        _skeletonHitSFXAlternate.mute = true;
        _spiderHitSFXAlternate.mute = true;
        _merchentDialogAlternate.mute = true;
        _merchentDialog.mute = true;
        _biteSFX.mute = true;
        _biteSFXAlternate.mute = true;
        _jumpSFX.mute = true;
        _jumpSFXAlternate.mute = true;
        _mossGiantDeathSFX.mute = true;
        _mossGiantDeathSFXAlternate.mute = true;
        _skeletonDeathSFX.mute = true;
        _skeletonDeathSFXAlternate.mute = true;
        _soldOutSFX.mute = true;
        _soldOutSFXAlternate.mute = true;
        _selectionSwitchSFXAlternate.mute = true;
        _selectionSwitchSFX.mute = true;
        _spiderSpitSFX.mute = true;
        _spiderSpitSFXAlternate.mute = true;
        _konamiCorrectSFX.mute = true;
        _konamiCorrectSFXAlternative.mute = true;
        _konamiWrongSFX.mute = true;
        _konamiWrongSFXAlternative.mute = true;
        _konamiCodeHint.mute = true;
    }

    public void PlayGettingHitSound()
    {
        if (PlayerPrefs.GetInt(GameManager.Instance.UserIdentifier + "_" + "alternateSFXToggle", 0) == 1)
            _playerHitSFXAlternate.Play();
        else
            _playerHitSFX.Play();
    }

    public void PlayGettingCollectibleSFX()
    {
        if (PlayerPrefs.GetInt(GameManager.Instance.UserIdentifier + "_" + "alternateSFXToggle", 0) == 1)
            _collectibleSFXAlternate.Play();
        else
            _collectibleSFX.Play();
    }

    public void PlayMossGiantHitSound()
    {
        if (PlayerPrefs.GetInt(GameManager.Instance.UserIdentifier + "_" + "alternateSFXToggle", 0) == 1)
            _mossGiantHitSFXAlternate.Play();
        else
            _mossGiantHitSFX.Play();
    }

    public void PlaySkeletonHitSound()
    {
        if (PlayerPrefs.GetInt(GameManager.Instance.UserIdentifier + "_" + "alternateSFXToggle", 0) == 1)
            _skeletonHitSFXAlternate.Play();
        else
            _skeletonHitSFX.Play();
    }

    public void PlaySpiderHitSound()
    {
        if (PlayerPrefs.GetInt(GameManager.Instance.UserIdentifier + "_" + "alternateSFXToggle", 0) == 1)
            _spiderHitSFXAlternate.Play();
        else
            _spiderHitSFX.Play();
    }

    public void PlayMerchentGreeting(bool isAlt, bool isHint)
    {
        if (!isAlt)
        {
            _merchentDialog.Play();
        }
        else
        {
            if (isHint)
                _konamiCodeHint.Play();
            else
                _merchentDialogAlternate.Play();
        }
    }

    public void PlayBiteSound()
    {
        if (PlayerPrefs.GetInt(GameManager.Instance.UserIdentifier + "_" + "alternateSFXToggle", 0) == 1)
            _biteSFXAlternate.Play();
        else
            _biteSFX.Play();
    }

    public void PlayJumpSound()
    {
        if (PlayerPrefs.GetInt(GameManager.Instance.UserIdentifier + "_" + "alternateSFXToggle", 0) == 1)
            _jumpSFXAlternate.Play();
        else
            _jumpSFX.Play();
    }

    public void PlayImpaleSound()
    {
        if (PlayerPrefs.GetInt(GameManager.Instance.UserIdentifier + "_" + "alternateSFXToggle", 0) == 1)
            _impaleAlternate.Play();
        else
            _impaleSFX.Play();
    }

    public void PlayPlayerDeathSFX()
    {
        if (PlayerPrefs.GetInt(GameManager.Instance.UserIdentifier + "_" + "alternateSFXToggle", 0) == 1)
            _playerDeathSFXAlternate.Play();
        else
            _playerDeathSFX.Play();
    }

    public void PlayMossGiantDeathSFX()
    {
        if (PlayerPrefs.GetInt(GameManager.Instance.UserIdentifier + "_" + "alternateSFXToggle", 0) == 1)
            _mossGiantDeathSFXAlternate.Play();
        else
            _mossGiantDeathSFX.Play();
    }

    public void PlaySkeletonDeathSFX()
    {
        if (PlayerPrefs.GetInt(GameManager.Instance.UserIdentifier + "_" + "alternateSFXToggle", 0) == 1)
            _skeletonDeathSFXAlternate.Play();
        else
            _skeletonDeathSFX.Play();
    }

    public void PlayHomeButtonSFX()
    {
        if (PlayerPrefs.GetInt(GameManager.Instance.UserIdentifier + "_" + "alternateSFXToggle", 0) == 1)
            _homeButtonSFXAlternate.Play();
        else
            _homeButtonSFX.Play();
    }

    public void PlaySoldOutSFX()
    {
        if (PlayerPrefs.GetInt(GameManager.Instance.UserIdentifier + "_" + "alternateSFXToggle", 0) == 1)
            _soldOutSFXAlternate.Play();
        else
            _soldOutSFX.Play();
    }

    public void PlaySelectionSwitchSFX()
    {
        if (PlayerPrefs.GetInt(GameManager.Instance.UserIdentifier + "_" + "alternateSFXToggle", 0) == 1)
            _selectionSwitchSFXAlternate.Play();
        else
            _selectionSwitchSFX.Play();
    }

    public void PlaySpiderSpit()
    {
        if (PlayerPrefs.GetInt(GameManager.Instance.UserIdentifier + "_" + "alternateSFXToggle", 0) == 1)
            _spiderSpitSFXAlternate.Play();
        else
            _spiderSpitSFX.Play();
    }

    public void PlayWinMusic()
    {
        _winMusic.Play();
    }

    public void PlayGameOverMusic()
    {
        _gameOverMusic.Play();
    }

    // Konami Code Related
    public void PlayKonamiCodeCorrectSFX()
    {
        if (PlayerPrefs.GetInt(GameManager.Instance.UserIdentifier + "_" + "alternateSFXToggle", 0) == 1)
            _konamiCorrectSFXAlternative.Play();
        else
            _konamiCorrectSFX.Play();
    }

    public void PlayKonamiCodeWrongSFX()
    {
        if (PlayerPrefs.GetInt(GameManager.Instance.UserIdentifier + "_" + "alternateSFXToggle", 0) == 1)
            _konamiWrongSFXAlternative.Play();
        else
            _konamiWrongSFX.Play();
    }

    public void FadeStopKonamiCodeHint()
    {
        StartCoroutine(StartFade(_konamiCodeHint, 3f, 0));
    }

    public void AbruptStopMerchantDialog()
    {
        _merchentDialogAlternate.Stop();
    }

    public void AbruptStopKonamiCodeHint()
    {
        _konamiCodeHint.Stop();
    }

    public static IEnumerator StartFade(AudioSource audioSource, float duration, float targetVolume)
    {
        float currentTime = 0;
        float start = audioSource.volume;
        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(start, targetVolume, currentTime / duration);
            yield return null;
        }
        yield break;
    }
}