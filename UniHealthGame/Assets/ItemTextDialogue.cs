using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemTextDialogue : MonoBehaviour
{
    private GameObject panelDialogue;
    private GameObject itemDialogue;
    private bool firstItem = false;
    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        panelDialogue = GameObject.Find("PanelDialogue2");
        itemDialogue = GameObject.Find("ItemDialogue");
        player = GameObject.Find("Player");
        panelDialogue.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (!panelDialogue.activeSelf && player.GetComponent<PlayerHealthEnergy>().IsfirstItemCollected())
        {
            panelDialogue.SetActive(true);
            Time.timeScale = 0f;
            Cursor.visible = true;
        }

        if (Input.anyKey && player.GetComponent<PlayerHealthEnergy>().IsfirstItemCollected() && panelDialogue.activeSelf)
        {
            Time.timeScale = 1f;
            Cursor.visible = false;
            Destroy(itemDialogue);
        }
    }


}

