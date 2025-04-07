using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OXMiniGame : MonoBehaviour
{
    public GameObject[] cylinders; // 9개의 실린더
    public Text goalText; // 목표 O, X를 표시할 텍스트
    public GameObject clearPanel; // 클리어 시 표시할 패널
    public float rotationSpeed = 500f; // 실린더 회전 속도

    private string goalState; // 목표 상태 (O 또는 X)
    private bool[] isOStates; // 각 실린더의 현재 상태 저장 (true = O, false = X)

    void Start()
    {
        // 게임 시작 시 초기화
        isOStates = new bool[cylinders.Length];
        SetRandomStates();
        SetGoalState();
        clearPanel.SetActive(false); // 클리어 패널 숨김
    }

    void SetRandomStates()
    {
        // 실린더 상태를 랜덤으로 설정
        for (int i = 0; i < cylinders.Length; i++)
        {
            bool isO = Random.Range(0, 2) == 0;
            isOStates[i] = isO;
            SetCylinderRotation(cylinders[i], isO);
        }
    }

    void SetCylinderRotation(GameObject cylinder, bool isO)
    {
        // 실린더를 O 또는 X 방향으로 회전 설정하고 X축은 -90도로 고정
        float targetAngle = isO ? 0f : 180f;
        cylinder.transform.eulerAngles = new Vector3(-90, 0, targetAngle);
    }

    void SetGoalState()
    {
        // 목표 상태를 설정 (O 또는 X)
        goalState = Random.Range(0, 2) == 0 ? "O" : "X";
        goalText.text = "" + goalState;
    }

    public void RotateCylinder(GameObject clickedCylinder)
    {
        int index = System.Array.IndexOf(cylinders, clickedCylinder);
        if (index == -1) return; // 클릭된 실린더를 못 찾으면 종료

        // 상태 전환과 회전 시작
        StartCoroutine(RotateCylinderCoroutine(clickedCylinder, index));
    }

    private IEnumerator RotateCylinderCoroutine(GameObject cylinder, int index)
    {
        // 현재 상태 가져오기
        bool isO = isOStates[index];
        float startAngle = isO ? 0f : 180f;
        float endAngle = isO ? 180f : 0f;
        isOStates[index] = !isO;

        // 부드럽게 회전
        float elapsedTime = 0f;
        while (elapsedTime < 1f)
        {
            elapsedTime += Time.deltaTime * rotationSpeed / 180f; // 시간 기반 회전
            float angle = Mathf.Lerp(startAngle, endAngle, elapsedTime);
            cylinder.transform.eulerAngles = new Vector3(-90, 0, angle);
            yield return null;
        }

        // 정확한 각도로 설정
        cylinder.transform.eulerAngles = new Vector3(-90, 0, endAngle);

        // 게임 클리어 조건 확인
        CheckGameClear();
    }

    void CheckGameClear()
    {
        // 모든 실린더가 목표 상태와 같은지 확인
        for (int i = 0; i < cylinders.Length; i++)
        {
            if ((goalState == "O" && !isOStates[i]) || (goalState == "X" && isOStates[i]))
            {
                return; // 목표와 다른 상태가 있다면 종료
            }
        }

        // 클리어 조건 충족
        clearPanel.SetActive(true);
    }
}