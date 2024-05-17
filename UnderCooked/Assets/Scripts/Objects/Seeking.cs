using UnityEngine;

public delegate void ObjectSelectHandler(GameObject obj);

public class Seeking : MonoBehaviour
{
    float       _radius = 0.41f;
    float       _maxDistance = 0.7f;
    Collider    _collidedObject;
    GameObject  _selectedObject;
    Color       _originalColor;


    public LayerMask    Layermask;
    public GameObject   OverlappedGameObject;
    public event ObjectSelectHandler OverlapHandler;


    protected virtual void Overlapped(GameObject obj)
    {
        if (OverlapHandler != null)
            OverlapHandler.Invoke(obj);
    }


    void Update()
    {
        Vector3 playerPosition = transform.position;
        Vector3 playerForward = transform.forward;
        RaycastHit hit;

        if (Physics.SphereCast(playerPosition, _radius, playerForward, out hit, _maxDistance, Layermask))
        {
            _collidedObject = hit.collider;
            OverlappedGameObject = _collidedObject.gameObject;

            RestoreObjectColor();

            // 충돌한 객체의 Renderer 가져오기
            MeshRenderer objRenderer = OverlappedGameObject.GetComponent<MeshRenderer>();
            objRenderer.material.SetColor("_EmissionColor", new Color(0.5f, 0.45f, 0.4f, 0f));
            objRenderer.material.EnableKeyword("_EMISSION");
            _originalColor = objRenderer.material.color;

            _selectedObject = OverlappedGameObject;

            Overlapped(OverlappedGameObject);
        }
        else
        {
            _collidedObject = null; 
            OverlappedGameObject = null;

            RestoreObjectColor();

            Overlapped(OverlappedGameObject);
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

            _selectedObject = null;
        }
    }

}