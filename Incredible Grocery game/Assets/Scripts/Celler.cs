using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Celler : MonoBehaviour
{

    [SerializeField]
    private Sounds _sounds; // �����

    [SerializeField]
    private GameObject _union; // ���������� ����
    [SerializeField]
    private GameObject _mindElementPrefab; // ������ ������� �����
    private GameObject _client; // ������ �������� �������

    private List<Product> _added; // ������ ���� ��� ���������
    private List<Sprite> _need; // ������ ���� ��� �����

    private GameController _gameController; // ������� �� ������ GameController ��� ���������� ������

    private Storage _storage; // ������� �� Storage ��� ���� ��� �� �������� Storage

    void Start() {
        _added = FindObjectOfType<GameController>().added;
        _need = FindObjectOfType<GameController>().need;
        _storage = FindObjectOfType<Storage>();
        _gameController = FindObjectOfType<GameController>();
    }


    private void SayWhatYouMake() // ������� �������� �� ������
    {
        _union.SetActive(true); // �������� ���������� ����
        _sounds.UnionShow();

        float offsetX = 0; //�������� X
        float offsetY = 0.2f; //�������� Y

        if (_added.Count == 2)
        {
            offsetX = 0.75f / 2;
        }

        if (_added.Count == 3)
        {
            offsetX = 0.75f;
        }

        List<GameObject> addedMindElement = new List<GameObject>(); // ���������� � ���������� ���� ������

        for (int i = 0; i < _added.Count; i++) {
            GameObject MindElement = Instantiate(_mindElementPrefab, new Vector3(_union.transform.position.x- offsetX, _union.transform.position.y + offsetY, 1), Quaternion.identity, _union.transform);
            MindElement.transform.position = new Vector2(MindElement.transform.position.x + 0.7f*i, MindElement.transform.position.y);
            Sprite sprite = _added[i].GetComponent<SpriteRenderer>().sprite;
            MindElement.GetComponent<SpriteRenderer>().sprite = sprite;

            addedMindElement.Add(MindElement); // ��������� � ���������� ����
            _added[i].RemoveFromStorage(); // ������� �� ������ ����������� � added

            if (i == _added.Count - 1) { // ���� ��������� ��������
                StartCoroutine(CheckProduct(1, addedMindElement)); // � ��������� 1 ��� �������� ������������ ������� 
                _gameController.DisableButton(); // ������ sell ���������
                _storage.ReversSwapCoor(); // �������� ���� Storage
            }
        }
    }

    IEnumerator CheckProduct(float time, List<GameObject> addedMindElement)
    {
        yield return new WaitForSeconds(time);
        float newTime = 0.5f; //������� ����� �������� ������������

        IEnumerator SetStatus(int i, float time) { // ���������� ����� ������ � ������ true ��� false
            yield return new WaitForSeconds(time);
            if (_need.Contains(addedMindElement[i].GetComponent<SpriteRenderer>().sprite))
            {
                addedMindElement[i].GetComponent<MindElement>().SetTrue();
            }
            else
            {
                addedMindElement[i].GetComponent<MindElement>().SetFalse();
            }

            if (i == addedMindElement.Count - 1) //���� ��������� ��������
            {
                _client = GameObject.FindGameObjectsWithTag("Client")[0]; //������� ������� 
                _client.GetComponent<Emotion>().SayEmotion(); // ����� ������� ������� ������

                IEnumerator FadeWhatYouMake(float time) { // �������� �� ��� �� ��������
                    yield return new WaitForSeconds(time);
                    DeleteMindProduct(addedMindElement); // ���� ��������� �������� �� ������� ��� �� Union 
                    _union.SetActive(false); // �������� Union
                    _sounds.UnionFade();
                }

                StartCoroutine(FadeWhatYouMake(1.5f)); //����� �������

            }
        }

        for (int i = 0; i < addedMindElement.Count; i++)
        {
            StartCoroutine(SetStatus(i, newTime));
            newTime += 0.5f; // � ������ ����� ��������� ����� ������������� ����� ��������
        }
    }

    private void DeleteMindProduct(List<GameObject> MindElement) //������� �� ����������� ���� ��������
    {
        for (int i = 0; i < MindElement.Count; i++)
        {
            Destroy(MindElement[i]);
        }
    }

}
