using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] float maxHp;
    [SerializeField] float hp;

    public void OnHit(HIT_TYPE type, int power)
    {
        float finalDamage = power * Hitbox.damageRatio[(int)type];      // ������ ����.
        hp = Mathf.Clamp(hp - finalDamage, 0, maxHp);
    }
}
