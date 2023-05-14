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
        Vector3 sliderPos = transform.position;
        sliderPos.y = _collider.bounds.max.y + 0.5f;
        if (gameObject.tag != "Moss Giant")
            sliderPos.x -= 1;
        else
            sliderPos.x -= 0.3f;
        if (gameObject.tag == "Spider")
            sliderPos.y += 0.5f;
        Slider.transform.position = sliderPos;
       
        player = GameObject.Find("Player").GetComponent<Player>();
        Init();
    }

    public virtual void Init()
    {
        animator = GetComponentInChildren<Animator>();
        sprtieRenderer = GetComponentInChildren<SpriteRenderer>();
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

        if (!isHit)
        {
            transform.position = Vector3.MoveTowards(transform.position, currentDest, speed * Time.deltaTime);
        }

        float dist = Vector3.Distance(transform.position, player.transform.position);
        if (dist > 2)
        {
            isHit = false;
            animator.SetBool("InCombat", false);
        }

        Vector3 direction = player.transform.localPosition - transform.localPosition;
        if (direction.x < 0 && animator.GetBool("InCombat"))
            sprtieRenderer.flipX = true;
        else if (direction.x > 0 && animator.GetBool("InCombat"))
            sprtieRenderer.flipX = false;
    }
}
