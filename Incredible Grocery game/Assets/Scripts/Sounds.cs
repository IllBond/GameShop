using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sounds : MonoBehaviour
{

    public AudioClip unionShow;
    public AudioClip unionFade;
    public AudioClip clickBotton;
    public AudioClip addMoney;
    public AudioClip checkProcut;


    [SerializeField]
    private GameObject _soundOn;
    [SerializeField]
    private GameObject _soundOff;

    void Update()
    {
        if (Input.GetMouseButtonDown(2))
        {
            PlayerPrefs.SetInt("firstStart", 0);
            PlayerPrefs.SetInt("music", 0);
            PlayerPrefs.SetInt("sound", 0);
        }

    }



    public void SoundOn()
    {
        PlayerPrefs.SetInt("sound", 1);
        _soundOff.SetActive(true);
        _soundOn.SetActive(false);
    }

    public void SoundOff()
    {
        PlayerPrefs.SetInt("sound", 0);
        _soundOn.SetActive(true);
        _soundOff.SetActive(false);
    }


    public void UnionShow()
    {
        PlaySound(unionShow);  
    }

    public void UnionFade()
    {
        PlaySound(unionFade);
    }

    public void ClickBotton()
    {
        PlaySound(clickBotton);
    }

    public void AddMoney()
    {
        PlaySound(addMoney);
    }

    public void CheckProcut()
    {
        PlaySound(checkProcut);
    }


    public void PlaySound(AudioClip sound)
    {
        if (PlayerPrefs.GetInt("sound") == 1)
        {
            GetComponent<AudioSource>().PlayOneShot(sound);
        }
    }
}
