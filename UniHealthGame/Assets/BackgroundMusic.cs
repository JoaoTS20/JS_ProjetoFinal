using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    [Header("Healthy Background")]
    [SerializeField] private AudioSource backgroundMusicHealthy;

    [SerializeField] private bool playingHealthy;

    [Header("Unhealthy Background")]
    [SerializeField] private AudioSource backgroundMusicUnhealthy;

    [SerializeField] private bool playingUnhealthy;

    private GameObject player;

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
