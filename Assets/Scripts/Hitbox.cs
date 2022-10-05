using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum HIT_TYPE
{
    Head,
    Body,
    Arm,
    Leg,
}


public class Hitbox : MonoBehaviour
{
    // 부위별 피격 데미지 배율.
    public static float[] damageRatio = { 2.5f, 1.0f , 0.7f, 0.5f};

    [SerializeField] HIT_TYPE type;
    [SerializeField] UnityEvent<HIT_TYPE, int> onHit;


    // 나는 어떠한 '부위'인데 power데미지로 맞았다.
    public HIT_TYPE OnHit(int power)
    {
        onHit?.Invoke(type, power);
        return type;
    }
}
