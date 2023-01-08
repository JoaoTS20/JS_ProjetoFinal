using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemTextDialogue : MonoBehaviour
{
    private GameObject itemDialogue;
    private bool firstItem = false;

    // Start is called before the first frame update
    void Start()
    {
        itemDialogue = GameObject.Find("ItemDialogue");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKey && firstItem)
        {
            Time.timeScale = 1f;
            Cursor.visible = false;
            Destroy(itemDialogue);
        }
    }

    public void activateTextDialogue()
    {
        itemDialogue.SetActive(true);
        firstItem = true;
        Time.timeScale = 0f;
        Cursor.visible = true;
    }

    public void makeInactive()
    {
        itemDialogue.SetActive(false);

    }
}

