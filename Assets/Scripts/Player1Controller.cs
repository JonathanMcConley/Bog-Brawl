using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player1Controller : MonoBehaviour
{
    private Rigidbody2D rigidBody;
    public bool isSinking;
    public float jumpHeight;
    public float speed;
    private float horizontalInput;
    public float minSinkingVelocity;
    public int jumps;
    private float jumpResistance;
    private float yDeathboxMin;
    private float yDeathboxMax;
    private float xDeathboxMin;
    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        isSinking = false;
        jumps = 2;
        jumpResistance = 1;
        yDeathboxMin = -15;
        xDeathboxMin = -20;
        yDeathboxMax = 30;
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
        }
        if (!isSinking)
        {
            jumpResistance = 1;
        }
        //Jump with X for P1, and Numpad 8 for P2
        if (Input.GetKeyDown(KeyCode.X) && jumps > 0)
        {
            rigidBody.AddForce(Vector2.up * jumpHeight * jumpResistance, ForceMode2D.Impulse);
            jumps -= 1;
        }
        //Block with Z for P1, and Numpad 7 for P2
        //Grab with C for P1, and Numpad 9 for P2
        //Light Attack with Spacebar for P1, and Numpad 4 for P2
        //Heavy Attack with V for P1, and Numpad 5 for P2
        //Charge Attack while holding B for P1, and Numpad 6 for P2, release to use attack
        //Move Left/ Right with A and D for P1, and Left and Right Arrowkeys for P2
        if (Input.GetKey(KeyCode.A))
        {
            transform.Translate(Vector3.left * speed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.Translate(Vector3.right * speed * Time.deltaTime);
        }
        //Tilt Up with W for P1, and Up Arrowkey for P2
        //Crouch/ Tilt Down with S for P1, and Down Arrowkey for P2
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
}
