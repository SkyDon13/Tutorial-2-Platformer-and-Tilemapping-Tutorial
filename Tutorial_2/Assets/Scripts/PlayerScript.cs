using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{

    private Rigidbody2D rd2d;
    public float speed;
    public Text score;
    private int scoreValue = 0;
    private int livesValue = 3;
    public Text lives;
    public Text win;
    public Text lose;
    private bool isonGround;
    public Transform groundcheck;
    public float checkRadius;
    public LayerMask allGround;
    public float jumpForce;
    public bool facingRight = true;
    public AudioClip musicClipOne;
    public AudioClip musicClipTwo;
    public AudioSource musicSource;
    Animator anim;
    private bool isJumping;

    // Start is called before the first frame update
    void Start()
    {
        rd2d = GetComponent<Rigidbody2D>() ;
        score.text = scoreValue.ToString() ;
        lives.text = livesValue.ToString() ;
        win.text = "";
        lose.text = "";
        musicSource.clip = musicClipOne;
        musicSource.Play();
        musicSource.loop = true;
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float hozMovement = Input.GetAxis("Horizontal");
        float verMovement = Input.GetAxis("Vertical");

        rd2d.AddForce(new Vector2(hozMovement * speed, verMovement * speed));

        isonGround = Physics2D.OverlapCircle(groundcheck.position, checkRadius, allGround);


        if (facingRight == false && hozMovement > 0)
        {
            Flip();
        }

        else if (facingRight == true && hozMovement < 0)
        {
            Flip();
        }
        
        if (hozMovement > 0 && facingRight == true)
        {
            Debug.Log("Facing Right");

        }
        
        if (hozMovement < 0 && facingRight == false)
        {
            Debug.Log("Facing Left");
        }

        if (verMovement > 0 && isonGround == false)
        {
            Debug.Log("Jumping");
        }

        else if (verMovement == 0 && isonGround == true)
        {
            Debug.Log("Not Jumping");
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Coin")
        {
            scoreValue += 1;
            score.text = scoreValue.ToString();
            Destroy(collision.collider.gameObject);

            if (scoreValue == 4)
            {
                transform.position = new Vector3(55f, 0.0f, 0.0f);
                livesValue = 3;
                lives.text = livesValue.ToString();
            }

            if (scoreValue >= 8)
            {
                musicSource.loop = false;
                win.text = "You Win! Game by Skyler Donovan.";
                musicSource.clip = musicClipTwo;
                musicSource.Play();
            }
        }
        
        else if (collision.collider.tag == "Enemy")
        {
            livesValue -= 1;
            lives.text = livesValue.ToString();
            Destroy(collision.collider.gameObject);
            
            if (livesValue <= 0)
            {
                Destroy(gameObject);
                lose.text = "You Lose!";
            }

        }

    }

   private void OnCollisionStay2D(Collision2D collision)
   {
        if (collision.collider.tag == "Ground" && isonGround)
        {
            if (Input.GetKey(KeyCode.W))
            {
                rd2d.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse) ;
                
                anim.SetInteger("State", 0);
            }
        }
   }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode. A))
        {
            anim.SetInteger("State", 0);
        }

        if(Input.GetKeyUp(KeyCode. D))
        {
            anim.SetInteger("State", 0);
        }

        if (Input.GetKeyDown(KeyCode. A))
        {
            anim.SetInteger("State", 1);
        }
        
        if (Input.GetKeyDown(KeyCode. D))
        {
            anim.SetInteger("State", 1);
        }

        if (Input.GetKeyUp(KeyCode.W))
        {
            anim.SetInteger("State", 0);
        }

        if (Input.GetKeyDown(KeyCode. W))
        {
            anim.SetInteger("State", 2);
        }

        if (Input.GetKey("escape"))
        {
            Application.Quit();
        }
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector2 Scaler = transform.localScale;
        Scaler.x = Scaler.x * -1;
        transform.localScale = Scaler;
    }
}
