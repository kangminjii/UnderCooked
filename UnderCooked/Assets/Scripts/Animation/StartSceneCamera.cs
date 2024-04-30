using System.Collections;
using UnityEngine;

public class StartSceneCamera : MonoBehaviour
{
    Vector3 finalPos = new Vector3(-1.57f, 4.17f, -9.05f);
    static Vector3 finalAngle = new Vector3(0, 7.83f, 0);
    Quaternion finalRotation = Quaternion.Euler(finalAngle);


    public IEnumerator CameraAnimation()
    {
        while (transform.position.x > finalPos.x || transform.position.y > finalPos.y || transform.position.z > finalPos.z)
        {
            transform.position = Vector3.Lerp(transform.position, finalPos, 3f * Time.deltaTime);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, finalRotation, 5f * Time.deltaTime);
            
            yield return null;
        }
    }
}
