using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    readonly float LIFE_TIME = 25f;
    float lifeTime;

    System.Action<HIT_TYPE> onHitCallback;

    public void Shoot(float speed, System.Action<HIT_TYPE> onHitCallback)
    {
        this.onHitCallback = onHitCallback;     // 콜백 이벤트 등록.

        Rigidbody rigid = GetComponent<Rigidbody>();
        rigid.velocity = transform.forward * speed;
    }
    private void Update()
    {
        if ((lifeTime += Time.deltaTime) >= LIFE_TIME)
            Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Hitbox hitbox = collision.collider.GetComponent<Hitbox>();
        if (hitbox != null)
        {
            HIT_TYPE type = hitbox.OnHit(100);
            onHitCallback?.Invoke(type);        // 총알의 주인에게 피격 부위 전달.
        }

        Destroy(gameObject);
    }    
}
