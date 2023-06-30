using UnityEngine;

public class Shop : MonoBehaviour
{
    [SerializeField]
    private GameObject _shopPanel;
    private int _currentItemSelected = 0;
    private int _currentItemCost = 10;
    private Player _player;
    [SerializeField]
    private GameObject _selection;
    bool justPlayedHint = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            GameManager.Instance.PlayerAtShop = true;
            UIManager.Instance.DisableButtons();
            // Display subtitles
            if (PlayerPrefs.GetInt(GameManager.Instance.UserIdentifier + "_" + "Subtitles", 1) == 1)
            {
                UIManager.Instance.DisplaySubtitles();
            }

            _player = other.GetComponent<Player>();
            if (_player != null)
            {
                GameManager.Instance.PlayerInShop = true;
                _currentItemSelected = 0;
                _currentItemCost = 10;
                UIManager.Instance.UpdateShopSelection(298);
                if (!(_player.getNumDiamonds() >= _currentItemCost))
                    UIManager.Instance.UpdateBuyItem(0, false);
                else
                    UIManager.Instance.UpdateBuyItem(0, true);
                UIManager.Instance.OpenShop(_player.getNumDiamonds());
                HandleGreetingDialoge();
            }

            if (_shopPanel != null)
            {
                _shopPanel.SetActive(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            GameManager.Instance.PlayerAtShop = false;
            UIManager.Instance.EnableButtons();
            UIManager.Instance.DisableSubtitles();
            if(_shopPanel != null)
            {
                GameManager.Instance.PlayerInShop = false;
                _shopPanel.SetActive(false);
                if
                (GameManager.Instance.boughtAppleInCurrentVisit &&
                    GameManager.Instance.boughtBootsInCurrentVisit &&
                    GameManager.Instance.boughtKeyInCurrentVisit)
                {
                    //Debug.Log("Shopoholic!");
                    GameManager.Instance.DoAchievementUnlock(Achievements.AchievementsIDs.achievement_shopaholic, (bool achievementUnlocked) =>
                    {
                        if (achievementUnlocked)
                        {
                            // The achievement was unlocked, so increment the Completionist achievement
                            GameManager.Instance.DoAchievementIncrement(Achievements.AchievementsIDs.achievement_on_track_to_completion);
                            GameManager.Instance.DoAchievementIncrement(Achievements.AchievementsIDs.achievement_still_on_track_to_completion);
                            GameManager.Instance.DoAchievementIncrement(Achievements.AchievementsIDs.achievement_completionist);
                        }
                    });
                }
                else
                {
                    GameManager.Instance.boughtAppleInCurrentVisit = false;
                    GameManager.Instance.boughtBootsInCurrentVisit = false;
                    GameManager.Instance.boughtKeyInCurrentVisit = false;
                }

                if (justPlayedHint)
                {
                    if(!AudioManager.Instance.IsHintStillPlaying())
                    {
                        //Debug.Log("Its Dangerous Out There achievement");
                        GameManager.Instance.DoAchievementUnlock(Achievements.AchievementsIDs.achievement_its_dangerous_out_there, (bool achievementUnlocked) =>
                        {
                            if (achievementUnlocked)
                            {
                                // The achievement was unlocked, so increment the Completionist achievement
                                GameManager.Instance.DoAchievementIncrement(Achievements.AchievementsIDs.achievement_on_track_to_completion);
                                GameManager.Instance.DoAchievementIncrement(Achievements.AchievementsIDs.achievement_still_on_track_to_completion);
                                GameManager.Instance.DoAchievementIncrement(Achievements.AchievementsIDs.achievement_completionist);
                            }
                        });
                    }
                    AudioManager.Instance.FadeStopKonamiCodeHint();
                }

                if(GameManager.Instance.HasBootsOfFlight && !GameManager.Instance.DisplayedBootsInstructions)
                {
                    GameManager.Instance.DisplayedBootsInstructions = true;
                    UIManager.Instance.DisplayBootsOfFlightInstructions();
                    GameManager.Instance.EnableOrDisableBootsOfFlight();
                }
            }
        }
    }

    public void SelectItem(int item)
    {
        AudioManager.Instance.PlaySelectionSwitchSFX();
        _currentItemSelected = item;
        switch(item)
        {
            case 0:
                UIManager.Instance.UpdateShopSelection(298);
                _currentItemCost = 10;

                if (!(_player.getNumDiamonds() >= _currentItemCost))
                    UIManager.Instance.UpdateBuyItem(0, false);
                else
                    UIManager.Instance.UpdateBuyItem(0, true);

                break;
            case 1:
                UIManager.Instance.UpdateShopSelection(208);
                _currentItemCost = 175;

                if (!(_player.getNumDiamonds() >= _currentItemCost))
                    UIManager.Instance.UpdateBuyItem(1, false);
                else
                    UIManager.Instance.UpdateBuyItem(1, true);
                break;
            case 2:
                UIManager.Instance.UpdateShopSelection(123);
                _currentItemCost = 100;

                if (!(_player.getNumDiamonds() >= _currentItemCost))
                    UIManager.Instance.UpdateBuyItem(2, false);
                else
                    UIManager.Instance.UpdateBuyItem(2, true);
                break;
        }
    }

    public void BuyItem()
    {
        if (_currentItemSelected == 0)
        {
            if (!GameManager.Instance.RefillHealth())
                return;

            //Debug.Log("An apple a day keeps the doctor away!"); // Achievement 8
            GameManager.Instance.DoAchievementUnlock(Achievements.AchievementsIDs.achievement_an_apple_a_day_keeps_the_doctor_away, (bool achievementUnlocked) =>
            {
                if (achievementUnlocked)
                {
                    // The achievement was unlocked, so increment the Completionist achievement
                    GameManager.Instance.DoAchievementIncrement(Achievements.AchievementsIDs.achievement_on_track_to_completion);
                    GameManager.Instance.DoAchievementIncrement(Achievements.AchievementsIDs.achievement_still_on_track_to_completion);
                    GameManager.Instance.DoAchievementIncrement(Achievements.AchievementsIDs.achievement_completionist);
                }
            });
            bool canBuyAnother = _player.getNumDiamonds() >= _currentItemCost * 2;
            UIManager.Instance.UpdateAfterBuyApple(canBuyAnother);
            AudioManager.Instance.PlayBiteSound();
            GameManager.Instance.boughtAppleInCurrentVisit = true;
        }
        else if(_currentItemSelected == 1)
        {
            UIManager.Instance.UpdateBootsSoldOut();
            AudioManager.Instance.PlaySoldOutSFX();
            GameManager.Instance.HasBootsOfFlight = true;
            GameManager.Instance.boughtBootsInCurrentVisit = true;
        }
        else if (_currentItemSelected == 2)
        {
            UIManager.Instance.UpdateKeySoldOut();
            AudioManager.Instance.PlaySoldOutSFX();
            GameManager.Instance.HasKeyToChest = true;
            GameManager.Instance.boughtKeyInCurrentVisit = true;
        }
        _player.setNumDiamonds(_player.getNumDiamonds() - _currentItemCost);
    }
    
    public void HandleGreetingDialoge()
    {
        if (PlayerPrefs.GetInt(GameManager.Instance.UserIdentifier + "_" + "alternateSFXToggle", 0) == 1)
        {
            float randomValue = UnityEngine.Random.value;
            if (!GameManager.Instance.HintProvided)
            {
                GameManager.Instance.HintProvided = true;
                AudioManager.Instance.AbruptStopMerchantDialog();
                AudioManager.Instance.PlayMerchentGreeting(true, true);
                justPlayedHint = true;
            }
            else
            {
                justPlayedHint = false;
                AudioManager.Instance.AbruptStopKonamiCodeHint();
                AudioManager.Instance.PlayMerchentGreeting(true, false);
            }
        }
        else
            AudioManager.Instance.PlayMerchentGreeting(false, false);
    }
}
