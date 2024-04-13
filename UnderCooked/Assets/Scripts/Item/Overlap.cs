using System;
using UnityEngine;

public class Overlap : MonoBehaviour
{
    float _radius = 0.41f;
    float _maxDistance = 0.7f; // 정면으로 체크할 최대 거리
    Collider _short_Obj;
    GameObject _selectedObject;
    Color _originalColor;

    public LayerMask Layermask;
    public GameObject SelectGameObject;
    public static Action<GameObject> ObjectSelectEnter;


    void Update()
    {
        Vector3 playerPosition = transform.position;
        Vector3 playerForward = transform.forward;
        RaycastHit hit;

        if (Physics.SphereCast(playerPosition, _radius, playerForward, out hit, _maxDistance, Layermask))
        {
            _short_Obj = hit.collider;
            SelectGameObject = _short_Obj.gameObject;

            RestoreObjectColor();

            // 충돌한 객체의 Renderer 가져오기
            MeshRenderer objRenderer = SelectGameObject.GetComponent<MeshRenderer>();

            _selectedObject = SelectGameObject;
            
            _originalColor = objRenderer.material.color;

            objRenderer.material.SetColor("_EmissionColor", new Color(0.5f, 0.45f, 0.4f, 0f));
            objRenderer.material.EnableKeyword("_EMISSION");

            ObjectSelectEnter.Invoke(SelectGameObject);
        }

        else
        {
            _short_Obj = null; // 충돌한 콜라이더가 없으면 Short_Obj를 null로 설정
            SelectGameObject = null;

            RestoreObjectColor();
            ObjectSelectEnter.Invoke(SelectGameObject);
        }
    }


    private void RestoreObjectColor()
    {
        if (_selectedObject != null)
        {
            MeshRenderer objRenderer = _selectedObject.GetComponent<MeshRenderer>();
            if (objRenderer != null && objRenderer.material != null)
            {
                // 원래 색상으로 복구
                objRenderer.material.SetColor("_EmissionColor", _originalColor);
                objRenderer.material.DisableKeyword("_EMISSION");
            }
            // 선택된 객체 초기화
            _selectedObject = null;
        }
    }

    

}