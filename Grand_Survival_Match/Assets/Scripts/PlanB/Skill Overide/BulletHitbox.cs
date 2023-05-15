using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletHitbox : HitBox
{   
    public float speed;

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        if (photonView.IsMine)
        {
            transform.Translate(Vector3.forward * Time.deltaTime * speed);
        }
    }
}
