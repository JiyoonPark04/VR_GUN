using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    //live time of projectile
    public float m_Lifetime = 5.0f;

    private Rigidbody m_Rigidbody = null;
    //물리 시뮬레이션을 통해서 오브젝트의 위치를 조절합니다.
    //리지드바디(rigidbody) 컴포넌트는 오브젝트의 위치를 제어합니다.

    private void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        SetInnactive();
    }

    private void OnCollisionEnter(Collision collision)
    // OnCollisionEnter-> 이collider/rigidbody에 다른 collider/rigidbody가 닿을 때 호출됩니다.
    {
        SetInnactive();
    }

    public void Launch(Blaster blaster)
    {
        //Position
        transform.position = blaster.m_Barrel.position;
        transform.rotation = blaster.m_Barrel.rotation;

        //Activate
        gameObject.SetActive(true);
         //public void SetActive(bool value);->게임 개체를 활성화/비활성화합니다

        //Fire,and track
        m_Rigidbody.AddRelativeForce(Vector3.forward * blaster.m_Force, ForceMode.Impulse);
        //Rigidbody.AddRelativeForce(Vector3 force, ForceMode mode)
        //상대좌표계 시스템에 Rigidbody로 힘을 추가합니다.
        // ForceMode->Rigidbody.AddForce를 사용해서 힘을 적용하는 방법에 대한 Option을 나타냅니다.
        //Force->해당 리지드바디의 질량을 사용해서 연속적인 힘을 가하는 경우
        //Acceleration->질량을 무시하고 리지드바디에 연속적인 가속력을 가한다
        //Impulse-리지드바디의 질량을 사용해서 짧은 순간의 힘을 가하는 경우에 사용한다
        //VelocityChange->질량을 무시하고 리지드바디에 속도 변화를 짧은 순간에 적용할 경우 사용한다
        StartCoroutine(TrackLifetime());
       //코루틴을 시작합니다.
    }

    private IEnumerator TrackLifetime()
    {
        yield return new WaitForSeconds(m_Lifetime);
        SetInnactive();
    }

    //reset the valocity 
    public void SetInnactive()
    {
        m_Rigidbody.velocity = Vector3.zero;
        m_Rigidbody.angularVelocity = Vector3.zero;
        //Rigidbody.angularVelocity->리비드바디(rigidbody)의 각속도 벡터를 나타냅니다

        gameObject.SetActive(false);
    }
}
