using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private static UIManager _instance;
    public static UIManager Instance
    {
        get
        {
            if(_instance == null)
            {
                Debug.LogError("UI Manager is NULL");
            }

            return _instance;
        }
    }

    [SerializeField]
    private Text _playerGemCountText;
    [SerializeField]
    private Image _selectionImage;
    [SerializeField]
    private Text gemCountText;
    [SerializeField]
    private Text gemCountTextShop;
    [SerializeField]
    private Image[] lives;
    public Button getGemsButton;
    [SerializeField]
    private Image _fadeOutImage;
    [SerializeField]
    private Image _homeButtonImage;
    [SerializeField]
    private Image _aButtonImage, _bButtonImage, _JoystickImage, _upArrowImage, _rightArrowImage, _downArrowImage, _leftArrowImage;
    [SerializeField] Text _bootsPrice, _keyPrice;
    [SerializeField]
    private Button _buyItemButton;
    [SerializeField]
    private Button _appleButton;
    private Color _enabledColor;
    private Color _disabledColor;
    [SerializeField]
    private GameObject _needKeyPanel;
    [SerializeField]
    private Text _clock;
    [SerializeField]
    private Text _fps;
    [SerializeField]
    private Image _bootsImage;
    [SerializeField]
    private Image _keyImage;
    [SerializeField]
    private Animator _anim;


    private int _lifeBarSpeed = 5;
    [SerializeField]
    private GameObject _subtitlesPanel;
    [SerializeField]
    private Text _subtitles;

    private void Awake()
    {
        _instance = this;
    }

    private void Start()
    {
        _enabledColor = _buyItemButton.image.color;
        _disabledColor = new Color(_enabledColor.r, _enabledColor.g, _enabledColor.b, _enabledColor.a);
        _disabledColor.a = 0.3f;
        
        Vector3 clockPos = _clock.gameObject.transform.position;
        clockPos.x = Screen.width / 2;
        _clock.gameObject.transform.position = clockPos;
        _anim.SetTrigger("Start");
        _subtitlesPanel.gameObject.SetActive(false);
        _subtitles.text = "";
    }

    private void Update()
    {
        if (!GameManager.Instance.GameComplete)
        {
            _clock.text = GameManager.Instance.DisplayTime;
        }
        else
        {
            _clock.text = GameManager.Instance.CurrentBeatTime;
        }

        // Only display FPS if deltaTime is greater than zero
        if (GameManager.Instance.DeltaTime> 0.0f)
        {
            _fps.text = "FPS: " + Mathf.Round(GameManager.Instance.SmoothedFPS).ToString();
        }
    }


    public void OpenShop(int gemCount)
    {
        _playerGemCountText.text = gemCount.ToString() + "G";
    }

    public void UpdateShopSelection(int yPos)
    {
        _selectionImage.rectTransform.anchoredPosition = new Vector2(_selectionImage.rectTransform.anchoredPosition.x, yPos);
    }

    public void UpdateBuyItem(int item, bool canBuy)
    {
        if(!canBuy)
        {
            _buyItemButton.enabled = false;
            _buyItemButton.image.color = _disabledColor;
            return;
        }

        switch (item)
        {
            case 0:
                for (int i = 0; i < lives.Length; i++)
                {
                    if (!lives[i].enabled)
                    {
                        _buyItemButton.enabled = true;
                        _buyItemButton.image.color = _enabledColor;

                        return;
                    }
                }
                _buyItemButton.enabled = false;
                _buyItemButton.image.color = _disabledColor;
                break;
            case 1:
                if (_bootsPrice.text == "SOLD OUT")
                {
                    _buyItemButton.enabled = false;
                    _buyItemButton.image.color = _disabledColor;
                }
                else
                {
                    _buyItemButton.enabled = true;
                    _buyItemButton.image.color = _enabledColor;
                }
                break;
            case 2:
                if (_keyPrice.text == "SOLD OUT")
                {
                    _buyItemButton.enabled = false;
                    _buyItemButton.image.color = _disabledColor;
                }
                else
                {
                    _buyItemButton.enabled = true;
                    _buyItemButton.image.color = _enabledColor;
                }
                break;
        }
    }

    public void ResetSelectionCursor()
    {
        _appleButton.onClick.Invoke();
    }

    public void UpdateAfterBuyApple(bool canBuyAnother)
    {
        for (int i = 0; i < lives.Length; i++)
        {
            if (!lives[i].enabled && canBuyAnother)
            {
                _buyItemButton.enabled = true;
                _buyItemButton.image.color = _enabledColor;

                return;
            }
        }
        _buyItemButton.enabled = false;
        _buyItemButton.image.color = _disabledColor;
    }

    public void UpdateBootsSoldOut()
    {
        _bootsPrice.text = "SOLD OUT";
        _buyItemButton.enabled = false;
        _buyItemButton.image.color = _disabledColor;
        _bootsImage.enabled = true;
    }

    public void UpdateKeySoldOut()
    {
        _keyPrice.text = "SOLD OUT";
        _buyItemButton.enabled = false;
        _buyItemButton.image.color = _disabledColor;
        _keyImage.enabled = true;
    }

    public void UpdateGemCount(int count)
    {
        gemCountText.text = "" + count;
        gemCountTextShop.text = "" + count + "G";
    }

    public void UpdateLives(int livesRemaining, bool decrease = true)
    {
        if (GameManager.Instance.IsDead)
            return;

        if(decrease)
            lives[livesRemaining].enabled = false;
        else
            lives[livesRemaining - 1].enabled = true;
    }

    public void UpdateLivesOnSuddenDeath()
    {
        for (int i = 0; i < lives.Length; i++)
        {
            lives[i].enabled = false;
        }
    }

    public void UpdateLivesOnKonamiCode()
    {
        for (int i = 0; i < lives.Length; i++)
        {
            lives[i].enabled = true;
        }
    }

    public void StartFadeOut(string sceneToLoad)
    {
        StartCoroutine(FadeOut(1f, sceneToLoad));
    }

    IEnumerator FadeOut(float time, string sceneToLoad)
    {
        _fadeOutImage.gameObject.SetActive(true);
        Color color = _fadeOutImage.color;
        color.a = 0f;

        for (float t = 0.0f; t < time; t += Time.deltaTime)
        {
            float alpha = Mathf.Lerp(0.0f, 1.0f, Mathf.Min(1.0f, t / time));
            color.a = alpha;
            _fadeOutImage.color = color;
            _aButtonImage.color = color;
            _bButtonImage.color = color;
            _JoystickImage.color = color;
            _upArrowImage.color = color;
            _rightArrowImage.color = color;
            _downArrowImage.color = color;
            _leftArrowImage.color = color;
            _homeButtonImage.color = color;
            yield return null;
        }

        if (sceneToLoad == "Win")
        {
            AudioManager.Instance.StopGameSounds();
            GameManager.Instance.DisplayWinScreen(true);
        }
        else if (sceneToLoad == "Game_Over")
        {
            AudioManager.Instance.StopGameSounds();
            GameManager.Instance.DisplayGameOverScreen(true);
        }
    }

    public void EnableNeedKeyPanel()
    {
        _needKeyPanel.SetActive(true);
    }

    public void DisableNeedKeyPanel()
    {
        _needKeyPanel.SetActive(false);
    }

    public void SetClockDisplay(int val)
    {
        if (val == 1)
            _clock.enabled = true;
        else
            _clock.enabled = false;
    }

    public void UpdateEnemyLifeBar(Slider _slider, float targetHealth, float currentVelocity)
    {
        float currentHealthBarVal = Mathf.SmoothDamp(_slider.value, targetHealth, ref currentVelocity, _lifeBarSpeed * Time.deltaTime);
        _slider.value = currentHealthBarVal;
    }

    public void DisplaySubtitles()
    {
        StartCoroutine(DisplaySubtitlesRoutine());
    }

    IEnumerator DisplaySubtitlesRoutine()
    {
        _subtitles.lineSpacing = 1.2f;
        _subtitlesPanel.gameObject.SetActive(true);
        if (PlayerPrefs.GetInt(GameManager.Instance.UserIdentifier + "_" + "alternateSFXToggle", 0) == 1)
        {
            if (!GameManager.Instance.HintProvided)
            {
                _subtitles.text = "Be careful around here, stranger.\n";
                yield return new WaitForSeconds(3f);
                _subtitles.text = "I've seen some strange folk talking about\nsome old cheat code or something";
                yield return new WaitForSeconds(6f);
                _subtitles.text = "that can make 'em real powerful.";
                yield return new WaitForSeconds(2f);
                _subtitles.text = "I don't know all the details, " +
                                  "but it's\nprobably best to steer clear of them.";
                yield return new WaitForSeconds(5f);
            }
            else
            {
                _subtitles.text = "What're ya buyin'?";
                yield return new WaitForSeconds(2f);
            }
        }
        else
        {
            _subtitles.text = "Welcome!";
            yield return new WaitForSeconds(1f);
        }




        _subtitles.text = "";
        _subtitlesPanel.gameObject.SetActive(false);
    }

    public void DisableSubtitles()
    {
        _subtitles.text = "";
        _subtitlesPanel.gameObject.SetActive(false);
    }

    public void HideButtons()
    {
        _aButtonImage.enabled = false;
        _bButtonImage.enabled = false;
    }

    public void ShowButtons()
    {
        _aButtonImage.enabled = true;
        _bButtonImage.enabled = true;
    }
}
