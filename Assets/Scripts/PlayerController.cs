using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] Weapon weapon;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        weapon.Setup(OnHitEnemy);     // ���� �ʱ�ȭ.
    }

    private void Update()
    {
        // ���콺 ��ư�� ������ ��.
        if (Input.GetMouseButtonDown(0))
        {
            weapon.StartFire();
        }
        // ���콺 ��ư�� ������ �ִ� ����.
        else if(Input.GetMouseButton(0))
        {
            weapon.StayFire();
        }

        // ������ Ű�� ������ ��
        if(Input.GetMouseButton(1))
        {
            weapon.Aim(Input.GetMouseButtonDown(1));
        }
        if(Input.GetMouseButtonUp(1))
        {
            weapon.EndAim();
        }

        // ����, ��� ����.
        if (Input.GetKeyDown(KeyCode.B))
            weapon.ChangeMode();

        if (Input.GetKeyDown(KeyCode.R))
            weapon.Reload();
    }

    private void OnHitEnemy(HIT_TYPE type)
    {
        // UI�� ��Ʈ �̹��� ȣ��.
        Debug.Log("�� �ǰ� : " + type);

        UI.Instance.OnHit(type);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * 100f);
    }
}
