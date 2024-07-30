using UnityEngine;


public class Seeking : MonoBehaviour
{
    float       _radius = 0.41f;
    float       _maxDistance = 0.7f;
    Color       _originalColor;
    Collider    _collidedObject;
    GameObject  _selectedObject;
    GameObject  _seekedGameObject;
    
    [SerializeField]
    LayerMask   _layermask;
    [SerializeField]
    Player      _player;


    /*
     * RayCast를 사용하여 Player와 충돌된 물체를 저장
     * 
     * 충돌된 물체를 알리기 위한 색 변환 작업
     * -> 새로운 충돌 물체의 색을 변화시키기 위해 변화된 물체는 색을 복구함
     * -> MeshRenderer의 Emission을 활성화시켜 색을 바꿈
     * -> 원본 색상을 저장하여 충돌되지 않을 때 색을 복구할 수 있게함
     * 
     * 충돌된 물체를 Player에게 전달
     */
    void Update()
    {
        Vector3 playerPosition = transform.position;
        Vector3 playerForward = transform.forward;
        RaycastHit hit;

        if (Physics.SphereCast(playerPosition, _radius, playerForward, out hit, _maxDistance, _layermask))
        {
            _collidedObject = hit.collider;
            _seekedGameObject = _collidedObject.gameObject;

            RestoreObjectColor();

            MeshRenderer objRenderer = _seekedGameObject.GetComponent<MeshRenderer>();

            objRenderer.material.SetColor("_EmissionColor", new Color(0.5f, 0.45f, 0.4f, 0f));
            objRenderer.material.EnableKeyword("_EMISSION");
            _originalColor = objRenderer.material.color;

            _selectedObject = _seekedGameObject;
        }
        else
        {
            _collidedObject = null;
            _seekedGameObject = null;

            RestoreObjectColor();
        }

        _player.Select(_seekedGameObject);
    }


    /* 충돌된 물체의 원래 색상으로 복구해주는 함수
     * -> MeshRenderer의 Emission을 비활성화시키며 원본 색으로 복구함
     * -> 복구된 Object는 null 처리
     */
    private void RestoreObjectColor()
    {
        if (_selectedObject != null)
        {
            MeshRenderer objRenderer = _selectedObject.GetComponent<MeshRenderer>();

            if (objRenderer != null)
            {
                objRenderer.material.SetColor("_EmissionColor", _originalColor);
                objRenderer.material.DisableKeyword("_EMISSION");
            }

            _selectedObject = null;
        }
    }

}