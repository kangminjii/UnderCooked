using UnityEngine;

public class Overlap : MonoBehaviour
{
    public float radius = 1f;
    public float maxDistance = 5f; // �������� üũ�� �ִ� �Ÿ�
    public LayerMask layermask;
    public Collider Short_Obj;

    private Color originalColor;

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
            Renderer objRenderer = Short_Obj.GetComponent<Renderer>();
        }
        else
        {
            Short_Obj = null; // �浹�� �ݶ��̴��� ������ Short_Obj�� null�� ����
        }
    }

    // ���� �������� üũ�Ǵ� ������ �ð������� ǥ���ϱ� ���� gizmo
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position + transform.forward * maxDistance, radius);
    }
}