using UnityEngine;

public class CloudMove : MonoBehaviour
{
    public GameObject CloudPrefab; 
    
    GameObject _nextCloud;


    void Start()
    {
        _nextCloud = gameObject;
        _nextCloud.transform.position = new Vector3(51,22, 7);
    }

    void Update()
    {
        if (transform.position.x < -40)
            Destroy(this.gameObject);

        transform.Translate(Vector3.left * Time.deltaTime);

        if (_nextCloud.transform.position.x < -8)
        {
            Vector3 rightEndPosition = transform.TransformPoint(Vector3.right * 0.5f);
            _nextCloud = Instantiate(CloudPrefab, rightEndPosition, Quaternion.identity);
        }
    }

}

