using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MindElement : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer _state;
    [SerializeField]
    private Sprite _stateTrue; // спрайт истины
    [SerializeField]
    private Sprite _stateFalse; // спрайт ошибки


    // Изменить спрайт дочернего элемента
    public void SetState(Sprite state)
    {
        _state.sprite = state;
    }    
    
    // Установить что выбор верен
    public void SetTrue()
    {
        GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.3f);
        SetState(_stateTrue);
    }

    // Установить что выбор НЕверен
    public void SetFalse()
    {
        GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.3f);
        SetState(_stateFalse);
    }

}
