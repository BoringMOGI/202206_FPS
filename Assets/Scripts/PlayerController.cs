using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] Weapon weapon;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            weapon.Shoot(OnHitEnemy);
        }
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
