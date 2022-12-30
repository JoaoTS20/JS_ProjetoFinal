using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    [SerializeField] private AudioSource backgroundMusicHealthy;

    private bool playingHealthy;

    [SerializeField] private AudioSource backgroundMusicUnhealthy;

    public GameObject player;

    private bool playingUnhealthy;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        backgroundMusicHealthy.Play();
        playingHealthy = true;
        playingUnhealthy = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!player.GetComponent<PlayerHealthEnergy>().isDead())
        {
            if (player.GetComponent<PlayerHealthEnergy>().isHealthy())
            {
                if (!playingHealthy)
                {
                    backgroundMusicUnhealthy.Stop();
                    backgroundMusicHealthy.Play();
                    playingHealthy = true;
                    playingUnhealthy = false;
                }
            }
            else
            {
                if (!playingUnhealthy)
                {
                    backgroundMusicHealthy.Stop();
                    backgroundMusicUnhealthy.Play();
                    playingHealthy = false;
                    playingUnhealthy = true;
                }

            }
        }
        else
        {
            backgroundMusicHealthy.Stop();
            backgroundMusicUnhealthy.Stop();
        }

    }
}
