using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManageGame : MonoBehaviour
{
    public Player1Controller player1;
    public Player2Controller player2;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape)) 
        {
            Application.Quit();   
        }
        if(player1.getLivesLeft() == 0 || player2.getLivesLeft() == 0) 
        {
            StartCoroutine("WinCoroutine");
        }
    }
    IEnumerator WinCoroutine() 
    {
        yield return new WaitForSeconds(5);
        Application.Quit();
    }
}
