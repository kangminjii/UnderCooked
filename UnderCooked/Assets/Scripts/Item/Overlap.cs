using UnityEngine;

public class Overlap : MonoBehaviour
{
    public float radius = 0.5f;
    public float maxDistance = 1f; // 정면으로 체크할 최대 거리
    public LayerMask layermask;
    public Collider Short_Obj;

    private Color originalColor;

    // table의 색 변화
    private Material _commonMaterial;
    private Material _instanceMaterial;

    void Update()
    {
        // 플레이어의 위치와 방향 계산
        Vector3 playerPosition = transform.position;
        Vector3 playerForward = transform.forward;

        // 정면 방향으로 일정 거리만큼 이동한 위치 계산
        Vector3 checkPosition = playerPosition + playerForward * maxDistance;

        RaycastHit hit; // 충돌 정보를 저장할 변수

        // 정면 방향에서 SphereCast로 콜라이더 검출
        if (Physics.SphereCast(playerPosition, radius, playerForward, out hit, maxDistance, layermask))
        {
            Short_Obj = hit.collider; // 충돌한 콜라이더를 Short_Obj에 할당

            // 충돌한 객체의 Renderer 가져오기
            MeshRenderer objRenderer = Short_Obj.GetComponent<MeshRenderer>();
            _commonMaterial = objRenderer.material;
            _instanceMaterial = Instantiate(_commonMaterial);
            
            Color newEmissionColor = new Color(0.5f, 0.45f, 0.4f, 0f);
            _instanceMaterial.SetColor("_EmissionColor", newEmissionColor);
            _instanceMaterial.EnableKeyword("_EMISSION");
        }
        else
        {
            Short_Obj = null; // 충돌한 콜라이더가 없으면 Short_Obj를 null로 설정
            //if(_instanceMaterial != null)
            //    _instanceMaterial.DisableKeyword("_EMISSION");
        }
    }

    // 정면 방향으로 체크되는 범위를 시각적으로 표시하기 위한 gizmo
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position + transform.forward * maxDistance, radius);
    }
 
}