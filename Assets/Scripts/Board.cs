using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq; // 카드 랜덤 배정

public class Board : MonoBehaviour
{
    public GameObject card;
    
    // Start is called before the first frame update
    void Start()
    {
        int gmLevel = GameManager.sceneVariable.level;
        int lv = gmLevel + 2;

        int[] arr = new int[(lv * 4)];
        for (int i = 0; i < (lv * 4); i++)
        {
            arr[i] = i / 2;
        }  // 현재 스테이지(또는 난도)에 따라 카드 배열 증가 [0~5 12개/0~7 16개/0~9 20개]

        arr = arr.OrderBy(x => Random.Range(0f, 9f)).ToArray();
        // =>: 배열을 순서대로 1회씩 순회한다는 뜻, Random으로 값 지정, ToArray로 배열로 변환
        
        for (int i = 0; i < (lv * 4); i++) // for (초기값; 조건; 변화) {반복 내용}
        {
            GameObject go = Instantiate(card, this.transform); // Instiate (생성물, this.transform); - 이 오브젝트 밑으로 생성
            
            float x = (i % 4) * 1.4f - 2.1f;  // 가로 4줄 고정
            float y = (i / 4) * 1.4f - 1.6f - (0.7f * gmLevel);  // 세로 3 ~ 5줄 [-2.3f, -3.0f, -3.7f]
            go.transform.position = new Vector2(x,y);

            // Card 컴포넌트의 Setting 함수에 arr[i] 값을 넘겨줌
            go.GetComponent<Card>().Setting(arr[i]);
            go.GetComponent<Card>().showNum = i;
        }

        GameManager.instance.cardCount = arr.Length;  // Length: 글자수, 배열 개수 구하는 함수
        GameManager.instance.cardMaxCount = arr.Length;
    }
}
