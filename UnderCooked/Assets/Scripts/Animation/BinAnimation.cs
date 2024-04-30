using UnityEngine;

public class BinAnimation : MonoBehaviour
{
    float _scaleFactor = 0.001f;

    private void Update()
    {
        if(transform.parent.name == "BinSpawnPos")
        {
            float scale = Mathf.Lerp(1f, _scaleFactor, Time.time);
            Vector3 newScale = new Vector3(scale, scale, scale );
            transform.localScale = newScale;
        }
    }

    // Animation ¿Ã∫•∆Æ
    public void DeleteObject()
    {
        Destroy(this.gameObject);
    }
}
