using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Storage : MonoBehaviour
{
    private Vector2 _startPos; // Где начать создавать товары
    private float _xCoor; // Смещение по X при создании
    private float _yCoor; // Смещение по Y при создании

    [SerializeField]
    private List<Sprite> productSprites = new List<Sprite>(); // Спрайты которые будем вставлять 

    private bool _isNeedSwap; // Если true то окно Storage перемещается

    private float _xSwapCoor; // Коорндинат для перемещения окна Storage
    private float _ySwapCoor; // Коорндинат для перемещения окна Storage
    [SerializeField]
    private Product _productPrefab; // Префаб продукта
    [SerializeField]
    private Canvas _canvas; // Префаб продукта


    void Start()
    {
        
        _xSwapCoor = 6.7f; // Координата где окно Storage будет видимым
        _ySwapCoor = transform.position.x; // Координата где окно Storage будет Невидимым

        SetProduct(); // При создании обьекта на игровом поле в него по очереди вставляются товары
    }


    void Update() {
        if (Time.timeScale > 0)
        {
            SwapStorage();
        }
            

      


    }

    // Логика перемещения окна
    private void SwapStorage() { 
        if (_isNeedSwap) { 
            transform.position = Vector3.Lerp(transform.position, new Vector3(_ySwapCoor, transform.position.y, transform.position.z), 0.1f);
            if (transform.position.x == _ySwapCoor)
            {
                _isNeedSwap = false;
            }
        }
    }

    // Установить продукты 
    private void SetProduct() {
        float cash = 0.02700938f / _canvas.transform.localScale.x; // для корректного расположения относительно разрешения экрана
        _startPos = new Vector2(-2/ cash, 2.7f/ cash); // Где начать создавать товары
        _xCoor = 1.33f/ cash; // Смещение по X при создании
        _yCoor = 1.5f/ cash; // Смещение по Y при создании


        int count = 0; // Счетчик
        float cash_xCoor = _startPos.x; // Для обнуления координаты X


        for (int i = 0; i < productSprites.Count; i++) { //Столько сколько у нас спрайтов
            Product product = Instantiate(_productPrefab, _startPos, Quaternion.identity, transform); // Вставить
            product.transform.position = new Vector2(transform.position.x + product.transform.position.x, transform.position.y + product.transform.position.y); // Меняем позицию 
            product.GetComponent<SpriteRenderer>().sprite = productSprites[i]; // Меняем спрайт
           
            count++; // Увеличиваем счетчик
            _startPos.x += _xCoor; // Меняем координат по X

            if (count%4==0) { // Каждые 4 товара обнулять координату по X, а по Y делать ниже
                _startPos.y -= _yCoor; // Меняем координат по Y
                _startPos.x = cash_xCoor; //Обнуляем координат по X
            }
        }
    }

    // Появление и исчезновение 
    public void ReversSwapCoor() {
        _isNeedSwap = true; // Нужно свапнуть
        float cash = _ySwapCoor; // Меняем местами X и Y
        _ySwapCoor = _xSwapCoor; // Меняем местами X и Y
        _xSwapCoor = cash; // Меняем местами X и Y
    }
}
