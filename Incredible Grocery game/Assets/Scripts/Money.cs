using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Money : MonoBehaviour
{
    [SerializeField]
    private Sounds _sounds; // �����


    [SerializeField]
    private Text _value; // ������ �� ���������

    void Start () {
        _value.text = "" + GetMoney(); // ������������� ��� ������ ������� �-�� �����
    }

    public int GetMoney() {
        return PlayerPrefs.GetInt("Money"); // �� ����� ����� �-�� �����
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
