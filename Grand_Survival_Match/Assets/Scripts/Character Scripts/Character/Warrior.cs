using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warrior : CharacterStats
{
    public CharacterSkill qSkill;
    public CharacterSkill wSkill;
    public CharacterSkill eSkill;
    public CharacterSkill rSkill;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        if (stunTime <= 0)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                if (qCooltime > 0)
                {
                    return;
                }
                UseQ(5);
            }
            if (Input.GetKeyDown(KeyCode.W))
            {
                if (wCooltime > 0)
                {
                    return;
                }
                UseW(8);
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (eCooltime > 0)
                {
                    return;
                }
                UseE(5);
            }
            if (Input.GetKeyDown(KeyCode.R))
            {
                if (rCooltime > 0)
                {
                    return;
                }
                UseR(15);
            }
        }
    }
}
