using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Emotion : MonoBehaviour
{

    [SerializeField]
    private Sounds _sounds; // �����

    [SerializeField]
    private GameObject _union; // ������� ����������
    private List<Product> _added; // ������ ������� ������� ������
    private List<Sprite> _need; // ������ ������� �����
    private int _count = 0; // �-�� ��������� �������
    private bool _isErrors; // ���� �� ������?
    private Money _money; // ����� � ����������� �����
    private Client _client; // ����� �������
    [SerializeField]
    private GameObject _mindElementPrefab; // ������ ���� ��� ���������� � ���������� ����



    [SerializeField]
    private Sprite _goodEmotion; // ������ ������� ������
    [SerializeField]
    private Sprite _badEmotion; // ������ ������ ������

    void Start() {
        _money = FindObjectOfType<Money>();
        _need = FindObjectOfType<GameController>().need;
        _added = FindObjectOfType<GameController>().added;
        _client = GetComponent<Client>();
        _sounds = FindObjectOfType<Sounds>();
    }

    public void SayEmotion() // ������� �������� ����� ������
    {
        IEnumerator GetEmotion(float time)
        {
            yield return new WaitForSeconds(time);
            _union.SetActive(true); //�������� ���������� ����
            _sounds.UnionShow();

            for (int i = 0; i < _need.Count; i++) // ���������� �� ������ ���� ��� ��� ����� 
            {
                if (_need.Contains(_added[i].GetComponent<SpriteRenderer>().sprite)) // ���������� �� ��� ����� � ��� ��� �������
                {
                    _count++; // ���� ����� ���� ������ +1 � ��������
                }
                else
                {
                    _isErrors = true; // ���� ������ ��� �� ������ ���� � � �������� ������ �� �������
                }

                if (i == _need.Count - 1) // ���� ��������� �������� �� �������� ����� � �����, � ����������� �� ���������� ������� � ������� ������
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
        StartCoroutine(GetEmotion(1)); // �������� ����� 1 ���
    }

    private void GoodEmotion()
    {
        _client.DeleteMindProduct(); // ������� �� union ��������
        SetEmotion(_goodEmotion); // ���������� ������� ������
    }

    private void BadEmotion()
    {
        _client.DeleteMindProduct();// ������� �� union ��������
        SetEmotion(_badEmotion); // ���������� ������ ������

    }


    //������ ������ � ������ ���� �����
    private void SetEmotion(Sprite emotion)
    {
        float offsetX = 0.75f; // �������� �� x
        float offsetY = 0.2f; // �������� �� Y

        GameObject MindElement = Instantiate(_mindElementPrefab, new Vector3(_union.transform.position.x - offsetX, _union.transform.position.y + offsetY, 1), Quaternion.identity, _union.transform);
        MindElement.transform.position = new Vector2(MindElement.transform.position.x + 0.7f, MindElement.transform.position.y);
        Sprite sprite = emotion;
        MindElement.GetComponent<SpriteRenderer>().sprite = sprite;

        _client.GoBack();
    }
}
