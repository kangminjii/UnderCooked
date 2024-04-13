using System;
using UnityEngine;

public class Overlap : MonoBehaviour
{
    float _radius = 0.41f;
    float _maxDistance = 0.7f; // �������� üũ�� �ִ� �Ÿ�
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

            // �浹�� ��ü�� Renderer ��������
            MeshRenderer objRenderer = SelectGameObject.GetComponent<MeshRenderer>();

            _selectedObject = SelectGameObject;
            
            _originalColor = objRenderer.material.color;

            objRenderer.material.SetColor("_EmissionColor", new Color(0.5f, 0.45f, 0.4f, 0f));
            objRenderer.material.EnableKeyword("_EMISSION");

            ObjectSelectEnter.Invoke(SelectGameObject);
        }

        else
        {
            _short_Obj = null; // �浹�� �ݶ��̴��� ������ Short_Obj�� null�� ����
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
                // ���� �������� ����
                objRenderer.material.SetColor("_EmissionColor", _originalColor);
                objRenderer.material.DisableKeyword("_EMISSION");
            }
            // ���õ� ��ü �ʱ�ȭ
            _selectedObject = null;
        }
    }

    

}