using System;
using UnityEngine;

public class Overlap : MonoBehaviour
{
    public float _radius = 0.5f;
    public float _maxDistance = 0.1f; // 정면으로 체크할 최대 거리
    public LayerMask layermask;
    private Collider _short_Obj;


    private GameObject prevSelectedGameObject;
    public GameObject SelectGameObject;

    private GameObject selectedObject;
    private Color originalColor;

    public static Action<GameObject> ObjectSelectEnter;


    void Update()
    {
        // 플레이어의 위치와 방향 계산
        Vector3 playerPosition = transform.position;
        Vector3 playerForward = transform.forward;

        // 정면 방향으로 일정 거리만큼 이동한 위치 계산
        //Vector3 checkPosition = playerPosition + playerForward * _maxDistance;

        RaycastHit hit; // 충돌 정보를 저장할 변수

        // 정면 방향에서 SphereCast로 콜라이더 검출

        if (Physics.SphereCast(playerPosition, _radius, playerForward, out hit, _maxDistance, layermask))
        {

            _short_Obj = hit.collider; // 충돌한 콜라이더를 Short_Obj에 할당
            SelectGameObject = _short_Obj.gameObject;

            RestoreObjectColor();

            // 충돌한 객체의 Renderer 가져오기
            MeshRenderer objRenderer = SelectGameObject.GetComponent<MeshRenderer>();

            selectedObject = SelectGameObject;

            originalColor = objRenderer.material.color;

            objRenderer.material.SetColor("_EmissionColor", new Color(0.5f, 0.45f, 0.4f, 0f));
            objRenderer.material.EnableKeyword("_EMISSION");
           

            if (prevSelectedGameObject != SelectGameObject)
            {
                ObjectSelectEnter.Invoke(SelectGameObject);
                prevSelectedGameObject = SelectGameObject;
            }
        }
        else
        {

            RestoreObjectColor();

            _short_Obj = null; // 충돌한 콜라이더가 없으면 Short_Obj를 null로 설정
            SelectGameObject = null;
        }
    }

  
    private void RestoreObjectColor()
    {
        if (selectedObject != null)
        {
            MeshRenderer objRenderer = selectedObject.GetComponent<MeshRenderer>();
            if (objRenderer != null && objRenderer.material != null)
            {
                // 원래 색상으로 복구
                objRenderer.material.SetColor("_EmissionColor", originalColor);
                objRenderer.material.DisableKeyword("_EMISSION");
            }
            // 선택된 객체 초기화
            selectedObject = null;
        }
    }

    // 정면 방향으로 체크되는 범위를 시각적으로 표시하기 위한 gizmo
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position + transform.forward * _maxDistance, _radius);
    }

}