using System;
using UnityEngine;

public class Overlap : MonoBehaviour
{
    float _radius = 0.41f;
    float _maxDistance = 0.7f; // �������� üũ�� �ִ� �Ÿ�
    
    Collider _collidedObject;
    GameObject _selectedObject;
    Color _originalColor;


    public LayerMask Layermask;
    public GameObject OverlappedGameObject;
    public static Action<GameObject> ObjectSelectEnter;


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

            // �浹�� ��ü�� Renderer ��������
            MeshRenderer objRenderer = OverlappedGameObject.GetComponent<MeshRenderer>();
            objRenderer.material.SetColor("_EmissionColor", new Color(0.5f, 0.45f, 0.4f, 0f));
            objRenderer.material.EnableKeyword("_EMISSION");
            _originalColor = objRenderer.material.color;

            _selectedObject = OverlappedGameObject;

            ObjectSelectEnter.Invoke(OverlappedGameObject);
        }
        else
        {
            _collidedObject = null; 
            OverlappedGameObject = null;

            RestoreObjectColor();
            ObjectSelectEnter.Invoke(OverlappedGameObject);
        }
    }


    private void RestoreObjectColor()
    {
        if (_selectedObject != null)
        {
            MeshRenderer objRenderer = _selectedObject.GetComponent<MeshRenderer>();
            if (objRenderer != null && objRenderer.material != null)
            {
                // ���� �������� ����
                objRenderer.material.SetColor("_EmissionColor", _originalColor);
                objRenderer.material.DisableKeyword("_EMISSION");
            }

            _selectedObject = null;
        }
    }

}