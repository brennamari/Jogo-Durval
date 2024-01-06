using System.Collections;
using UnityEngine;

public class player : MonoBehaviour
{
    public float speed;
    public float jumpForce;
    public int maxJumps = 2;  // Número máximo de pulos

    private int jumpsLeft;
    private bool isJumping;
    private Animator anim;
    private Rigidbody2D rig;

    private bool isAttacking;

    void Start()
    {
        anim = GetComponent<Animator>();
        rig = GetComponent<Rigidbody2D>();
        jumpsLeft = maxJumps;
    }

    void Update()
    {
        Move();
        Jump();

        if (Input.GetKeyDown(KeyCode.F) && !isJumping && !isAttacking)
        {
            StartCoroutine(AttackCoroutine());
        }
    }

    void Move()
    {
        float movement = Input.GetAxis("Horizontal");
        rig.velocity = new Vector2(movement * speed, rig.velocity.y);

        if (movement > 0)
        {
            if (!isJumping && !isAttacking)
            {
                anim.SetInteger("transition", 1);
            }
            transform.eulerAngles = new Vector3(0, 0, 0);
        }
        else if (movement < 0)
        {
            if (!isJumping && !isAttacking)
            {
                anim.SetInteger("transition", 1);
            }
            transform.eulerAngles = new Vector3(0, 180, 0);
        }
        else if (movement == 0 && !isJumping && !isAttacking)
        {
            anim.SetInteger("transition", 0);
        }
    }

    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && jumpsLeft > 0 && !isJumping && !isAttacking)
        {
            rig.velocity = new Vector2(rig.velocity.x, 0);  // Zera a velocidade vertical antes do pulo
            rig.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
            jumpsLeft--;
            isJumping = true;
            anim.SetInteger("transition", 2);
        }
    }

    IEnumerator AttackCoroutine()
    {
        anim.SetInteger("transition", 4);
        isAttacking = true;

        // Adicione aqui qualquer lógica adicional para o ataque
        // Por exemplo, espera por um tempo para a animação de ataque terminar
        yield return new WaitForSeconds(1f);

        isAttacking = false;
        anim.SetInteger("transition", 0);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 3)
        {
            isJumping = false;
            if (rig.velocity.y <= 0.1)  // Certifique-se de que o jogador está descendo antes de mudar para a animação de caminhada
            {
                anim.SetInteger("transition", 0); // Muda para a transição de idle quando tocar no chão
                jumpsLeft = maxJumps;  // Restaura os pulos quando tocar no chão
            }
        }
    }
}