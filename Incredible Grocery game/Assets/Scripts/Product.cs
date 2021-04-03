using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;


public class Product : MonoBehaviour
{

    [SerializeField]
    private Sounds _sounds; // �����

    public bool isSelect; // ��������� �������� ������ / �� ������

    [SerializeField]
    private SpriteRenderer _state; // ���������. ������ ������� ���� ��������

    [SerializeField]
    private Sprite _stateTrue; // ������ �������
    [SerializeField]
    private Sprite _stateFalse; // ������ ��������

    private GameController _gameController; // ����� � ������� ���� ������� ������� ��� ������������


    void Start () {
        _sounds = FindObjectOfType<Sounds>();
    }
     // ��������� � added
    public Product AddToStorage() 
    {
        _gameController = FindObjectOfType<GameController>(); 

        isSelect = true; // ����� ������
        _sounds.CheckProcut();
        SetState(_stateTrue); // ���������� �������
        GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.3f); // ������� ������� ����������
        _gameController.CheckEnableButton(); //��������� ����� �� �������� ������
        return this;
    }

    // ������� �� added
    public Product RemoveFromStorage() 
    {
        _gameController = FindObjectOfType<GameController>();

        isSelect = false; // ����� �� ������
        
        SetState(null); // ������ �������
        GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1); // ������� ������� ������������
        _gameController.CheckDisableButton();  //��������� ����� �� ��������� ������
        return this;
    }

    private void SetState(Sprite state)
    {
     
        _state.sprite = state;
    }





}
