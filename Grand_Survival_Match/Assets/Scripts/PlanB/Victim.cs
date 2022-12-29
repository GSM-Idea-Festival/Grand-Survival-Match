using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Victim : MonoBehaviour
{
    StatManager statManager;

    private float hp;
    public float HP
    {
        get { return hp; }
        private set
        {
            hp = Mathf.Clamp(0, value, statManager.GetStat(PlayerStat.Hp));
        }
    }

    private void Start()
    {
        statManager = GetComponent<StatManager>();
        hp = statManager.GetStat(PlayerStat.Hp);
    }

    public void TakeDamage(float damage)
    {
        if (!statManager.GetBuff(Buff.Immune))
        {
            if (!statManager.GetBuff(Buff.Defence))
            {
                hp -= damage;
            }
            else
            {
                hp -= damage * 0.5f;
            }
        }
        if (hp <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void TakeHeal(float heal)
    {
        hp += heal;
    }
}
