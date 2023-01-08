using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitialTextDialogue : MonoBehaviour
{
    private GameObject initialDialogue;

    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        initialDialogue = GameObject.Find("InitialDialogue");
        Time.timeScale = 0f;
        Cursor.visible = true;

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKey)
        {
            Time.timeScale = 1f;
            Cursor.visible = false;
            Destroy(initialDialogue);
        }
    }
}
