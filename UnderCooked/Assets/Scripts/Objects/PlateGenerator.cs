using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlateGenerator : MonoBehaviour
{
    float            _plateSpawnTime = 1.0f;
    string           _plateName = "Plate";
    int              _maxPlateNumber = 3;
    List<GameObject> _plateList = new List<GameObject>();
    
    [SerializeField]
    Transform        _plateSpawnPos;


    /*
     * 접시를 생성하는 사물 - PlateGenerator
     * -> 게임 시작시 접시 소환
     * -> Player를 Observer로 등록 후 주체의 상태 변화에 따라 갱신
     */
    private void Awake()
    {
        for(int i = 0; i < _maxPlateNumber; i++)
            StartCoroutine(SpawnPlate());

        Player.PlateGenerate += HandlePlateGenerator;
        Player.PlateDestroy += HandlePlateDestroy;
    }


    /*
     * 프로젝트 종료시 구독 해제
     */
    private void OnDestroy()
    {
        Player.PlateGenerate -= HandlePlateGenerator;
        Player.PlateDestroy -= HandlePlateDestroy;
    }


    /*
     * GameObject "Plate"가 없어질 때마다 불러지는 함수
     * -> PlateList 갱신
     * -> 새로운 Plate 생성하는 코루틴 호출
     */
    public void HandlePlateGenerator()
    {
        _plateList.RemoveAt(_plateList.Count - 1);
        StartCoroutine(SpawnPlate());
    }


    /*
     * 접시 생성대 위에 접시가 생성되는 코루틴
     * -> PlateSpawnPos에서 접시개수를 확인한 후, 위치에 따라 접시를 Instantiate함
     * -> PlateList 갱신
     * -> 효과음 재생
     */
    public IEnumerator SpawnPlate()
    {
        yield return new WaitForSeconds(_plateSpawnTime);

        Vector3 spwanPlatePos = _plateSpawnPos.position + new Vector3(0, (_plateSpawnPos.childCount - 1) * 0.05f, 0);
        GameObject plate = Managers.Resource.Instantiate(_plateName, spwanPlatePos, Quaternion.identity, _plateSpawnPos);
        _plateList.Add(plate);

        Managers.Sound.Play("Effect/Game/WashedPlate", Define.Sound.Effect);
    }


    /*
     * Player가 접시를 가져갈 때 접시를 파괴하는 함수
     */
    public void HandlePlateDestroy()
    {
        Managers.Resource.Destroy(_plateSpawnPos.GetChild(_plateSpawnPos.childCount - 1).gameObject);
    }
}
