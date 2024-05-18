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
     * �̺�Ʈ�� ��ϵ� ��� delegate�� ȣ���Ѵ�
     */
    protected virtual void OnSeek(GameObject obj)
    {
        if (Seek != null)
            Seek.Invoke(obj);
    }


    /*
     * RayCast�� ����Ͽ� Player�� �浹�� ��ü�� ����
     * 
     * �浹�� ��ü�� �˸��� ���� �� ��ȯ �۾�
     * -> ���ο� �浹 ��ü�� ���� ��ȭ��Ű�� ���� ��ȭ�� ��ü�� ���� ������
     * -> MeshRenderer�� Emission�� Ȱ��ȭ���� ���� �ٲ�
     * -> ���� ������ �����Ͽ� �浹���� ���� �� ���� ������ �� �ְ���
     * 
     * �浹�� ��ü�� Player���� �����ϴ� OnSeek �̺�Ʈ�� ȣ��
     * 
     * ���: Update�� �ƴ� �浹 ��ü�� �ٲ𶧸� ȣ��ǰ� �������? 
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


    /* �浹�� ��ü�� ���� �������� �������ִ� �Լ�
     * -> MeshRenderer�� Emission�� ��Ȱ��ȭ��Ű�� ���� ������ ������
     * -> ������ Object�� null ó��
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