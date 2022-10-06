using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public enum MODE
    {
        Single = 1 << 0,  // 단발.
        Burst  = 1 << 1,  // 점사.
        Auto   = 1 << 2,  // 연발.
    }

    public Transform muzzle;    // 총구 위치.
    public Bullet bulletPrefab; // 총알 프리팹.

    [Header("Info")]
    public MODE[] modes;        // 총기별 가능한 모드.
    public int maxBullet;       // 최대 총알 수.
    public float fireRate;      // 연사 속도.
    public float bulletSpeed;   // 탄속.

    private int bulletCount;        // 현재 총알 수.
    private int modeIndex;          // 모드 인덱스.
    Vector3 destination;            // 목적지.
    Action<HIT_TYPE> onHitType;     // 총알에게 전달할 히트 이벤트.
    Animator anim;

    bool isBurst;                   // 점사 모드 사격 중.
    bool isReloading;               // 장전중.

    private MODE mode => modes[modeIndex];

    private void Start()
    {
        anim =GetComponent<Animator>();
    }
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
            destination = eye.transform.position + (eye.transform.forward * 1000f);
        }
    }

    public void Setup(Action<HIT_TYPE> onHitType)
    {
        this.onHitType = onHitType;
        bulletCount = maxBullet;

        UI.Instance.SetBullet(bulletCount, maxBullet);
    }

    Coroutine coBurst;  // 점사 코루틴.

    // 발사 키를 누르기 시작한 경우.
    public void StartFire()
    {
        if (isReloading)
            return;

        if(mode == MODE.Single)
        {
            // 단발.
            Shoot();
        }
        else if (mode == MODE.Burst && coBurst == null)
        {
            // 점사.
            coBurst = StartCoroutine(IEBurst());
        }
    }
    
    // 발사키를 계속 누르는 경우.
    public void StayFire()
    {
        if (isReloading)
            return;

        if (mode == MODE.Auto)
        {
            // 연사.
            Shoot();
        }
    }
    public void ChangeMode()
    {
        if ((modeIndex += 1) >= modes.Length)
            modeIndex = 0;

        Debug.Log($"모드 변경 : {mode}");
    }
    
    // 에임 (조준)
    public void Aim(bool isStart)
    {
        if (isStart)
            anim.SetTrigger("onAim");

        anim.SetBool("isAim", true);
    }
    public void EndAim()
    {
        anim.SetBool("isAim", false);
    }

    // 리로드 (재장전)
    public void Reload()
    {
        if (isReloading || isBurst)
            return;

        isReloading = true;
        anim.SetTrigger("onReload");
    }
    private void OnEndReload()
    {
        isReloading = false;
        bulletCount = maxBullet;
        UI.Instance.SetBullet(bulletCount, maxBullet);
    }

    // 발사 관련...
    private IEnumerator IEBurst()
    {
        isBurst = true;

        for (int i = 0; i < 3; i++)
        {
            CreateBullet();
            bulletCount--;
            
            UI.Instance.SetBullet(bulletCount, maxBullet);
            yield return new WaitForSeconds(fireRate);
        }

        isBurst = false;
        coBurst = null;
    }

    private float nextFireTime;
    private void Shoot()
    {
        if (bulletCount <= 0 || Time.time < nextFireTime)
            return;

        // 다음 사격 가능 시간.
        nextFireTime = Time.time + fireRate;
        CreateBullet();
        bulletCount--;

        UI.Instance.SetBullet(bulletCount, maxBullet);
    }
    private void CreateBullet()
    {
        anim.SetTrigger("onFire");

        Bullet newBullet = Instantiate(bulletPrefab);
        newBullet.transform.position = muzzle.position;     // 총알의 출발지는 총구(muzzle)이다.
        newBullet.transform.LookAt(destination);            // 목적지 방향으로 바라보게 한다 (정면 기준)
        newBullet.Shoot(bulletSpeed, onHitType);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawLine(Camera.main.transform.position, Camera.main.transform.forward * 1000f);

        Gizmos.color = Color.blue;
        Gizmos.DrawRay(muzzle.position, muzzle.forward * 1000f);

        Gizmos.color = Color.red;
        Gizmos.DrawLine(muzzle.transform.position, destination);
    }
}
