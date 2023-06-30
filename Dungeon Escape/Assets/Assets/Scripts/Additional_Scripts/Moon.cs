using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Moon : MonoBehaviour
{
    [SerializeField]
    private GameObject _moonPrefab;
    [SerializeField]
    private Tilemap _tilemap;
    [SerializeField]
    private Tile _moonTile, _starsTile;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.name != "Player" && other.name != "Hit_Box")
            return;

        if (other.name == "Hit_Box")
        {
            Vector3Int moonPos = GameManager.Instance.GetCellPosition(_tilemap, _moonTile);
            GameManager.Instance.SetCellAtPosition(_tilemap, _starsTile, moonPos);
            Instantiate(_moonPrefab, transform.position, Quaternion.identity);
            AudioManager.Instance.PlayObjectBreakSFX();

            // Moon Cutter
            //Debug.Log("Moon Cutter");
            GameManager.Instance.DoAchievementUnlock(Achievements.AchievementsIDs.achievement_moon_cutter, (bool achievementUnlocked) =>
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

        //Destroy(gameObject);
    }
}
