using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Setting : MonoBehaviour
{
    [SerializeField]
    private GameObject _pause;
    [SerializeField]
    private GameObject _button; 

    [SerializeField]
    private GameObject _soundOn; 
    [SerializeField]
    private GameObject _soundOff; 
    [SerializeField]
    private GameObject _musicOn; 
    [SerializeField]
    private GameObject _musicOff;

    void Start() {

        if (PlayerPrefs.GetInt("sound") == 1)
        {
            _soundOff.SetActive(true);
            _soundOn.SetActive(false);
        }
        else
        {
            _soundOn.SetActive(true);
            _soundOff.SetActive(false);
        }

        if (PlayerPrefs.GetInt("music") == 1)
        {
            _musicOff.SetActive(true);
            _musicOn.SetActive(false);
        }
        else
        {
            _musicOn.SetActive(true);
            _musicOff.SetActive(false);
        }
    }
    
    public void OpenSetting() {

        Time.timeScale = 0f;
        _pause.SetActive(true);
        _button.SetActive(false);
    }

    public void CloseSetting() {
        Time.timeScale = 1f;
        _pause.SetActive(false);
        _button.SetActive(true);
    }
}
