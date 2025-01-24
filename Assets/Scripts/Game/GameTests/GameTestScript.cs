using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class GameTestScript
{
    private CarController _carController;
    private GameObject _leftMoveButton;
    private GameObject _rightMoveButton;
    
    // A Test behaves as an ordinary method
    [Test]
    public void GameTestScriptSimplePasses()
    {
        // Use the Assert class to test conditions
    }

    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    [UnityTest]
    public IEnumerator GameTestScriptWithEnumeratorPasses()
    {
        // Use the Assert class to test conditions.
        // Use yield to skip a frame.
        
        // 타임스케일 변경
        Time.timeScale = 10f;
        
        // 씬 로드하기
        SceneManager.LoadScene("Scenes/Game", LoadSceneMode.Single);
        yield return waitForSceneLoad();
        
        // 필수 오브젝트 확인
        var gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        Assert.IsNotNull(gameManager, "GameManager is Null");

        var startButton = GameObject.Find("Start Button");
        Assert.IsNotNull(startButton, "StartButton is Null");
        
        _leftMoveButton = GameObject.Find("Left Move Button");
        Assert.IsNotNull(_leftMoveButton, "LeftButton is Null");
        
        _rightMoveButton = GameObject.Find("Right Move Button");
        Assert.IsNotNull(_rightMoveButton, "RightButton is Null");
        
        // 게임 실행
        startButton.GetComponent<UnityEngine.UI.Button>().onClick.Invoke();
        
        //플레이어 자동차 확인
        _carController = GameObject.Find("Car(Clone)").GetComponent<CarController>();
        Assert.IsNotNull(_carController, "CarController is Null");
        
        //가스의 등장 위치 파악하기
        Vector3 lefPos = new Vector3(-1f, 0.2f, -3f);
        Vector3 rightPos = new Vector3(1f, 0.2f, -3f);
        Vector3 centerPos = new Vector3(0f, 0.2f, -3f);

        float rayDistance = 10f;
        Vector3 rayDirection = Vector3.forward;
        
        // 플레이 시간
        float elapsedTime = 0f;
        float targetTime = 10f;
        
        // 반복
        while (gameManager.GameState == GameManager.State.Play)
        {
            RaycastHit hit;
            if (Physics.Raycast(lefPos, rayDirection, out hit, rayDistance, LayerMask.GetMask("Gas")))
            {
                Debug.Log("left");
                MoveCar(hit.point);
            }
            
            else if (Physics.Raycast(rightPos, rayDirection, out hit, rayDistance, LayerMask.GetMask("Gas")))
            {
                Debug.Log("right");
                MoveCar(hit.point);
            }
            
            else if (Physics.Raycast(centerPos, rayDirection, out hit, rayDistance, LayerMask.GetMask("Gas")))
            {
                Debug.Log("center");
                MoveCar(hit.point);
            }

            else
            {
                Debug.Log("none");
                MoveButtonUp(_leftMoveButton);
                MoveButtonUp(_rightMoveButton);
            }
            
            Debug.DrawRay(lefPos, rayDirection * rayDistance, Color.red);
            Debug.DrawRay(centerPos, rayDirection * rayDistance, Color.green);
            Debug.DrawRay(rightPos, rayDirection * rayDistance, Color.blue);
            
            // 시간 체크
            elapsedTime += Time.deltaTime;
            
            yield return null;
        }

        if (elapsedTime < targetTime)
        {
            Assert.Fail("Game Time is too short");
        }
        
        Time.timeScale = 1f;
    }

    private IEnumerator waitForSceneLoad()
    {
        while (SceneManager.GetActiveScene().buildIndex > 0)
        {
            yield return null;
        }
    }

    /// <summary>
    /// Move Button Down
    /// </summary>
    /// <param name="moveButton"></param>
    private void MoveButtonDown(GameObject moveButton)
    {
        PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
        ExecuteEvents.Execute(moveButton, pointerEventData, ExecuteEvents.pointerDownHandler);
    }

    /// <summary>
    /// Move Button Up
    /// </summary>
    /// <param name="moveButton"></param>
    private void MoveButtonUp(GameObject moveButton)
    {
        PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
        ExecuteEvents.Execute(moveButton, pointerEventData, ExecuteEvents.pointerUpHandler);
    }

    /// <summary>
    /// 플레이어 자동차 이동
    /// </summary>
    /// <param name="targetPos">이동 타겟 위치</param>
    private void MoveCar(Vector3 targetPos)
    {
        if (Mathf.Abs(targetPos.x - _carController.transform.position.x) < 0.01f)
        {
            MoveButtonUp(_leftMoveButton);
            MoveButtonUp(_rightMoveButton);
            return;
        }
        
        if (targetPos.x < _carController.transform.position.x)
        { 
            //왼쪽으로 이동
            MoveButtonDown(_leftMoveButton);
            MoveButtonUp(_rightMoveButton);
        }
        
        else if (targetPos.x > _carController.transform.position.x)
        {
            //오른쪽으로 이동
            MoveButtonDown(_rightMoveButton);
            MoveButtonUp(_leftMoveButton);
        }

        else
        {
            MoveButtonUp(_leftMoveButton);
            MoveButtonUp(_rightMoveButton);
        }
    }
}
