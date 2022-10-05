using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public Transform muzzle;    // 총구 위치.
    public Bullet bulletPrefab; // 총알 프리팹.
    public float bulletSpeed;   // 탄속.
    public int maxBullet;       // 최대 총알 수.
    public int bulletCount;     // 현재 총알 수.

    private void Start()
    {
        UI.Instance.SetBullet(bulletCount, maxBullet);
    }

    Vector3 destination;
    private void Update()
    {
        Camera eye = Camera.main;   // 메인 카메라.
        Ray ray = new Ray(eye.transform.position, eye.transform.forward);   // 카메라 정면방향 Ray.

        RaycastHit hit;         // 충돌 정보.
        if (Physics.Raycast(ray, out hit, float.MaxValue))
        {
            // 내가 크로스헤어로 정확히 보고 있는 지점이 있다면.. 그 지점을 목적지로 잡는다.
            destination = hit.point;
        }
        else
        {
            // 허공을 바라보고 있다면 눈으로부터 정면으로 아주 먼 곳을 목적지로 잡는다.
            destination = eye.transform.position + (eye.transform.forward * float.MaxValue);
        }
    }

    public void Shoot(System.Action<HIT_TYPE> callback)
    {
        if (bulletCount <= 0)
            return;

        Bullet newBullet = Instantiate(bulletPrefab);
        newBullet.transform.position = muzzle.position;     // 총알의 출발지는 총구(muzzle)이다.
        newBullet.transform.LookAt(destination);            // 목적지 방향으로 바라보게 한다 (정면 기준)
        newBullet.Shoot(bulletSpeed, callback);
        bulletCount--;

        UI.Instance.SetBullet(bulletCount, maxBullet);
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(muzzle.position, muzzle.forward * 1000f);

        Gizmos.color = Color.red;
        Gizmos.DrawLine(muzzle.transform.position, destination);
    }
}
