using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Target : MonoBehaviour
{
    public Color m_FlashDamageColor = Color.white;

    private MeshRenderer m_MeshRenderer = null;
    //MeshRenderer-> MeshFilter 또는 TextMesh에 의해 삽입되는 메쉬를 렌더링합니다.
    private Color m_OriginalColor = Color.white;

    private int m_MaxHealth = 3;
    private int m_Health = 0;
    public int score = 0;

    private void Awake()
    {
        m_MeshRenderer = GetComponent<MeshRenderer>();
        m_OriginalColor = m_MeshRenderer.material.color;
    }

    private void OnEnable()
        //활성화 될 때 마다 호출되는 함수(Awake/Start와 달리 활성화 될 때 마다)
    {
        ResetHealth();
    }

    private void OnCollisionEnter(Collision collision)
       // OnCollisionEnter-> 이collider/rigidbody에 다른 collider/rigidbody가 닿을 때 호출됩니다.
    {
        if (collision.gameObject.CompareTag("Projectile"))
            //public bool CompareTag(string tag);->게임 오브젝트에 tag 태그가 태깅되었는가?
            Damamge();
    }

    private void Damamge()
    {
        StopAllCoroutines();
        //현재 behaviour상에서 동작하는 모든 coroutine의 동작을 멈춥니다.
        StartCoroutine(Flash());
        //코루틴을 시작합니다.
        RemoveHealth();
    }

    private IEnumerator Flash()
    {
        m_MeshRenderer.material.color = m_FlashDamageColor;

        WaitForSeconds wait = new WaitForSeconds(0.1f);
        // WaitForSeconds->주어진 시간(초)동안, co - routine의 수행을 중단합니다.
        yield return wait;

        m_MeshRenderer.material.color = m_OriginalColor;
    }

    private void RemoveHealth()
    {
        m_Health--;
        CheckForDeath();
    }

    private void ResetHealth()
    {
        m_Health = m_MaxHealth;
    }

    private void CheckForDeath()
    {
        if (m_Health <= 0)
            Kill();
    }

    private void Update()
    {

    }
    private bool Kill()
    {
        gameObject.SetActive(false);
        //public void SetActive(bool value);->게임 개체를 활성화/비활성화합니다.
        return true;
    }

}
