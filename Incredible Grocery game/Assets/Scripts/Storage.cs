using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Storage : MonoBehaviour
{
    private Vector2 _startPos; // ��� ������ ��������� ������
    private float _xCoor; // �������� �� X ��� ��������
    private float _yCoor; // �������� �� Y ��� ��������

    [SerializeField]
    private List<Sprite> productSprites = new List<Sprite>(); // ������� ������� ����� ��������� 

    private bool _isNeedSwap; // ���� true �� ���� Storage ������������

    private float _xSwapCoor; // ���������� ��� ����������� ���� Storage
    private float _ySwapCoor; // ���������� ��� ����������� ���� Storage
    [SerializeField]
    private Product _productPrefab; // ������ ��������
    [SerializeField]
    private Canvas _canvas; // ������ ��������


    void Start()
    {
        
        _xSwapCoor = 6.7f; // ���������� ��� ���� Storage ����� �������
        _ySwapCoor = transform.position.x; // ���������� ��� ���� Storage ����� ���������

        SetProduct(); // ��� �������� ������� �� ������� ���� � ���� �� ������� ����������� ������
    }


    void Update() {
        if (Time.timeScale > 0)
        {
            SwapStorage();
        }
            

      


    }

    // ������ ����������� ����
    private void SwapStorage() { 
        if (_isNeedSwap) { 
            transform.position = Vector3.Lerp(transform.position, new Vector3(_ySwapCoor, transform.position.y, transform.position.z), 0.1f);
            if (transform.position.x == _ySwapCoor)
            {
                _isNeedSwap = false;
            }
        }
    }

    // ���������� �������� 
    private void SetProduct() {
        float cash = 0.02700938f / _canvas.transform.localScale.x; // ��� ����������� ������������ ������������ ���������� ������
        _startPos = new Vector2(-2/ cash, 2.7f/ cash); // ��� ������ ��������� ������
        _xCoor = 1.33f/ cash; // �������� �� X ��� ��������
        _yCoor = 1.5f/ cash; // �������� �� Y ��� ��������


        int count = 0; // �������
        float cash_xCoor = _startPos.x; // ��� ��������� ���������� X


        for (int i = 0; i < productSprites.Count; i++) { //������� ������� � ��� ��������
            Product product = Instantiate(_productPrefab, _startPos, Quaternion.identity, transform); // ��������
            product.transform.position = new Vector2(transform.position.x + product.transform.position.x, transform.position.y + product.transform.position.y); // ������ ������� 
            product.GetComponent<SpriteRenderer>().sprite = productSprites[i]; // ������ ������
           
            count++; // ����������� �������
            _startPos.x += _xCoor; // ������ ��������� �� X

            if (count%4==0) { // ������ 4 ������ �������� ���������� �� X, � �� Y ������ ����
                _startPos.y -= _yCoor; // ������ ��������� �� Y
                _startPos.x = cash_xCoor; //�������� ��������� �� X
            }
        }
    }

    // ��������� � ������������ 
    public void ReversSwapCoor() {
        _isNeedSwap = true; // ����� ��������
        float cash = _ySwapCoor; // ������ ������� X � Y
        _ySwapCoor = _xSwapCoor; // ������ ������� X � Y
        _xSwapCoor = cash; // ������ ������� X � Y
    }
}
