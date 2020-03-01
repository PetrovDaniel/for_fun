using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : Unit
{
    [SerializeField]
    private float maxSpeed = 10f;
    [SerializeField]
    private float jumpForce = 700f;

    private bool grounded = true;
    private new Rigidbody2D rigidbody;
    public Bullet bullet;
    private Animator anim;
    private SpriteRenderer sprite;

    private CharacterState State
    {
        get { return (CharacterState)anim.GetInteger("State"); }
        set { anim.SetInteger("State", (int)value); }
    }

    void FixedUpdate()
    {
        CheckGround();
    }

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        //bullet = Resources.Load<Bullet>("Bullet");
    }

    // Update is called once per frame
    private void Update()
    {
        State = CharacterState.Idle;

        // стреляем
        if (Input.GetButtonDown("Jump"))
            AttackMove();

        // Прыгаем
        if (grounded && (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)))
            JumpMove();

        // Передвигаемся вдоль х
        if(Input.GetButton("Horizontal"))
            RunMove();

        // выходим на esc 
        Exit();
    }

    private void AttackMove()
    {
        State = CharacterState.Attack;
        float direction = 1.0f;
        direction = (sprite.flipX ? -1.0f : 1.0f);
        Vector3 position = transform.position;
        position.y += 9.65f;
        position.x += 5.72f * direction;
        Bullet newBullet = Instantiate(bullet, position, bullet.transform.rotation) as Bullet;
        newBullet.Direction = newBullet.transform.right * direction;
    }
    private void JumpMove()
    {   
        // передаем телу импульс и запускаем анимацию
        rigidbody.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
        State = CharacterState.Jump;
    }
    private void RunMove()
    {
        // получаем направление движения, умножаем на единичный вектор перемещаем на данный вектор
        Vector3 direction = transform.right * Input.GetAxis("Horizontal");
        transform.position = Vector3.MoveTowards(transform.position, transform.position + direction, maxSpeed * Time.deltaTime);
        // теперь вращаем только спрайт
        sprite.flipX = direction.x < 0.0f;
        if (grounded)
            State = CharacterState.Run;
    }
    private void CheckGround()
    {
        // получаем массив пересекаемых колайдеров и если кто-то еще есть, то значит мы на этом стоим
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 3f);
        grounded = colliders.Length > 1;
    }
    private void Exit()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
}

// состояния персонажа
public enum CharacterState
{
    Idle,
    Run,
    Jump,
    Attack
}