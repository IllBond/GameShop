using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Emotion : MonoBehaviour
{

    [SerializeField]
    private Sounds _sounds; // Звуки

    [SerializeField]
    private GameObject _union; // Облочко диалоговое
    private List<Product> _added; // Товары которые добавил кассир
    private List<Sprite> _need; // Товары которые нужны
    private int _count = 0; // К-во правильнх товаров
    private bool _isErrors; // Есть ли ошибки?
    private Money _money; // Класс с добавлением денег
    private Client _client; // Класс клиента
    [SerializeField]
    private GameObject _mindElementPrefab; // Преваб того что появляется в диалоговом окне



    [SerializeField]
    private Sprite _goodEmotion; // Спрайт хорошей эмоции
    [SerializeField]
    private Sprite _badEmotion; // Спрайт плохой эмоции

    void Start() {
        _money = FindObjectOfType<Money>();
        _need = FindObjectOfType<GameController>().need;
        _added = FindObjectOfType<GameController>().added;
        _client = GetComponent<Client>();
        _sounds = FindObjectOfType<Sounds>();
    }

    public void SayEmotion() // Вызывет продавец когда собрал
    {
        IEnumerator GetEmotion(float time)
        {
            yield return new WaitForSeconds(time);
            _union.SetActive(true); //Включить диалоговое окно
            _sounds.UnionShow();

            for (int i = 0; i < _need.Count; i++) // Проходимся по списку того что нам нужно 
            {
                if (_need.Contains(_added[i].GetComponent<SpriteRenderer>().sprite)) // Сравниваем то что нужно с тем что собрано
                {
                    _count++; // Если товар есть значит +1 к счетчику
                }
                else
                {
                    _isErrors = true; // Если товара нет то Ошибка есть и к счетчику ничего не добавим
                }

                if (i == _need.Count - 1) // Если последняя итерация то прибавим денег в кассу, в зависимости от правильных ответов и вызовем эмоцию
                {
                    if (_isErrors)
                    {
                        BadEmotion();
                        _money.SetMoney(10 * _count);
                    }
                    else
                    {
                        GoodEmotion();
                        _money.SetMoney(10 * _count * 2);
                    }
                }
            }
        }
        StartCoroutine(GetEmotion(1)); // Запустим через 1 сек
    }

    private void GoodEmotion()
    {
        _client.DeleteMindProduct(); // удалить из union продукты
        SetEmotion(_goodEmotion); // Установить хорошие эмоции
    }

    private void BadEmotion()
    {
        _client.DeleteMindProduct();// удалить из union продукты
        SetEmotion(_badEmotion); // Установить плохие эмоции

    }


    //Ставим эмоцию и клиент идет домой
    private void SetEmotion(Sprite emotion)
    {
        float offsetX = 0.75f; // Смещение по x
        float offsetY = 0.2f; // Смещение по Y

        GameObject MindElement = Instantiate(_mindElementPrefab, new Vector3(_union.transform.position.x - offsetX, _union.transform.position.y + offsetY, 1), Quaternion.identity, _union.transform);
        MindElement.transform.position = new Vector2(MindElement.transform.position.x + 0.7f, MindElement.transform.position.y);
        Sprite sprite = emotion;
        MindElement.GetComponent<SpriteRenderer>().sprite = sprite;

        _client.GoBack();
    }
}
