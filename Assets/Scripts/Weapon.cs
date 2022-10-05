using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public Transform muzzle;    // �ѱ� ��ġ.
    public Bullet bulletPrefab; // �Ѿ� ������.
    public float bulletSpeed;   // ź��.
    public int maxBullet;       // �ִ� �Ѿ� ��.
    public int bulletCount;     // ���� �Ѿ� ��.

    private void Start()
    {
        UI.Instance.SetBullet(bulletCount, maxBullet);
    }

    Vector3 destination;
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
            destination = eye.transform.position + (eye.transform.forward * float.MaxValue);
        }
    }

    public void Shoot(System.Action<HIT_TYPE> callback)
    {
        if (bulletCount <= 0)
            return;

        Bullet newBullet = Instantiate(bulletPrefab);
        newBullet.transform.position = muzzle.position;     // �Ѿ��� ������� �ѱ�(muzzle)�̴�.
        newBullet.transform.LookAt(destination);            // ������ �������� �ٶ󺸰� �Ѵ� (���� ����)
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
