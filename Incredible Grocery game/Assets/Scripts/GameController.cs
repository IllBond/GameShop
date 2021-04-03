using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    [SerializeField]
    private Client _clientPrefab; // ������ ������� ����� ��������� � ����
    private bool _isTouched = true; // ������ ������� ����� ��������� � ����

    public List<Sprite> need = new List<Sprite>(); // �� ��� ����� �������
    public List<Product> added = new List<Product>(); // �� ��� ��������� � �������


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
        //����������� ������� �� ��������. 
       
      /*    
        //�������������� ���������� ������
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
                    // ���� ������� �� ������� �� �������� ��� � �������� � ������ added
                    added.Add(ray.collider.gameObject.GetComponent<Product>().AddToStorage()); // ������������ ��� ������� ������� ������
                    CheckEnableButton();
                } else {
                    // ���� �������  ������� �� ����� ��������� � ������ ��� �� ������ added
                    added.Remove(ray.collider.gameObject.GetComponent<Product>().RemoveFromStorage()); // ������������ ��� ������� ������� ������
                    CheckDisableButton();
                }
        }
        }
    }


    //������� ������ ������� 
    public void NewClient() {
        Destroy(GameObject.FindGameObjectsWithTag("Client")[0]); // ������� ������� 
        need.Clear();  // ������ ���� ���� ��� ��� �����
        added.Clear(); // ������ ���� ���� ��� ���������
        EnterClient(); // ������� �������
    }



    private void EnterClient() {
        IEnumerator Enter (float time)
        {
            yield return new WaitForSeconds(time);
            Instantiate(_clientPrefab, _clientPrefab.StartCoor, Quaternion.identity); // ����� 1 ������� ���������� ������
        }
        StartCoroutine(Enter(1)); 
    }


    public void CheckEnableButton() // ����� �� �������� ������ sell 
    {
        if (need.Count == added.Count)
        {
            GameObject button = GameObject.FindGameObjectsWithTag("Sell")[0];
            button.GetComponent<Button>().interactable = true;
        }
    }

    public void CheckDisableButton() // ��������� ����� �� ��������� ������ sell
    {
        if (need.Count > added.Count || need.Count == 0 || added.Count == 0)
        {
            GameObject button = GameObject.FindGameObjectsWithTag("Sell")[0];
            button.GetComponent<Button>().interactable = false;
        }
    }

    public void DisableButton() // ��������� ������
    {
        GameObject button = GameObject.FindGameObjectsWithTag("Sell")[0];
        button.GetComponent<Button>().interactable = false;
    }
}
