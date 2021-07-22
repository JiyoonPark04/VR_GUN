using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR;

public class Blaster : MonoBehaviour
//MonoBehaviour->유니티의 모든 스크립트가 상속받는 클래스
{
    //인스펙터의 일부 필드에 헤더를 추가해주는데 사용
    [Header("Input")] //Header->인스펙터 내에 타이틀을 달 수 있는 속성
    public SteamVR_Action_Boolean m_FireAction = null;
    public SteamVR_Action_Boolean m_ReloadAction = null;

    [Header("Settings")]
    public int m_Force = 10;
    public int m_MaxProjectileCount = 6;
    public float m_ReloadTime = 1.5f;

    [Header("References")]
    public Transform m_Barrel = null;
    public GameObject m_ProjectilePrefab = null;
    public Text m_AmmoOutput = null;

    private bool m_IsReloading = false;
    private int m_FireCount = 0;

    private SteamVR_Behaviour_Pose m_Pose = null;
    private Animator m_Animator;
    private ProjectilePool m_projectilePool = null;

    private void Awake()
        //Awake->프리팹이 인스턴스화 한 직후, 스크립트가 호출되자마자 오브젝트 활성화 여부와
        //상관없이 실행 된다. 모든 오브젝트가 초기화 된 후 호출되기 때문에GameObject.Find 같은 명령문을 안전하게 사용할 수 있다.
        //Awake함수는 언제나 Start함수 전에 호출되는 점 주의
    {

        // pose
        m_Pose = GetComponentInParent<SteamVR_Behaviour_Pose>();
        //GetComponentInParent->컴포넌트 형으로 부모 오브젝트를 검색해, 가장 처음에 나타난 부모 오브젝으를 반환한다.
        //GetComponentsInParent->컴포넌트 형으로 부모오브젝트를 검색해서 나타난 부모 오브젝트들의 배열을 반환한다.
        m_Animator = GetComponent<Animator>();
        //GetComponent<Type>();->게임오브젝트에서 컴포넌트를 가져오는 함수,Type는 컴포넌트의 타입

        m_projectilePool = new ProjectilePool(m_ProjectilePrefab, m_MaxProjectileCount); //??왜 getcomponent를 안쓰고 new를 사용해서 선언했는지 물어보기
    }
    private void Start()
        //Start->Update 함수가 호출되기 전에 한번만 호출된다.
    {
        UpdateFiredCount(0);
    }

    private void Update()
        //매 프레임마다 호출되는 가장 일반적인 Update 함수.
        //오브젝트가 활성화 되어 있어야 호출 되며 프레임 단위이기에 호출 시간 간격은 프레임에 따라 변한다.
    {
        if (m_IsReloading)
            return;

        if (m_FireAction.GetStateDown(m_Pose.inputSource))
            //액션이름.GetStateDown(포즈.inputsource)
        { 
            m_Animator.SetBool("Fire", true);
            //void.애니메이터.SetBool(string name, bool value);
            Fire();
        }

        if (m_FireAction.GetStateUp(m_Pose.inputSource))
        {
            m_Animator.SetBool("Fire", false);
        }

        if (m_ReloadAction.GetStateDown(m_Pose.inputSource))
            StartCoroutine(Reload());
            //StartCoroutine->코루틴, 실행을 중지하여 유니티에 제어권을 돌려주고, 그러나
            //계속할 때는 다음 프레임에서 중지한 곳부터 실행을 계속할 수 있는 기능.
    }
    private void Fire()
    {
        if (m_FireCount >= m_MaxProjectileCount)
            return;

        Projectile targetProjectile = m_projectilePool.m_projectile[m_FireCount];
        targetProjectile.Launch(this);//??물어보기->그럼 this 는 blaster를 의미?

        UpdateFiredCount(m_FireCount + 1);
    }

    private IEnumerator Reload()
    //IEnumerable 콜렉션은 foreach 구문 등에서 개체를 한개한개 넘겨주는 일을 한다.(하나의 구문의 번수만 기억함)
    //IEnumerator 는 여러개의 구문을 한꺼번에 수행가능(여러 구문의 넘겨주는 물건의 번수를 다 기억함)
    {
        if (m_FireCount == 0)
            yield break;
        //yield break->해당 반복을 모두 마친다

        m_AmmoOutput.text = "-";
        m_IsReloading = true;

        m_projectilePool.SetAllProjectiles();

        yield return new WaitForSeconds(m_ReloadTime);
        //yield return-> 이걸로 빠져나온 구문은 현재 그 상태를 기억한다(했던 자리에서 다시 시작된다)

        UpdateFiredCount(0);
        m_IsReloading = false;
    }

    private void UpdateFiredCount(int newValue)
    {
        m_FireCount = newValue;
        m_AmmoOutput.text = (m_MaxProjectileCount - m_FireCount).ToString();
    }
}
