using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class RoadController : MonoBehaviour
{
    [SerializeField] private GameObject[] gasObjects;
    [SerializeField] private GameObject[] monsterObjects;

    private void OnEnable()
    {
        // 모든 가스 아이템 비활성화
        foreach (var gasObject in gasObjects)
        {
            gasObject.SetActive(false);
        }

        // 모든 몬스터 비활성화
        foreach (var monsterObject in monsterObjects)
        {
            monsterObject.SetActive(false);
        }
    }


    /// <summary>
    /// 플레이어 차량이 도로에 진입하면 다음 도로를 생성
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.SpawnRoad(transform.position + new Vector3(0,0,10));
        }
    }

    /// <summary>
    /// 플레이어 차량이 도로를 벗어나면 해당 도로를 제거
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.DestroyRoad(gameObject);
        }
    }

    /// <summary>
    ///  랜덤으로 가스 아이템을 생성
    /// </summary>
    public void SpawnGas()
    {
        int index = Random.Range(0, gasObjects.Length);
        gasObjects[index].SetActive(true);
    }

    /// <summary>
    /// 가스가 생성되지 않은 위치에 몬스터 생성
    /// </summary>
    public void SpawnMonsters()
    {
        for (int i = 0; i < gasObjects.Length; i++)
        {
            // 가스가 비활성화된 위치에만 몬스터 활성화
            if (!gasObjects[i].activeSelf && i < monsterObjects.Length)
            {
                monsterObjects[i].SetActive(true);
            }
        }
    }
}
