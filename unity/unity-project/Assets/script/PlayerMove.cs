using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum PlayerState { IDLE, RUN, JUMP, ATTACK, DEAD }

public class PlayerMove : MonoBehaviour {

    [SerializeField]
    private float maxSpeed = 20f;
    [SerializeField]
    private float minSpeed = 0.1f;
    [SerializeField]
    private float jumpForce = 2000f;
    [SerializeField]
    private int playerNumber = 1;
    [SerializeField]
    private float attackDuration = 0.25f;
    [SerializeField]
    private int playerLife = 3;

    private bool facingRight = true;
    private bool grounded = false;
    private PlayerState state = PlayerState.IDLE;
    private float attackTimer = 0;

    private Transform top;
    private Transform bottom;
    private int groundMask;
    private Rigidbody2D body;
    private Animator animator;
    private ParticleSystem particle;

    public int PlayerLife
    {
        get
        {
            return playerLife;
        }
    }

    void Start () {
        top = transform.Find("top");
        bottom = transform.Find("bottom");
        groundMask = 1 << LayerMask.NameToLayer("Ground");
        body = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        particle = GetComponentInChildren<ParticleSystem>();
	}
	
	void FixedUpdate () {
        if (state != PlayerState.DEAD)
        {
            float hInput = GetAxis("Horizontal");
            float hSpeed = hInput * maxSpeed;

            body.velocity = new Vector2(hSpeed, body.velocity.y);

            grounded = Physics2D.Linecast(top.position, bottom.position, groundMask);

            if (state == PlayerState.ATTACK && attackTimer <= Time.realtimeSinceStartup)
            {
                setState(PlayerState.IDLE);
            }

            if (GetButtonDown("Fire"))
            {
                setState(PlayerState.ATTACK);
                attackTimer = Time.realtimeSinceStartup + attackDuration;
            }

            if (grounded && state != PlayerState.ATTACK)
            {
                if (GetButtonDown("Jump"))
                {
                    body.AddForce(Vector2.up * jumpForce);
                    setState(PlayerState.JUMP);
                }
                else if (Mathf.Abs(body.velocity.x) > minSpeed)
                {
                    setState(PlayerState.RUN);
                }
                else
                {
                    setState(PlayerState.IDLE);
                }
            }

            if (hInput < 0 && facingRight || hInput > 0 && !facingRight)
            {
                flip();
            }
        }
    }

    void setState(PlayerState state)
    {
        var trigger = state.ToString().ToLowerInvariant();
        if (state != this.state)
        {
            this.state = state;
            animator.SetTrigger(trigger); 
        }
        BroadcastMessage("PlaySound", trigger);
    }

    void flip()
    {
        var scale = transform.localScale;
        scale.x = -scale.x;
        transform.localScale = scale;
        facingRight = !facingRight;
    }

    public void hit()
    {
        if (state != PlayerState.DEAD)
        {
            playerLife--;
            if (playerLife <= 0)
            {
                setState(PlayerState.DEAD);
            }
            else
            {
                BroadcastMessage("PlaySound", "hit");
            }
            var main = particle.main;
            main.loop = false;
            particle.Play();
        }
    }

    public void hide()
    {
        GetComponent<SpriteRenderer>().enabled = false;
        StartCoroutine(RestartLevel());
    }

    private IEnumerator RestartLevel()
    {
        yield return new WaitForSeconds(3.0f);  // or however long you want it to wait
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    float GetAxis(string axisName)
    {
        return Input.GetAxis(axisName + playerNumber);
    }

    bool GetButton(string buttonName)
    {
        return Input.GetButton(buttonName + playerNumber);
    }

    bool GetButtonDown(string buttonName)
    {
        return Input.GetButtonDown(buttonName + playerNumber);
    }
}
