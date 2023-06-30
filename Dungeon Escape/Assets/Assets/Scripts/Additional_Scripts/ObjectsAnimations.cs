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

    [SerializeField]
    private GameObject _flyingDiamondPrefab;
    [SerializeField]
    private GameObject _keyToTheCastlePrefab;

    private const int _NUMGEMSFROMCHEST= 25;

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        _chestOpened = false;
        _candleDestroyed = false;
        _lampDestroyed = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
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
            if (GameManager.Instance.HasKeyToChest)
            {
                _chestAnim.SetTrigger("Open");
                _chestOpened = true;
                StartCoroutine(SpawnDiamondsFromChest());
                UIManager.Instance.UpdateKeyToChestUsed();
                AudioManager.Instance.PlayTreasureChestSFX();
            }
            else
            {
                UIManager.Instance.EnableNeedKeyToChestPanel();
            }
        }
        else if(gameObject.tag == "Key")
        {
            GameManager.Instance.HasKeyToCastle = true;
            UIManager.Instance.UpdateKeyToCastleObtained();
            AudioManager.Instance.PlayKeyObtainedSFX();
            Destroy(gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (gameObject.tag == "Chest" && !_chestOpened && other.name == "Player" && !GameManager.Instance.HasKeyToChest)
        {
            UIManager.Instance.DisableNeedKeyToChestPanel();
        }

    }

    public void DestroyObject()
    {
        Destroy(gameObject);
    }

    IEnumerator SpawnDiamondsFromChest()
    {
        for (int i = 0; i < _NUMGEMSFROMCHEST; i++)
        {
            yield return new WaitForSeconds(0.1f);
            Instantiate(_flyingDiamondPrefab, transform.position, Quaternion.identity);
        }
        yield return new WaitForSeconds(0.5f);
        Vector3 newPos = new Vector3(-15.18f, -22.67f, 0);
        Instantiate(_keyToTheCastlePrefab, newPos, Quaternion.identity);
        AudioManager.Instance.PlayKeyAppearsSFX();
    }
}