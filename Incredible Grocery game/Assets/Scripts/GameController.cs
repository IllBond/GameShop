using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    [SerializeField]
    private Client _clientPrefab; // Клиент который будет появлятся в игре
    private bool _isTouched = true; // Клиент который будет появлятся в игре

    public List<Sprite> need = new List<Sprite>(); // То что нужно клиенту
    public List<Product> added = new List<Product>(); // То что добавлено в корзину


    void OnEnable() {
        if (PlayerPrefs.GetInt("firstStart") == 0)
        {
            PlayerPrefs.SetInt("firstStart", 1);
            PlayerPrefs.SetInt("music", 1);
            PlayerPrefs.SetInt("sound", 1);
        }
    }

    void Start()
    {

        EnterClient();
    }

    void Update() {
        //Отслеживает нажатия на продукты. 
       
      /*    
        //Альтернативное управление мышкой
        if (Input.GetMouseButtonDown(0)) {
            RaycastHit2D ray = Physics2D.GetRayIntersection(Camera.main.ScreenPointToRay(Input.mousePosition));
      */

        if (Input.touchCount == 0) {
            _isTouched = true;
        }

        if (Input.touchCount > 0 && _isTouched)
        {

            _isTouched = false;
            RaycastHit2D ray = Physics2D.GetRayIntersection(Camera.main.ScreenPointToRay(Input.GetTouch(0).position));

            if (ray != false)
            {
                if (!ray.collider.gameObject.GetComponent<Product>().isSelect && added.Count < need.Count)
                {
                    // Если продукт не выделен то выделить его и добавить в список added
                    added.Add(ray.collider.gameObject.GetComponent<Product>().AddToStorage()); // Возвращается тот продукт который нажали
                    CheckEnableButton();
                } else {
                    // Если продукт  выделен то снять выделение и убрать его из списка added
                    added.Remove(ray.collider.gameObject.GetComponent<Product>().RemoveFromStorage()); // Возвращается тот продукт который нажали
                    CheckDisableButton();
                }
        }
        }
    }


    //Создаем нового клиента 
    public void NewClient() {
        Destroy(GameObject.FindGameObjectsWithTag("Client")[0]); // Старого удаляем 
        need.Clear();  // Чистим поле того что нам нужно
        added.Clear(); // Чистим поле того что добавлено
        EnterClient(); // Создаем клиента
    }



    private void EnterClient() {
        IEnumerator Enter (float time)
        {
            yield return new WaitForSeconds(time);
            Instantiate(_clientPrefab, _clientPrefab.StartCoor, Quaternion.identity); // Через 1 секунду создасться клиент
        }
        StartCoroutine(Enter(1)); 
    }


    public void CheckEnableButton() // нужно ли включить кнопку sell 
    {
        if (need.Count == added.Count)
        {
            GameObject button = GameObject.FindGameObjectsWithTag("Sell")[0];
            button.GetComponent<Button>().interactable = true;
        }
    }

    public void CheckDisableButton() // Проверить нужно ли отключить кнопку sell
    {
        if (need.Count > added.Count || need.Count == 0 || added.Count == 0)
        {
            GameObject button = GameObject.FindGameObjectsWithTag("Sell")[0];
            button.GetComponent<Button>().interactable = false;
        }
    }

    public void DisableButton() // отключить кнопку
    {
        GameObject button = GameObject.FindGameObjectsWithTag("Sell")[0];
        button.GetComponent<Button>().interactable = false;
    }
}
