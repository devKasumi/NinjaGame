using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class Player : Character
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpForce = 350f;
    [SerializeField] private Kunai kunaiPrefab;
    [SerializeField] private Transform throwPoint;
    [SerializeField] private GameObject attackArea;
    [SerializeField] private GameObject specialAttackArea;
    [SerializeField] private GameObject specialAttackButtonImage;
    [SerializeField] private float dashingPower = 350f;

    private GameObject[] buttonArray;
    

    private bool isGrounded = true;
    private bool isjumping = false;
    private bool isAttack = false;
    //private bool isDeath = false;

    private float horizontal;

    private int coin = 0;

    private Vector3 savePoint;

    private bool isDashing = false;
    private float dashingCooldown = 2f;
    private float dashingTime = 0.7f;
    private float facingDirection = 1f;

    private float countdownSpecialAttack = 5f;

    private float timer = 0f;

    private void Awake()
    {
        coin = PlayerPrefs.GetInt("coin", 0);
        buttonArray = GameObject.FindGameObjectsWithTag("Button");
    }

    // Update is called once per frame
    void Update()
    {
        if (IsDead || isDashing) return;

        CountDownSpecialAttack();

        isGrounded = CheckGrounded();

        // -1 -> 0 -> 1
        //horizontal = Input.GetAxisRaw("Horizontal");


        if (isAttack)
        {
            rb.velocity = Vector2.zero;
            return;
        }

        if (isGrounded)
        {
            if (isjumping) return;

            //change anim run
            if (Mathf.Abs(horizontal) > 0.1f)
            {
                ChangeAnimation("run");
            }
        }

        //check falling
        if (!isGrounded && rb.velocity.y < 0)
        {
            ChangeAnimation("fall");
            isjumping = false;
        }

        // Moving
        if (Mathf.Abs(horizontal) > 0.1f)
        {
            rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
            transform.rotation = Quaternion.Euler(new Vector3(0f, horizontal > 0 ? 0 : 180,0f));   
        }
        //idle
        else if (isGrounded)
        {
            ChangeAnimation("idle");
            rb.velocity = Vector2.zero;
        }

    }

    public override void OnInit()
    {
        base.OnInit();

        isAttack = false;

        transform.position = savePoint;
        ChangeAnimation("idle");
        DeActiveAttack();
        DeActiveSpecialAttack();

        SavePoint();
        UIManager.GetInstance.SetCoin(coin);
        specialAttackButtonImage.GetComponent<Image>().fillAmount = 1f;
    }

    public override void OnDespawn()
    {
        base.OnDespawn();
        OnInit();
    }

    protected override void OnDeath()
    {
        base.OnDeath();   
    }

    bool CheckGrounded()
    {
        Debug.DrawLine(transform.position, transform.position + Vector3.down * 1.1f, Color.red);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1.1f, groundLayer); 
        return hit.collider != null;
    }

    public void Attack()
    {
        ChangeAnimation("attack");
        isAttack = true;
        Invoke(nameof(ResetAttack), 0.5f);
        ActiveAttack();
        Invoke(nameof(DeActiveAttack), 0.5f);
    }

    public void SpecialAttack()
    {
        if (specialAttackButtonImage.GetComponent<Image>().fillAmount == 1f)
        {
            ChangeAnimation("specialAttack");
            isAttack = true;
            specialAttackButtonImage.GetComponent<Image>().fillAmount = 0f;
            Invoke(nameof(ResetAttack), 0.7f);
            ActiveSpecialAttack();
            Invoke(nameof(DeActiveSpecialAttack), 0.7f);
        }
    }

    public void Throw()
    {
        ChangeAnimation("throw");
        isAttack = true;
        Invoke(nameof(ResetAttack), 0.5f);

        Instantiate(kunaiPrefab, throwPoint.position, throwPoint.rotation);
    }

    void ResetAttack()
    {
        isAttack = false;
        ChangeAnimation("idle");
    }

    public void Jump()
    {
        isjumping = true;
        ChangeAnimation("jump");
        rb.AddForce(jumpForce * Vector2.up);
    }

    public void Slide()
    {
        isDashing = true;
        ChangeAnimation("slide");
        rb.AddForce(dashingPower * Vector2.right * facingDirection);
        DisableButton();
        StartCoroutine(Dash());
        EnableButton();
    }

    internal void SavePoint()
    {
        savePoint = transform.position;
    }

    private void ActiveAttack()
    {
        attackArea.SetActive(true);
    }

    private void ActiveSpecialAttack()
    {
        specialAttackArea.SetActive(true);
    }

    private void DeActiveAttack()
    {
        attackArea.SetActive(false);
    }

    private void DeActiveSpecialAttack()
    {
        specialAttackArea.SetActive(false);
    }

    private void CountDownSpecialAttack()
    {
        if (specialAttackButtonImage.GetComponent<Image>().fillAmount < 1f)
        {
            timer += Time.deltaTime;
            specialAttackButtonImage.GetComponent<Image>().fillAmount = timer / countdownSpecialAttack;
            if (specialAttackButtonImage.GetComponent<Image>().fillAmount == 1f)
            {
                timer = 0f;
            }
            //Debug.LogError(timer + "   " + specialAttackButtonImage.GetComponent<Image>().fillAmount);
        }
    }

    public void SetMove(float horizontal)
    {
        this.horizontal = horizontal;
        if (horizontal != 0)
        {
            facingDirection = horizontal;
        }
    }

    private void EnableButton()
    {
        for (int i = 0; i < buttonArray.Length; i++)
        {
            buttonArray[i].GetComponent<Image>().raycastTarget = true;
        }
    }

    private void DisableButton()
    {
        for (int i = 0; i < buttonArray.Length; i++)
        {
            buttonArray[i].GetComponent<Image>().raycastTarget = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Coin")
        {
            coin++;
            PlayerPrefs.SetInt("coin", coin);
            UIManager.GetInstance.SetCoin(coin);
            Destroy(collision.gameObject);
        }
        if (collision.tag == "DeathZone")
        {
            rb.gravityScale = 0f;
            rb.velocity = Vector2.zero;
            ChangeAnimation("die");

            Invoke(nameof(OnInit), 1f);
        }
    }

    IEnumerator Dash()
    {
        yield return new WaitForSeconds(dashingTime);
        isDashing = false;
        yield return new WaitForSeconds(dashingCooldown);
        ChangeAnimation("idle");
    }
}
