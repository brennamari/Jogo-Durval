using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed;
    public float jumpForce;

    private bool isFire;
    private bool isJumping;

    public AudioSource walkSound;
    public AudioSource jumpSound;
    public AudioSource attackSound;

    private Animator anim;
    private Rigidbody2D rig;

    void Start()
    {
        anim = GetComponent<Animator>();
        rig = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        Move();
        Jump();
        Attack();
    }

    void Move()
    {
        float movement = Input.GetAxis("Horizontal");
        rig.velocity = new Vector2(movement * speed, rig.velocity.y);

        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            walkSound.Play();
        }

        if (movement > 0)
        {
            if (!isJumping)
            {
                anim.SetInteger("transition", 1);
            }
            transform.eulerAngles = new Vector3(0, 0, 0);
        }
        else if (movement < 0)
        {
            if (!isJumping)
            {
                anim.SetInteger("transition", 1);
            }
            transform.eulerAngles = new Vector3(0, 180, 0);
        }
        else if (movement == 0 && !isJumping && !isFire)
        {
            walkSound.Stop();
            anim.SetInteger("transition", 0);
        }
    }

    void Jump()
    {
        if (Input.GetButtonDown("Jump") && !isJumping )
        {
            jumpSound.Play();
            anim.SetInteger("transition", 2);
            rig.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
            isJumping = true;
            walkSound.Stop();
        }
    }

     void Attack()
     {
         if (Input.GetKeyDown(KeyCode.X) && !isFire)
         {
             StartCoroutine(FireCoroutine());
         }
     }

     IEnumerator FireCoroutine()
     {
         attackSound.Play();
         isFire = true;
         anim.SetInteger("transition", 3);
         yield return new WaitForSeconds(2f);
         anim.SetInteger("transition", 0);
         isFire = false;
         walkSound.Stop();
     }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.layer == 3)
        {
            if (isJumping)
            {
                isJumping = false;
                anim.SetInteger("transition", 0);
            }
        }
    }
}