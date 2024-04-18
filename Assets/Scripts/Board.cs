using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Board : MonoBehaviour
{

    public GameObject card;

    void Start()
    {
        int gmLevel = GameManager.sceneVariable.level;
        int lv = gmLevel + 2;

        int[] arr = new int[(lv * 4)];
        for (int i = 0; i < (lv * 4); i++)
        {
            arr[i] = i / 2;
        }  // ���� ��������(�Ǵ� ����)�� ���� ī�� �迭 ���� [0~5 12��/0~7 16��/0~9 20��]

        arr = arr.OrderBy(x => Random.Range(0f, 9f)).ToArray();

        for (int i = 0; i < (lv * 4); i++) // for (�ʱⰪ; ����; ��ȭ) {�ݺ� ����}
        {
            GameObject go = Instantiate(card, this.transform);

            float x = (i % 4) * 1.4f - 2.1f;  // ���� 4�� ����
            float y = (i / 4) * 1.4f - 1.6f - (0.7f * gmLevel);  // ���� 3 ~ 5�� [-2.3f, -3.0f, -3.7f]
            go.transform.position = new Vector2(x, y);

            go.GetComponent<Card>().Setting(arr[i]);
        }

        GameManager.Instance.cardCount = arr.Length;
        GameManager.Instance.cardMaxCount = arr.Length;
    }
}
