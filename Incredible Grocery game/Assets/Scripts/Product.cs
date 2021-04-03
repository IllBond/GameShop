using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;


public class Product : MonoBehaviour
{

    [SerializeField]
    private Sounds _sounds; // Звуки

    public bool isSelect; // Состояние продукта выбран / не выбран

    [SerializeField]
    private SpriteRenderer _state; // состояние. Обьект который есть дочерним

    [SerializeField]
    private Sprite _stateTrue; // Спрайт галочки
    [SerializeField]
    private Sprite _stateFalse; // Спрайт крестика

    private GameController _gameController; // Класс в котором есть функции которые нам понадобяться


    void Start () {
        _sounds = FindObjectOfType<Sounds>();
    }
     // Добавляем в added
    public Product AddToStorage() 
    {
        _gameController = FindObjectOfType<GameController>(); 

        isSelect = true; // товар выбран
        _sounds.CheckProcut();
        SetState(_stateTrue); // Установить галочку
        GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.3f); // Сделать продукт прозрачным
        _gameController.CheckEnableButton(); //Проверить нужно ли включить кнопку
        return this;
    }

    // Удалять из added
    public Product RemoveFromStorage() 
    {
        _gameController = FindObjectOfType<GameController>();

        isSelect = false; // Товар не выбран
        
        SetState(null); // убрать галочку
        GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1); // Сделать продукт НЕпрозрачным
        _gameController.CheckDisableButton();  //Проверить нужно ли выключить кнопку
        return this;
    }

    private void SetState(Sprite state)
    {
     
        _state.sprite = state;
    }





}
