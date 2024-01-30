using UnityEngine;

public class Overlap : MonoBehaviour
{
    public float radius = 0.5f;
    public float maxDistance = 1f; // �������� üũ�� �ִ� �Ÿ�
    public LayerMask layermask;
    public Collider Short_Obj;

    private Color originalColor;

    // table�� �� ��ȭ
    private Material _commonMaterial;
    private Material _instanceMaterial;

    void Update()
    {
        // �÷��̾��� ��ġ�� ���� ���
        Vector3 playerPosition = transform.position;
        Vector3 playerForward = transform.forward;

        // ���� �������� ���� �Ÿ���ŭ �̵��� ��ġ ���
        Vector3 checkPosition = playerPosition + playerForward * maxDistance;

        RaycastHit hit; // �浹 ������ ������ ����

        // ���� ���⿡�� SphereCast�� �ݶ��̴� ����
        if (Physics.SphereCast(playerPosition, radius, playerForward, out hit, maxDistance, layermask))
        {
            Short_Obj = hit.collider; // �浹�� �ݶ��̴��� Short_Obj�� �Ҵ�

            // �浹�� ��ü�� Renderer ��������
            MeshRenderer objRenderer = Short_Obj.GetComponent<MeshRenderer>();
            _commonMaterial = objRenderer.material;
            _instanceMaterial = Instantiate(_commonMaterial);
            
            Color newEmissionColor = new Color(0.5f, 0.45f, 0.4f, 0f);
            _instanceMaterial.SetColor("_EmissionColor", newEmissionColor);
            _instanceMaterial.EnableKeyword("_EMISSION");
        }
        else
        {
            Short_Obj = null; // �浹�� �ݶ��̴��� ������ Short_Obj�� null�� ����
            //if(_instanceMaterial != null)
            //    _instanceMaterial.DisableKeyword("_EMISSION");
        }
    }

    // ���� �������� üũ�Ǵ� ������ �ð������� ǥ���ϱ� ���� gizmo
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position + transform.forward * maxDistance, radius);
    }
 
}