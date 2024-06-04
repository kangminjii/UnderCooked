using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlateGenerator : MonoBehaviour
{
    float   _plateSpawnTime = 1.0f;
    int     _maxPlateNumber = 3;
    string  _plateName = "Plate";


    [SerializeField]
    Transform               _plateSpawnPos;
    public List<GameObject> PlateList = new List<GameObject>();


    /*
     * 접시를 생성하는 사물 - PlateGenerator
     * -> 게임 시작시 접시 소환
     * -> Player를 Observer로 등록 후 HandlePlateGenerator() 함수 구독
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
        PlateList.RemoveAt(PlateList.Count - 1);
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
        PlateList.Add(plate);

        Managers.Sound.Play("AudioClip/WashedPlate", Define.Sound.Effect);
    }


    /*
     * 접시가 파괴 될 때 호출되는 이벤트
     * 
     */
    public void HandlePlateDestroy()
    {
        Managers.Resource.Destroy(_plateSpawnPos.GetChild(_plateSpawnPos.childCount - 1).gameObject);
    }

}
