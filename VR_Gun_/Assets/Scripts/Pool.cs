using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pool
{
    public List<T> Create<T>(GameObject prefab, int count)//??물어보기
        where T: MonoBehaviour
        //where T: 부모 클래스 이름/인터페이스 이름/new()/struct/class/U
        //where 형식매개변수: 재약조건
    {
        //New list
        List<T> newPool = new List<T>();
         

        //Creat
        for(int i=0;i<count;i++)
        {
            GameObject projectileObject = GameObject.Instantiate(prefab, Vector3.zero, Quaternion.identity);
            //(Instantiate(GameObject original(생성하고자 하는 현재 씬의 게임 오브젝트나 prefab으로 선언된 객체),
            //Vector3 position(생성될 위치),
            //Quaternion rotation(생성될 회전 값, 게임 오브젝트에서 설정된 값을 하려면->original.transform.rotation))
            //(Quanternion.identity->회전 없음,월드 좌표축 또는 부모의 축으로 정렬됨)
            T newProjectile = projectileObject.GetComponent<T>();

            newPool.Add(newProjectile);
            //리스트에 값넣기(해당 리스트의 맨 뒤 인덱스 뒤에 붙는다)
        }

        return newPool;
    }
}

public class ProjectilePool : Pool
{
    public List<Projectile> m_projectile = new List<Projectile>();
    //List<자료형>변수명=new List<자료형>();

    public ProjectilePool(GameObject prefab, int count)
    {
        m_projectile = Create<Projectile>(prefab, count);//??물어보기
    }

    public void SetAllProjectiles()
    {
        foreach (Projectile projectile in m_projectile)
            //foreach->변수를 배열에 담아서 배열에 담긴 변수들을 반복시켜주는 반복문
            //foreach(자료형 이름(A), in 이름(B))-> A는 B가 가진 배열의 상자 수 만큼 반복해서 돌게된다.
            projectile.SetInnactive();
             //SetInnactive->Projection 스크립트에 있음
    }
}
