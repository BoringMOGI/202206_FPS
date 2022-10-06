using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public enum MODE
    {
        Single = 1 << 0,  // �ܹ�.
        Burst  = 1 << 1,  // ����.
        Auto   = 1 << 2,  // ����.
    }

    public Transform muzzle;    // �ѱ� ��ġ.
    public Bullet bulletPrefab; // �Ѿ� ������.

    [Header("Info")]
    public MODE[] modes;        // �ѱ⺰ ������ ���.
    public int maxBullet;       // �ִ� �Ѿ� ��.
    public float fireRate;      // ���� �ӵ�.
    public float bulletSpeed;   // ź��.

    private int bulletCount;        // ���� �Ѿ� ��.
    private int modeIndex;          // ��� �ε���.
    Vector3 destination;            // ������.
    Action<HIT_TYPE> onHitType;     // �Ѿ˿��� ������ ��Ʈ �̺�Ʈ.
    Animator anim;

    bool isBurst;                   // ���� ��� ��� ��.
    bool isReloading;               // ������.

    private MODE mode => modes[modeIndex];

    private void Start()
    {
        anim =GetComponent<Animator>();
    }
    private void Update()
    {
        Camera eye = Camera.main;   // ���� ī�޶�.
        Ray ray = new Ray(eye.transform.position, eye.transform.forward);   // ī�޶� ������� Ray.

        RaycastHit hit;         // �浹 ����.
        if (Physics.Raycast(ray, out hit, float.MaxValue))
        {
            // ���� ũ�ν����� ��Ȯ�� ���� �ִ� ������ �ִٸ�.. �� ������ �������� ��´�.
            destination = hit.point;
        }
        else
        {
            // ����� �ٶ󺸰� �ִٸ� �����κ��� �������� ���� �� ���� �������� ��´�.
            destination = eye.transform.position + (eye.transform.forward * 1000f);
        }
    }

    public void Setup(Action<HIT_TYPE> onHitType)
    {
        this.onHitType = onHitType;
        bulletCount = maxBullet;

        UI.Instance.SetBullet(bulletCount, maxBullet);
    }

    Coroutine coBurst;  // ���� �ڷ�ƾ.

    // �߻� Ű�� ������ ������ ���.
    public void StartFire()
    {
        if (isReloading)
            return;

        if(mode == MODE.Single)
        {
            // �ܹ�.
            Shoot();
        }
        else if (mode == MODE.Burst && coBurst == null)
        {
            // ����.
            coBurst = StartCoroutine(IEBurst());
        }
    }
    
    // �߻�Ű�� ��� ������ ���.
    public void StayFire()
    {
        if (isReloading)
            return;

        if (mode == MODE.Auto)
        {
            // ����.
            Shoot();
        }
    }
    public void ChangeMode()
    {
        if ((modeIndex += 1) >= modes.Length)
            modeIndex = 0;

        Debug.Log($"��� ���� : {mode}");
    }
    
    // ���� (����)
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

    // ���ε� (������)
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

    // �߻� ����...
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

        // ���� ��� ���� �ð�.
        nextFireTime = Time.time + fireRate;
        CreateBullet();
        bulletCount--;

        UI.Instance.SetBullet(bulletCount, maxBullet);
    }
    private void CreateBullet()
    {
        anim.SetTrigger("onFire");

        Bullet newBullet = Instantiate(bulletPrefab);
        newBullet.transform.position = muzzle.position;     // �Ѿ��� ������� �ѱ�(muzzle)�̴�.
        newBullet.transform.LookAt(destination);            // ������ �������� �ٶ󺸰� �Ѵ� (���� ����)
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
