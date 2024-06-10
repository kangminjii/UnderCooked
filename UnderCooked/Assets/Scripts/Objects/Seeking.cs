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
     * RayCast�� ����Ͽ� Player�� �浹�� ��ü�� ����
     * 
     * �浹�� ��ü�� �˸��� ���� �� ��ȯ �۾�
     * -> ���ο� �浹 ��ü�� ���� ��ȭ��Ű�� ���� ��ȭ�� ��ü�� ���� ������
     * -> MeshRenderer�� Emission�� Ȱ��ȭ���� ���� �ٲ�
     * -> ���� ������ �����Ͽ� �浹���� ���� �� ���� ������ �� �ְ���
     * 
     * �浹�� ��ü�� Player���� ����
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