using UnityEngine;

public class TextTT : MonoBehaviour
{
    [SerializeField] private GameObject objectToActivate1;
    [SerializeField] private GameObject objectToActivate2;
    [SerializeField] private GameObject objectToActivate3;
    [SerializeField] private GameObject objectToActivate4;
    [SerializeField] private GameObject objectToActivate5;
    [SerializeField] private GameObject objectToActivate6;
    
    void Start()
    {
        Invoke("ActivateObject1", 11f);
        Invoke("DestroyObject1", 15f);
        
        Invoke("ActivateObject2", 20f);
        Invoke("DestroyObject2", 24f);
        
        Invoke("ActivateObject3", 28f);
        Invoke("DestroyObject3", 33f);
        
        Invoke("ActivateObject4", 38f);
        Invoke("DestroyObject4", 42f);
        
        Invoke("ActivateObject5", 46f);
        Invoke("DestroyObject5", 54f);
        
        Invoke("ActivateObject6", 61f);
        Invoke("DestroyObject6", 67f);
        
        Invoke("DestroyObject", 70f);
    }

    void DestroyObject1()
    {
        Destroy(objectToActivate1);
    }
    
    void ActivateObject1()
    {
        objectToActivate1.SetActive(true);
    }
    
    // 오브젝트 2의 활성화 및 파괴 메서드
    void ActivateObject2()
    {
        objectToActivate2.SetActive(true);
    }
    
    void DestroyObject2()
    {
        Destroy(objectToActivate2);
    }
    
    // 오브젝트 3의 활성화 및 파괴 메서드
    void ActivateObject3()
    {
        objectToActivate3.SetActive(true);
    }
    
    void DestroyObject3()
    {
        Destroy(objectToActivate3);
    }
    
    // 오브젝트 4의 활성화 및 파괴 메서드
    void ActivateObject4()
    {
        objectToActivate4.SetActive(true);
    }
    
    void DestroyObject4()
    {
        Destroy(objectToActivate4);
    }
    
    // 오브젝트 5의 활성화 및 파괴 메서드
    void ActivateObject5()
    {
        objectToActivate5.SetActive(true);
    }
    
    void DestroyObject5()
    {
        Destroy(objectToActivate5);
    }
    
    // 오브젝트 6의 활성화 및 파괴 메서드
    void ActivateObject6()
    {
        objectToActivate6.SetActive(true);
    }
    
    void DestroyObject6()
    {
        Destroy(objectToActivate6);
    }
    
    void DestroyObject()
    {
        Destroy(gameObject);
    }

}

