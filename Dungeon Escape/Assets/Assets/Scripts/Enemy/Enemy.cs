using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Enemy : MonoBehaviour
{
    [SerializeField]
    protected int health;
    [SerializeField]
    protected int maxHealth;
    [SerializeField]
    protected float speed;
    [SerializeField]
    protected int gems;
    [SerializeField]
    protected Transform pointA, pointB;
    [SerializeField]
    protected GameObject _diamondPrefab;
    private Vector3 currentDest;
    [SerializeField]
    protected Animator animator;
    protected SpriteRenderer sprtieRenderer;
    protected bool isHit = false;
    protected Player player;
    protected bool isDead = false;
    public Slider Slider { get; set; }
    [SerializeField]
    protected BoxCollider2D _collider;
    protected float _currentVelocity = 0;

    private void Awake()
    {
        Slider = GetComponentInChildren<Slider>();
    }

    private void Start()
    {
        maxHealth = health;
        _collider = GetComponent<BoxCollider2D>();
        SetHealthBarPosition();

        player = GameObject.Find("Player").GetComponent<Player>();
        Init();
    }

    public virtual void Init()
    {
        animator = GetComponentInChildren<Animator>();
        sprtieRenderer = GetComponentInChildren<SpriteRenderer>();
        if(pointB != null)
            currentDest = pointB.position;
    }

    public virtual void Update()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Idle")
            && animator.GetBool("InCombat") == false)
        {
            return;
        } 

        if(!isDead)
            Movement();
    }

    public virtual void Movement()
    {
        if (currentDest == pointB.position)
            sprtieRenderer.flipX = false;
        else if (currentDest == pointA.position)
            sprtieRenderer.flipX = true;

        if (transform.position == pointA.position)
        {
            animator.SetTrigger("Idle");
            currentDest = pointB.position;
        }
        else if (transform.position == pointB.position)
        {
            animator.SetTrigger("Idle");
            currentDest = pointA.position;
        }

        float clostDist = gameObject.tag == "Moss Giant" ? 1.7f : 1.1f;
        float dist = Vector3.Distance(transform.position, player.transform.position);
        bool playerClose = dist <= clostDist;
        if(playerClose)
        {
            animator.SetBool("Player_Close", true);
            animator.SetBool("InCombat", true);
        }
        else
        {
            animator.SetBool("Player_Close", false);
        }

        float playerDistFromPointA = Vector3.Distance(player.transform.position, pointA.position);
        float playerDistFromPointB = Vector3.Distance(player.transform.position, pointB.position);

        if ((player.transform.position.x >= pointA.position.x || playerDistFromPointA < 1) && (player.transform.position.x <= pointB.position.x || playerDistFromPointB < 1) || playerClose)
        {
            animator.SetBool("InCombat", true);
        }
        else
        {
            isHit = false;
            animator.SetBool("InCombat", false);
        }

        Vector3 direction = player.transform.localPosition - transform.localPosition;
        if (direction.x < 0 && animator.GetBool("InCombat"))
            sprtieRenderer.flipX = true;
        else if (direction.x > 0 && animator.GetBool("InCombat"))
            sprtieRenderer.flipX = false;

        bool isIdleAnimationPlaying = animator.GetCurrentAnimatorStateInfo(0).IsName("Idle");
        if (!isIdleAnimationPlaying)
        {
            if (!animator.GetBool("InCombat"))
            {
                transform.position = Vector3.MoveTowards(transform.position, currentDest, speed * Time.deltaTime);
            }
            else if (!playerClose)
            {
                Vector3 playerPos = player.transform.position;
                playerPos.y = transform.position.y;
                transform.position = Vector3.MoveTowards(transform.position, playerPos, speed * Time.deltaTime);
            }
        }
    }

    private void SetHealthBarPosition()
    {
        if (gameObject.tag == "Spider")
            return;

        Vector3 sliderPos = transform.position;
        sliderPos.y = _collider.bounds.max.y + 0.5f;
        if (gameObject.tag != "Moss Giant")
            sliderPos.x -= 1;
        else
            sliderPos.x -= 0.3f;
       
        Slider.transform.position = sliderPos;
    }
}
