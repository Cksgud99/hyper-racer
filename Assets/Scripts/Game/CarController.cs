using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    [SerializeField] private GameObject gasEffectPrefab;
    [SerializeField] private GameObject monsterEffectPrefab;
    [SerializeField] private int gas = 300;
    [SerializeField] private float moveSpeed = 1f;
    
    public int Gas { get => gas; }      // Gas 정보

    private void Start()
    {
        StartCoroutine(GasCoroutine());
    }

    IEnumerator GasCoroutine()
    {
        while (true)
        {
            gas -= 10;
            yield return new WaitForSeconds(1f);
            if (gas <= 0) break;
        }
        
        // 게임종료
        GameManager.Instance.EndGame();
    }
    
    /// <summary>
    /// 자동차 이동 메서드
    /// </summary>
    /// <param name="direction"></param>
    public void Move(float direction)
    {
        transform.Translate(Vector3.right * (direction * moveSpeed * Time.deltaTime));
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, -1.5f, 1.5f), transform.position.y , transform.position.z);
    }

    /// <summary>
    /// 가스 아이템 획득시 호출되는 메서드
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        // TODO: 이펙트 재생
        if (other.CompareTag("Gas"))
        {
            gas += 30;
            GameManager.Instance.SetRoadSpeed(GameManager.Instance.roadMoveSpeed + 0.5f);
            
            //PlayEffect(GameManager.Instance.GetEffect(gasEffectPrefab), other.transform.position);
            
            // 가스 아이템 숨기기
            other.gameObject.SetActive(false);
        }
        
        else if (other.CompareTag("Monster"))
        {
            GameManager.Instance.SetRoadSpeed(0f);
            //StartCoroutine(MonsterCollisionEffect(other.transform.position));
        }
    }
    
    // /// <summary>
    // /// 몬스터 충돌 이펙트 생성 및 대기, 게임종료
    // /// </summary>
    // /// <param name="position"></param>
    // /// <returns></returns>
    // private IEnumerator MonsterCollisionEffect(Vector3 position)
    // {
    //     // 몬스터 이펙트 재생
    //     PlayEffect(GameManager.Instance.GetEffect(monsterEffectPrefab), position);
    //
    //     yield return new WaitForSeconds(1f);
    //     GameManager.Instance.EndGame();
    //     GameManager.Instance.SetRoadSpeed(5f);
    // }
    //
    // /// <summary>
    // /// 이펙트 재생
    // /// </summary>
    // /// <param name="effectPrefab"></param>
    // /// <param name="position"></param>
    // private void PlayEffect(GameObject effect, Vector3 position)
    // {
    //     effect.transform.position = position;
    //     StartCoroutine(ReturnEffectAfterDelay(effect, 1f));
    // }
    //
    // /// <summary>
    // /// 이펙트 재생 시간 대기
    // /// </summary>
    // /// <param name="effect"></param>
    // /// <param name="delay"></param>
    // /// <returns></returns>
    // private IEnumerator ReturnEffectAfterDelay(GameObject effect, float delay)
    // {
    //     yield return new WaitForSeconds(delay);
    //     GameManager.Instance.ReturnEffect(effect);
    // }
}
