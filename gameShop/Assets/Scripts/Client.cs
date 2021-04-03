using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Client : MonoBehaviour
{
    [SerializeField]
    private Sounds _sounds; // �����

    public Vector3 StartCoor { get; set; } =  new Vector3(-8.5f, 1, 1); // ���������� �����
    public Vector3 EndCoor { get; set; } = new Vector3(-4, -1, 1); // ���������� �����

    private bool _isMove = true; // ���� true �� ������ ������ ����
    private bool _isGoBack; // ���� false �� ������ ������ � ������

    private float _speed; // ���������� �������� ��������
    private float _startTime; // ����� ������ ��������
    private float _journeyLength; // ���������� ����� ������� � ������ ��������

    [SerializeField]
    private GameObject _union; // ������� ����������
    [SerializeField]
    private GameObject _walk;  // ������������� ������ 
    [SerializeField]
    private GameObject _mindElementPrefab; // ������ ���� ��� ���������� � ���������� ����

    [SerializeField]
    private List<Sprite> _productSprites; // ������� ������� ������� ������ ����� ��������

    private List<GameObject> _products = new List<GameObject>(); // �������� ������ �������� � ���������� �������
    private GameController _setting; // ������� �� ����� gameController;

    void Start() {
        ResetMoveParams(); 
        _setting = FindObjectOfType<GameController>();
        _sounds = FindObjectOfType<Sounds>();
    }

    // ����� ���������� ��� ��������. ����� ��� ������������ ��������
    void ResetMoveParams() {
        _speed = 0.05f;
        _startTime = Time.time; // �������� 1.0025
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
            _walk.GetComponent<Animator>().enabled = true; // �������� ��������
            float distCovered = (Time.time - _startTime) * _speed; // ���������� ��������� ��������
            float fracJourney = distCovered / _journeyLength; // ���������� �������� ��������

            transform.position = Vector3.Lerp(transform.position, EndCoor, fracJourney); // ����� � ������ �������

            if (transform.position.x > EndCoor.x - 0.1f && transform.position.y > EndCoor.y - 0.1f) // ����� �� � �����
            {
                if (_isGoBack)
                {
                    _setting.NewClient(); // ����� � ������ ����� ������
                }
                else {
                    _walk.GetComponent<Animator>().enabled = false; // ������������� ��������
                    transform.position = EndCoor; // �� ������ ������ ������ ��������� �� ��� � ������� ��� ����� ���� ������������������
                    EndMove(); // ������������� �������� + ������� ��� �����
                    GameObject store = GameObject.FindGameObjectsWithTag("Store")[0]; // ������� Store
                    StartCoroutine(GetStore(store, 5)); // �������� 5 ��� � ��������� ������� �������� store
                }
            }
        }
    }

    IEnumerator GetStore(GameObject store, float time) {
        yield return new WaitForSeconds(time);
        store.GetComponent<Storage>().ReversSwapCoor(); // ������� �������
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
        _isMove = false; // ��������� ��������
        if (!_isGoBack) {
            SayWhatYouWant(); // ������� ��� ����� ������
        }
        
    }

    private void SayWhatYouWant() // ������ ������� ��� �� �����
    {
        _union.SetActive(true); // �������� ���������� ����
        _sounds.UnionShow();

        int number = Random.Range(1, 4); // ������������� �-�� ������� ������ �� ����� �� 1 �� 3
        List<Sprite> productSpritesCash = new List<Sprite>(); // ������� ��������� ��������� ��� ��������
        productSpritesCash.AddRange(_productSprites); // ��������� ��� ������������ ������� �� ��������� ����
        float offsetX = 0; // �������� �� x ������������ Union
        float offsetY = 0.2f; // �������� �� y ������������ Union

        if (number == 2) { offsetX = 0.75f / 2; }
        if (number == 3) { offsetX = 0.75f; }

        //�� ������� ��������� � Union �� ��� �����
        for (int i = 0; i < number; i++) {
            GameObject MindElement = Instantiate(_mindElementPrefab, new Vector3(_union.transform.position.x- offsetX, _union.transform.position.y + offsetY, 1), Quaternion.identity, _union.transform);
            MindElement.transform.position = new Vector2(MindElement.transform.position.x + 0.7f*i, MindElement.transform.position.y);
            Sprite sprite = productSpritesCash[Random.Range(0, _productSprites.Count-1)]; // ���������� ��������� ������
            MindElement.GetComponent<SpriteRenderer>().sprite = sprite; // ��������� ������ 

            _products.Add(MindElement); // � ������ ��������� �������� �������

            productSpritesCash.Remove(sprite); //��������� �� ������ ��������� ������� �� ��� ��� ����
            FindObjectOfType<GameController>().need.Add(sprite); //� ������ ���� ��� ����� ������ � GAME CONTROLLER ��������� �� ��� �� �����
        }

        StartCoroutine(FadeWhatYouWan(5)); // ����� 5 ��� �������� ���� �����
    }


    public void DeleteMindProduct() {
        for (int i = 0; i < _products.Count; i++)
        {
            Destroy(_products[i]);
        }
    }

    public void GoBack() {
        _walk.GetComponent<SpriteRenderer>().flipX = true; // ������������ ��������� � ������ �������
        Vector3 cash = StartCoor; // ������ ������� ���������� ������ �������� � ����� ��������
        StartCoor = EndCoor;
        EndCoor = cash;

        ResetMoveParams(); 

        _isMove = true; // �������� ��������
        _isGoBack = true; // ���������� ��� �� ������������ �����
    }
}
