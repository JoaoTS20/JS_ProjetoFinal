using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndLevelScreen : MonoBehaviour
{
    private GameObject endLevelScreen;

    private GameObject player;

    private TMP_Text pontuationTextTMP;
    private TMP_Text continueTextTMP;

    [SerializeField] private AudioSource endLevelEffect;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        endLevelScreen = GameObject.Find("PanelEndLevel");
        pontuationTextTMP= GameObject.Find("PontuationTextTMP").GetComponent<TMP_Text>();
        continueTextTMP=GameObject.Find("ContinueTextTMP").GetComponent<TMP_Text>();
        endLevelScreen.SetActive(false);

        player = GameObject.Find("Player");


    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag=="Player")
        {
            Time.timeScale = 0f;
            prepareEndLevelScreen();
            endLevelScreen.SetActive(true);
            Cursor.visible = true;
            endLevelEffect.Play();
        }
        //Destroy(this.gameObject);
    }

    private float calculateScore()
    {
        float energy = player.GetComponent<PlayerHealthEnergy>().getEnergy();
        float health = player.GetComponent<PlayerHealthEnergy>().getHealth();

        return ((health * 0.55f) + (energy * 0.45f))*0.2f;

    }

    public void prepareEndLevelScreen()
    {
        float score = calculateScore();

        if (score >= 9.5)
        {
            pontuationTextTMP.text = "Pontuation:\n" + score.ToString("N1") + "\n\n Success!";
            pontuationTextTMP.color = Color.green;
        }
        else
        {
            continueTextTMP.text = "Restart";
            pontuationTextTMP.text = "Pontuation:\n" + score.ToString("N1") + "\n\n Failed!";
            pontuationTextTMP.color = Color.red;

        }
    }

    public void nextLevel()
    {
        if (calculateScore() >= 9.5)
        {
            if (SceneManager.GetActiveScene().buildIndex + 1 >= SceneManager.sceneCountInBuildSettings - 1)
            {
                SceneManager.LoadScene(0);
            }
            else
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
        }
        else
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

    }
}
