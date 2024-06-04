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
     * ���ø� �����ϴ� �繰 - PlateGenerator
     * -> ���� ���۽� ���� ��ȯ
     * -> Player�� Observer�� ��� �� HandlePlateGenerator() �Լ� ����
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
        PlateList.RemoveAt(PlateList.Count - 1);
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
        PlateList.Add(plate);

        Managers.Sound.Play("AudioClip/WashedPlate", Define.Sound.Effect);
    }


    /*
     * ���ð� �ı� �� �� ȣ��Ǵ� �̺�Ʈ
     * 
     */
    public void HandlePlateDestroy()
    {
        Managers.Resource.Destroy(_plateSpawnPos.GetChild(_plateSpawnPos.childCount - 1).gameObject);
    }

}
