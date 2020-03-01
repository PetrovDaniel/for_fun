using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerScript : MonoBehaviour
{
    public float maxSpeed = 10f;
    public float jumpForce = 700f;
    bool grounded = true;
    float move = 0f;
    bool facingRight = true;
    public GameObject Shot;
    public GameObject RespawnShot;
    Rigidbody2D player;
    GameObject bullet;
    public float bulletForce = 50f;
    Animator anim;


    private playerState State
    {
        get { return (playerState)anim.GetInteger("State"); }
        set { anim.SetInteger("State", (int)value); }
    }



    void FixedUpdate()
    {
        move = Input.GetAxis("Horizontal");
    }

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        State = playerState.Idle;

        // стреляем
        AttackMove();

        // Прыгаем
        JumpMove();

        // Передвигаемся вдоль х
        RunMove();

        // выходим на esc 
        Exit();
    }


    void OnTriggerEnter2D()
    {
        grounded = true;
    }

    void OnTriggerExit2D()
    {
        grounded = false;
    }

    void Flip()
    {
        facingRight = !facingRight;
        player.transform.rotation = Quaternion.Euler(
        transform.rotation.eulerAngles.x,
        transform.rotation.eulerAngles.y + 180f,
        transform.rotation.eulerAngles.z);
    }
    void AttackMove()
    {
        if (Input.GetButtonDown("Jump"))
        {
            State = playerState.Attack;
            bullet = Instantiate(Shot, RespawnShot.transform.position, Quaternion.identity);

            // кривое решение, но работает!!
            if (facingRight)
                bullet.GetComponent<Rigidbody2D>().AddForce(Vector2.right * bulletForce);
            else
                bullet.GetComponent<Rigidbody2D>().AddForce(Vector2.left * bulletForce);

            Destroy(bullet, 1);
        }
    }
    void JumpMove()
    {
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (grounded)
            {
                player.velocity = new Vector2(player.velocity.x, jumpForce);
                State = playerState.Jump;
            }
        }
    }
    void RunMove()
    {
        player.velocity = new Vector2(move * maxSpeed, player.velocity.y);
        if (move != 0 && grounded)
            State = playerState.Run;

        if (move > 0 && !facingRight)
            Flip();
        else if (move < 0 && facingRight)
            Flip();
    }
    void Exit()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
}

// состояния персонажа
public enum playerState 
{
    Idle,
    Run,
    Jump,
    Attack
}