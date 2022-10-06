using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] Weapon weapon;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        weapon.Setup(OnHitEnemy);     // 무기 초기화.
    }

    private void Update()
    {
        // 마우스 버튼을 눌럿을 때.
        if (Input.GetMouseButtonDown(0))
        {
            weapon.StartFire();
        }
        // 마우스 버튼을 누르고 있는 동안.
        else if(Input.GetMouseButton(0))
        {
            weapon.StayFire();
        }

        // 오른쪽 키를 누르는 중
        if(Input.GetMouseButton(1))
        {
            weapon.Aim(Input.GetMouseButtonDown(1));
        }
        if(Input.GetMouseButtonUp(1))
        {
            weapon.EndAim();
        }

        // 장전, 모드 변경.
        if (Input.GetKeyDown(KeyCode.B))
            weapon.ChangeMode();

        if (Input.GetKeyDown(KeyCode.R))
            weapon.Reload();
    }

    private void OnHitEnemy(HIT_TYPE type)
    {
        // UI에 히트 이미지 호출.
        Debug.Log("적 피격 : " + type);

        UI.Instance.OnHit(type);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * 100f);
    }
}
