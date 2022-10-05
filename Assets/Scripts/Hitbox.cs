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
    // ������ �ǰ� ������ ����.
    public static float[] damageRatio = { 2.5f, 1.0f , 0.7f, 0.5f};

    [SerializeField] HIT_TYPE type;
    [SerializeField] UnityEvent<HIT_TYPE, int> onHit;


    // ���� ��� '����'�ε� power�������� �¾Ҵ�.
    public HIT_TYPE OnHit(int power)
    {
        onHit?.Invoke(type, power);
        return type;
    }
}
