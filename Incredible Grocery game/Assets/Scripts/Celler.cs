using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Celler : MonoBehaviour
{

    [SerializeField]
    private Sounds _sounds; // Звуки

    [SerializeField]
    private GameObject _union; // Диалоговое окно
    [SerializeField]
    private GameObject _mindElementPrefab; // Префаб обьекта мысли
    private GameObject _client; // Обьект текущего клиента

    private List<Product> _added; // список того что добавлено
    private List<Sprite> _need; // список того что нужно

    private GameController _gameController; // Функции из класса GameController для отключения кнопки

    private Storage _storage; // функция из Storage для того что бы спрятать Storage

    void Start() {
        _added = FindObjectOfType<GameController>().added;
        _need = FindObjectOfType<GameController>().need;
        _storage = FindObjectOfType<Storage>();
        _gameController = FindObjectOfType<GameController>();
    }


    private void SayWhatYouMake() // Функция вещается на кнопку
    {
        _union.SetActive(true); // Показать диалоговое окно
        _sounds.UnionShow();

        float offsetX = 0; //Смещение X
        float offsetY = 0.2f; //Смещение Y

        if (_added.Count == 2)
        {
            offsetX = 0.75f / 2;
        }

        if (_added.Count == 3)
        {
            offsetX = 0.75f;
        }

        List<GameObject> addedMindElement = new List<GameObject>(); // Добавленые в диалоговое окно товары

        for (int i = 0; i < _added.Count; i++) {
            GameObject MindElement = Instantiate(_mindElementPrefab, new Vector3(_union.transform.position.x- offsetX, _union.transform.position.y + offsetY, 1), Quaternion.identity, _union.transform);
            MindElement.transform.position = new Vector2(MindElement.transform.position.x + 0.7f*i, MindElement.transform.position.y);
            Sprite sprite = _added[i].GetComponent<SpriteRenderer>().sprite;
            MindElement.GetComponent<SpriteRenderer>().sprite = sprite;

            addedMindElement.Add(MindElement); // Добавляем в диалоговое окно
            _added[i].RemoveFromStorage(); // Удалить из списка добавленого в added

            if (i == _added.Count - 1) { // Если послежняя итерация
                StartCoroutine(CheckProduct(1, addedMindElement)); // С задержкой 1 сек запустит проверяльщик товаров 
                _gameController.DisableButton(); // Кнопку sell отключить
                _storage.ReversSwapCoor(); // Спрятать окно Storage
            }
        }
    }

    IEnumerator CheckProduct(float time, List<GameObject> addedMindElement)
    {
        yield return new WaitForSeconds(time);
        float newTime = 0.5f; //Базовое время задержки срабатывания

        IEnumerator SetStatus(int i, float time) { // определяем какой статус у товара true или false
            yield return new WaitForSeconds(time);
            if (_need.Contains(addedMindElement[i].GetComponent<SpriteRenderer>().sprite))
            {
                addedMindElement[i].GetComponent<MindElement>().SetTrue();
            }
            else
            {
                addedMindElement[i].GetComponent<MindElement>().SetFalse();
            }

            if (i == addedMindElement.Count - 1) //Если послежняя итерация
            {
                _client = GameObject.FindGameObjectsWithTag("Client")[0]; //Находим клиента 
                _client.GetComponent<Emotion>().SayEmotion(); // Проим клиента сказать эмоцию

                IEnumerator FadeWhatYouMake(float time) { // Спрятать то что мы показали
                    yield return new WaitForSeconds(time);
                    DeleteMindProduct(addedMindElement); // Если послежняя итерация то удаляем все из Union 
                    _union.SetActive(false); // Скрываем Union
                    _sounds.UnionFade();
                }

                StartCoroutine(FadeWhatYouMake(1.5f)); //Через секунду

            }
        }

        for (int i = 0; i < addedMindElement.Count; i++)
        {
            StartCoroutine(SetStatus(i, newTime));
            newTime += 0.5f; // С каждой новой итерацией будет увеличиваться старт куротины
        }
    }

    private void DeleteMindProduct(List<GameObject> MindElement) //Удалить из диалогового окна элементы
    {
        for (int i = 0; i < MindElement.Count; i++)
        {
            Destroy(MindElement[i]);
        }
    }

}
