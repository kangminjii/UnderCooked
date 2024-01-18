using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Decorator Pattern
// 객체에 추가적인 요건을 동적으로 첨가하는 것
// 서브 클래스를 만드는 것을 통해 기능을 유연하게 확장한다.
// > 게임 구현에 자주 쓰이지 않는다.

// 컴포넌트 기반 아키텍처 또는 인터페이스 활용을 주로 함




public class Searching : MonoBehaviour
{

    private Material commonMaterial;
    private Material instanceMaterial;

    private void Start()
    {
        commonMaterial = GetComponent<MeshRenderer>().material;
        instanceMaterial = Instantiate(commonMaterial);
        GetComponent<MeshRenderer>().material = instanceMaterial;
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            instance
        }
    }



}
