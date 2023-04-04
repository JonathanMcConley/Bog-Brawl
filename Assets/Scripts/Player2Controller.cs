using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player2Controller : MonoBehaviour
{
    private Rigidbody2D rigidBody;
    private Animator animatorController;
    public Transform punchAttackPoint;
    public Transform kickAttackPoint;
    public float punchAttackRange;
    public float kickAttackRange;
    private bool isSinking;
    private float jumpHeight;
    private float speed;
    private float minSinkingVelocity;
    private int jumps;
    private float jumpResistance;
    private float yDeathboxMin;
    private float yDeathboxMax;
    private float xDeathboxMin;
    private float punchTime;
    private float kickTime;
    public float hitRecoveryTime;
    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        animatorController = GetComponent<Animator>();
        isSinking = false;
        jumps = 2;
        jumpResistance = 1;
        yDeathboxMin = -15;
        xDeathboxMin = -20;
        yDeathboxMax = 30;
        speed = 10;
        jumpHeight = 10;
        minSinkingVelocity = -0.1f;
        punchTime = 0.01f;
        kickTime = 0.3325f;
    }

    // Update is called once per frame
    void Update()
    {
        //Resets player position if they go too far off the map
        if (gameObject.transform.position.y < yDeathboxMin || gameObject.transform.position.x < xDeathboxMin || gameObject.transform.position.x > -xDeathboxMin || gameObject.transform.position.y > yDeathboxMax)
        {
            rigidBody.velocity = Vector2.zero;
            gameObject.transform.position = new Vector3(0, 0, -0.01f);
        }
        //If the player is sinking, their gravity is 0.001% of Normal Gravity
        if (isSinking)
        {
            jumpResistance = 0.625f;
            if (rigidBody.velocity.y < minSinkingVelocity)
            {
                rigidBody.velocity = new Vector2(rigidBody.velocity.x, minSinkingVelocity);
            }
            animatorController.SetBool("IsJumping", false);
        }
        if (!isSinking)
        {
            jumpResistance = 1;
            animatorController.SetBool("IsJumping", true);
        }
        //Jump with X for P1, and Numpad 8 for P2
        if (Input.GetKeyDown(KeyCode.Keypad8) && jumps > 0 && !animatorController.GetBool("GotHit"))
        {
            rigidBody.AddForce(Vector2.up * jumpHeight * jumpResistance, ForceMode2D.Impulse);
            jumps -= 1;
        }
        //Light Attack with Spacebar for P1, and Numpad 4 for P2
        if (Input.GetKeyDown(KeyCode.Keypad4) && !animatorController.GetBool("GotHit"))
        {
            StartCoroutine("PunchCoroutine");
        }
        //Heavy Attack with V for P1, and Numpad 5 for P2
        if (Input.GetKeyDown(KeyCode.Keypad5) && !animatorController.GetBool("GotHit"))
        {
            StartCoroutine("KickCoroutine");
        }
        //Move Left/ Right with A and D for P1, and Left and Right Arrowkeys for P2
        if (Input.GetKey(KeyCode.LeftArrow) && !animatorController.GetBool("GotHit"))
        {
            if (isSinking)
            {
                animatorController.SetBool("IsWalking", true);
            }
            else
            {
                animatorController.SetBool("IsWalking", false);
            }
            gameObject.transform.rotation = new Quaternion(0, 0, 0, 0);
            transform.Translate(Vector3.left * speed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.RightArrow) && !animatorController.GetBool("GotHit"))
        {
            if (isSinking)
            {
                animatorController.SetBool("IsWalking", true);
            }
            else
            {
                animatorController.SetBool("IsWalking", false);
            }
            gameObject.transform.rotation = new Quaternion(0, 180, 0, 0);
            transform.Translate(Vector3.left * speed * Time.deltaTime);
        }
        if (!Input.GetKey(KeyCode.RightArrow) && !Input.GetKey(KeyCode.LeftArrow))
        {
            animatorController.SetBool("IsWalking", false);
        }
    }
    //Allows Player to jump while they're touching ground, reduces gravity while touching ground
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))
        {
            isSinking = true;
            jumps = 2;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))
        {
            isSinking = false;
        }
    }

    //Coroutine for Punching
    private IEnumerator PunchCoroutine()
    {
        animatorController.SetBool("IsPunching", true);
        //Collision detection by Brackeys
        Collider2D[] hitPlayers = Physics2D.OverlapCircleAll(punchAttackPoint.position, punchAttackRange);
        foreach (Collider2D hitPlayer in hitPlayers)
        {
            if (hitPlayer.CompareTag("Player1"))
            {
                //Do damage
                //Assign knockback based on damage
                //Call their RecoveryFromHitCoroutine
            }
        }
        yield return new WaitForSeconds(punchTime);
        animatorController.SetBool("IsPunching", false);
    }
    //Coroutine for Kicking
    private IEnumerator KickCoroutine()
    {
        animatorController.SetBool("IsKicking", true);
        //Collision detection by Brackeys
        Collider2D[] hitPlayers = Physics2D.OverlapCircleAll(punchAttackPoint.position, punchAttackRange);
        foreach (Collider2D hitPlayer in hitPlayers)
        {
            if (hitPlayer.CompareTag("Player1"))
            {
                //Do damage
                //Assign knockback based on damage
                //Call their RecoveryFromHitCoroutine
            }
        }
        yield return new WaitForSeconds(kickTime);
        animatorController.SetBool("IsKicking", false);
    }
    //Coroutine for Getting Hit
    public IEnumerator RecoveryFromHitCoroutine()
    {
        animatorController.SetBool("GotHit", true);
        yield return new WaitForSeconds(hitRecoveryTime);
        animatorController.SetBool("GotHit", false);
    }
}
