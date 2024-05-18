using UnityEngine;

public delegate void ObjectSelectHandler(GameObject obj);

public class Seeking : MonoBehaviour
{
    float        _radius = 0.41f;
    float        _maxDistance = 0.7f;
    Color        _originalColor;
    Collider     _collidedObject;
    GameObject   _selectedObject;


    public LayerMask                 Layermask;
    public GameObject                SeekedGameObject;
    public event ObjectSelectHandler Seek;


    /*
     * 이벤트에 등록된 모든 delegate를 호출한다
     */
    protected virtual void OnSeek(GameObject obj)
    {
        if (Seek != null)
            Seek.Invoke(obj);
    }


    /*
     * RayCast를 사용하여 Player와 충돌된 물체를 저장
     * 
     * 충돌된 물체를 알리기 위한 색 변환 작업
     * -> 새로운 충돌 물체의 색을 변화시키기 위해 변화된 물체는 색을 복구함
     * -> MeshRenderer의 Emission을 활성화시켜 색을 바꿈
     * -> 원본 색상을 저장하여 충돌되지 않을 때 색을 복구할 수 있게함
     * 
     * 충돌된 물체를 Player에게 전달하는 OnSeek 이벤트를 호출
     * 
     * 고민: Update가 아닌 충돌 물체가 바뀔때만 호출되게 만드려면? 
     */
    void Update()
    {
        Vector3 playerPosition = transform.position;
        Vector3 playerForward = transform.forward;
        RaycastHit hit;

        if (Physics.SphereCast(playerPosition, _radius, playerForward, out hit, _maxDistance, Layermask))
        {
            _collidedObject = hit.collider;
            SeekedGameObject = _collidedObject.gameObject;

            RestoreObjectColor();

            MeshRenderer objRenderer = SeekedGameObject.GetComponent<MeshRenderer>();

            objRenderer.material.SetColor("_EmissionColor", new Color(0.5f, 0.45f, 0.4f, 0f));
            objRenderer.material.EnableKeyword("_EMISSION");
            _originalColor = objRenderer.material.color;

            _selectedObject = SeekedGameObject;
        }
        else
        {
            _collidedObject = null;
            SeekedGameObject = null;

            RestoreObjectColor();
        }

        OnSeek(SeekedGameObject);
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