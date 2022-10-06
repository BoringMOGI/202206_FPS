using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI : MonoBehaviour
{
    public static UI Instance;

    [SerializeField] TMP_Text bulletText;
    [SerializeField] Animation hitAnim;

    readonly string BULLET_FORMAT = "<size=50>{0}</size><size=20><color=#00D0FF>/</color>{1}</size>";

    private void Awake()
    {
        Instance = this;
    }
    public void SetBullet(int current, int max)
    {
        bulletText.text = string.Format(BULLET_FORMAT, current, max);
    }
    public void OnHit(HIT_TYPE type)
    {
        hitAnim.Stop();
        hitAnim.Play(type == HIT_TYPE.Head ? "Hit_head" : "Hit_normal");
    }
}
