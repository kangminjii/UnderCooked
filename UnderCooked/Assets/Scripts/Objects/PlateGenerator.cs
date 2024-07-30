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
     * ���ø� �����ϴ� �繰 - PlateGenerator
     * -> ���� ���۽� ���� ��ȯ
     * -> Player�� Observer�� ��� �� ��ü�� ���� ��ȭ�� ���� ����
     */
    private void Awake()
    {
        for(int i = 0; i < _maxPlateNumber; i++)
            StartCoroutine(SpawnPlate());

        Player.PlateGenerate += HandlePlateGenerator;
        Player.PlateDestroy += HandlePlateDestroy;
    }


    /*
     * ������Ʈ ����� ���� ����
     */
    private void OnDestroy()
    {
        Player.PlateGenerate -= HandlePlateGenerator;
        Player.PlateDestroy -= HandlePlateDestroy;
    }


    /*
     * GameObject "Plate"�� ������ ������ �ҷ����� �Լ�
     * -> PlateList ����
     * -> ���ο� Plate �����ϴ� �ڷ�ƾ ȣ��
     */
    public void HandlePlateGenerator()
    {
        _plateList.RemoveAt(_plateList.Count - 1);
        StartCoroutine(SpawnPlate());
    }


    /*
     * ���� ������ ���� ���ð� �����Ǵ� �ڷ�ƾ
     * -> PlateSpawnPos���� ���ð����� Ȯ���� ��, ��ġ�� ���� ���ø� Instantiate��
     * -> PlateList ����
     * -> ȿ���� ���
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
     * Player�� ���ø� ������ �� ���ø� �ı��ϴ� �Լ�
     */
    public void HandlePlateDestroy()
    {
        Managers.Resource.Destroy(_plateSpawnPos.GetChild(_plateSpawnPos.childCount - 1).gameObject);
    }
}
