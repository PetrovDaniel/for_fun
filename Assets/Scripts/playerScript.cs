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
    public Transform RespawnShot;


    void FixedUpdate()
    {
        //grounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, whatIsGround);
        move = Input.GetAxis("Horizontal");
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // стреляем
        if(Input.GetButton("Jump"))
        {
            Instantiate(Shot, RespawnShot);
        }
        // Прыгаем
        // /* забавно если раскоментировать*/ grounded = true;
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (grounded)
            {
                GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x, jumpForce);
            }
        }

        // Передвигаемся вдоль х

        GetComponent<Rigidbody2D>().velocity = new Vector2(move * maxSpeed, GetComponent<Rigidbody2D>().velocity.y);

        if (move > 0 && !facingRight)
            Flip();
        else if (move < 0 && facingRight)
            Flip();
         
        // выходим на esc 
        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }
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
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}
