using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Music : MonoBehaviour
{

    [SerializeField]
    private GameObject _musicOn;
    [SerializeField]
    private GameObject _musicOff;


    void Start()
    {
        if (PlayerPrefs.GetInt("music") == 1) {
            MusicOn();
        } else {
            MusicOff();
        }
    }


    public void MusicOn()
    {
        GetComponent<AudioSource>().Play();
        PlayerPrefs.SetInt("music", 1);
        _musicOff.SetActive(true);
        _musicOn.SetActive(false);
    }

    public void MusicOff()
    {
        GetComponent<AudioSource>().Stop();
        PlayerPrefs.SetInt("music", 0);
        _musicOn.SetActive(true);
        _musicOff.SetActive(false);

    }

}
