using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathScreen : MonoBehaviour
{
    public GameObject deathScreen;

    public GameObject player;

    private bool soundPlayed=false;


    [SerializeField] private AudioSource deathSoundEffect;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        deathScreen = GameObject.Find("PanelDeath");
        deathScreen.SetActive(false);
        Cursor.visible = false;

        player=GameObject.Find("Player");


    }

    // Update is called once per frame
    void Update()
    {
       if (player.GetComponent<PlayerHealthEnergy>().isDead())
        {
            
            if (!soundPlayed)
            {
                deathSoundEffect.Play();
                soundPlayed = true;
            }
            
            Time.timeScale = 0f;
            deathScreen.SetActive(true);
            Cursor.visible = true;
        }
    }


    public void restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
