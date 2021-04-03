using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MindElement : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer _state;
    [SerializeField]
    private Sprite _stateTrue; // ������ ������
    [SerializeField]
    private Sprite _stateFalse; // ������ ������


    // �������� ������ ��������� ��������
    public void SetState(Sprite state)
    {
        _state.sprite = state;
    }    
    
    // ���������� ��� ����� �����
    public void SetTrue()
    {
        GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.3f);
        SetState(_stateTrue);
    }

    // ���������� ��� ����� �������
    public void SetFalse()
    {
        GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.3f);
        SetState(_stateFalse);
    }

}
