using System;
using UnityEngine;

public class Overlap : MonoBehaviour
{
    public float _radius = 0.5f;
    public float _maxDistance = 0.1f; // �������� üũ�� �ִ� �Ÿ�
    public LayerMask layermask;
    private Collider _short_Obj;


    private GameObject prevSelectedGameObject;
    public GameObject SelectGameObject;

    private GameObject selectedObject;
    private Color originalColor;

    public static Action<GameObject> ObjectSelectEnter;


    void Update()
    {
        // �÷��̾��� ��ġ�� ���� ���
        Vector3 playerPosition = transform.position;
        Vector3 playerForward = transform.forward;

        // ���� �������� ���� �Ÿ���ŭ �̵��� ��ġ ���
        //Vector3 checkPosition = playerPosition + playerForward * _maxDistance;

        RaycastHit hit; // �浹 ������ ������ ����

        // ���� ���⿡�� SphereCast�� �ݶ��̴� ����

        if (Physics.SphereCast(playerPosition, _radius, playerForward, out hit, _maxDistance, layermask))
        {

            _short_Obj = hit.collider; // �浹�� �ݶ��̴��� Short_Obj�� �Ҵ�
            SelectGameObject = _short_Obj.gameObject;

            RestoreObjectColor();

            // �浹�� ��ü�� Renderer ��������
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

            _short_Obj = null; // �浹�� �ݶ��̴��� ������ Short_Obj�� null�� ����
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
                // ���� �������� ����
                objRenderer.material.SetColor("_EmissionColor", originalColor);
                objRenderer.material.DisableKeyword("_EMISSION");
            }
            // ���õ� ��ü �ʱ�ȭ
            selectedObject = null;
        }
    }

    // ���� �������� üũ�Ǵ� ������ �ð������� ǥ���ϱ� ���� gizmo
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position + transform.forward * _maxDistance, _radius);
    }

}