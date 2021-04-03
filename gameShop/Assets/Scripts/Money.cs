using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Money : MonoBehaviour
{
    [SerializeField]
    private Sounds _sounds; // Звуки


    [SerializeField]
    private Text _value; // Объект со значением

    void Start () {
        _value.text = "" + GetMoney(); // Устанавливаем при старте текущее к-во денег
    }

    public int GetMoney() {
        return PlayerPrefs.GetInt("Money"); // из префа берем к-во денег
    }
    
    public void SetMoney(int val) {
        if (val > 0) {
            int values = val + GetMoney();
            PlayerPrefs.SetInt("Money", values);
            _value.text = "" + values;
            _sounds.AddMoney();
        }

    }

}
