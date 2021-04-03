using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Client : MonoBehaviour
{
    [SerializeField]
    private Sounds _sounds; // Звуки

    public Vector3 StartCoor { get; set; } =  new Vector3(-8.5f, 1, 1); // Координаты входа
    public Vector3 EndCoor { get; set; } = new Vector3(-4, -1, 1); // Координаты кассы

    private bool _isMove = true; // Если true то клиент начнет идти
    private bool _isGoBack; // Если false то клиент пойдет к выходу

    private float _speed; // Коэфициент скорости движения
    private float _startTime; // Время начала движения
    private float _journeyLength; // Расстояние между началом и концом движения

    [SerializeField]
    private GameObject _union; // Облочко диалоговое
    [SerializeField]
    private GameObject _walk;  // Анимированный спрайт 
    [SerializeField]
    private GameObject _mindElementPrefab; // Преваб того что появляется в диалоговом окне

    [SerializeField]
    private List<Sprite> _productSprites; // Спрайты товаров которые клиент модет заказать

    private List<GameObject> _products = new List<GameObject>(); // Продукты которе появятся в диалоговом облочке
    private GameController _setting; // Функции из класа gameController;

    void Start() {
        ResetMoveParams(); 
        _setting = FindObjectOfType<GameController>();
        _sounds = FindObjectOfType<Sounds>();
    }

    // Сброс параметров для движения. Нужно для равномерного движения
    void ResetMoveParams() {
        _speed = 0.05f;
        _startTime = Time.time; // Примерно 1.0025
        _journeyLength = Vector3.Distance(StartCoor, EndCoor);
    }

    void Update() {

        if (Time.timeScale > 0) {
            Move();
        }
        
    }

    private void Move() {
        if (_isMove)
        {
            _walk.GetComponent<Animator>().enabled = true; // Включаем анимацию
            float distCovered = (Time.time - _startTime) * _speed; // коэфициент изменения скорости
            float fracJourney = distCovered / _journeyLength; // коэфициент скорости скорости

            transform.position = Vector3.Lerp(transform.position, EndCoor, fracJourney); // Конец и начало движеня

            if (transform.position.x > EndCoor.x - 0.1f && transform.position.y > EndCoor.y - 0.1f) // когда мы в конце
            {
                if (_isGoBack)
                {
                    _setting.NewClient(); // Выйти и зайдет новый клиент
                }
                else {
                    _walk.GetComponent<Animator>().enabled = false; // Останавливаем анимацию
                    transform.position = EndCoor; // На всякий случай меняем координат на тот в который нам нужно было трансформироваться
                    EndMove(); // Останавливаем движение + говорим что хотим
                    GameObject store = GameObject.FindGameObjectsWithTag("Store")[0]; // Находим Store
                    StartCoroutine(GetStore(store, 5)); // Задержка 5 сек и запускаем функцию открытия store
                }
            }
        }
    }

    IEnumerator GetStore(GameObject store, float time) {
        yield return new WaitForSeconds(time);
        store.GetComponent<Storage>().ReversSwapCoor(); // Открыть магазин
    }

    IEnumerator FadeWhatYouWan(float time) {
        yield return new WaitForSeconds(time);
        _union.SetActive(false);
        _sounds.UnionFade();
    }

    private void StartMove()
    {
        _isMove = true;
    }

    private void EndMove()
    {
        _isMove = false; // закончить движение
        if (!_isGoBack) {
            SayWhatYouWant(); // Сказать что хотим купить
        }
        
    }

    private void SayWhatYouWant() // Глиент говорит что он хочет
    {
        _union.SetActive(true); // Включить диалоговое окно
        _sounds.UnionShow();

        int number = Random.Range(1, 4); // Сгенирировать к-во товаров которе мы хотим от 1 до 3
        List<Sprite> productSpritesCash = new List<Sprite>(); // Создаем временное хранилище для спрайтов
        productSpritesCash.AddRange(_productSprites); // Добавляем все существующие спрайты во временное поле
        float offsetX = 0; // Смещение по x относительно Union
        float offsetY = 0.2f; // Смещение по y относительно Union

        if (number == 2) { offsetX = 0.75f / 2; }
        if (number == 3) { offsetX = 0.75f; }

        //По очереди вставляем в Union то что хотим
        for (int i = 0; i < number; i++) {
            GameObject MindElement = Instantiate(_mindElementPrefab, new Vector3(_union.transform.position.x- offsetX, _union.transform.position.y + offsetY, 1), Quaternion.identity, _union.transform);
            MindElement.transform.position = new Vector2(MindElement.transform.position.x + 0.7f*i, MindElement.transform.position.y);
            Sprite sprite = productSpritesCash[Random.Range(0, _productSprites.Count-1)]; // Генирируем рандомный спрайт
            MindElement.GetComponent<SpriteRenderer>().sprite = sprite; // Вставляем спрайт 

            _products.Add(MindElement); // В список продуктов добавили продукт

            productSpritesCash.Remove(sprite); //Исключаем из списка доступных товаров те что уже есть
            FindObjectOfType<GameController>().need.Add(sprite); //В список того что хочет клиент В GAME CONTROLLER добавляем то что он хочет
        }

        StartCoroutine(FadeWhatYouWan(5)); // через 5 сек скрываем поле юнион
    }


    public void DeleteMindProduct() {
        for (int i = 0; i < _products.Count; i++)
        {
            Destroy(_products[i]);
        }
    }

    public void GoBack() {
        _walk.GetComponent<SpriteRenderer>().flipX = true; // Поворачиваем персонажа в другую сторону
        Vector3 cash = StartCoor; // Меняем местами координаты начала движения и конца движения
        StartCoor = EndCoor;
        EndCoor = cash;

        ResetMoveParams(); 

        _isMove = true; // начинаем движение
        _isGoBack = true; // Обозначаем что мы возвращаемся назад
    }
}
