using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectsAnimations : MonoBehaviour
{
    [SerializeField]
    private Animator _candleAnim, _lampAnim, _chestAnim;
    [SerializeField]
    private bool _chestOpened, _lampDestroyed, _candleDestroyed;

    [SerializeField]
    Player _player;

    [SerializeField]
    private GameObject _diamondPrefab;

    // Start is called before the first frame update
    void Start()
    {
        _chestOpened = false;
        _candleDestroyed = false;
        _lampDestroyed = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("other.name: " + other.name);
        if (other.name != "Player" && other.name != "Hit_Box")
            return;

        if (gameObject.tag == "Candle" && !_candleDestroyed && other.name == "Hit_Box")
        {
            _candleAnim.SetTrigger("Break");
            AudioManager.Instance.PlayObjectBreakSFX();
            _candleDestroyed = true;
            Instantiate(_diamondPrefab, transform.position, Quaternion.identity);
        }
        else if (gameObject.tag == "Lamp" && !_lampDestroyed && other.name == "Hit_Box")
        {
            _lampAnim.SetTrigger("Break");
            AudioManager.Instance.PlayObjectBreakSFX();
            _lampDestroyed = true;
            Instantiate(_diamondPrefab, transform.position, Quaternion.identity);
        }
        else if (gameObject.tag == "Chest" && !_chestOpened && other.name == "Player")
        {
            _chestAnim.SetTrigger("Open");
            _chestOpened = true;
            _player.AddGems(25);
            AudioManager.Instance.PlayGettingCollectibleSFX();
        }
    }

    public void DestroyObject()
    {
        Destroy(gameObject);
    }
}
