using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CharacterTransformSharer : MonoBehaviourPun, IPunObservable
{
    Rigidbody rigid;

    float lag;
    float waitTime;
    Vector3 originPos;
    Vector3 targetPos;
    Quaternion originRot;
    Quaternion targetRot;

    void Start()
    {
        rigid = GetComponent<Rigidbody>();
        originPos = transform.position;
        targetPos = transform.position;
        originRot = transform.rotation;
        targetRot = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        waitTime = Mathf.Min(waitTime + Time.deltaTime,lag);
        if (!photonView.IsMine)
        {
            transform.position = Vector3.Lerp(originPos,targetPos,waitTime/lag);
            transform.rotation = Quaternion.Lerp(originRot, targetRot, waitTime / lag);
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        waitTime = 0;
        lag = Mathf.Abs((float)(PhotonNetwork.Time - info.timestamp));
        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
        }
        else
        {
            originPos = transform.position;
            originRot = transform.rotation;
            targetPos = (Vector3)stream.ReceiveNext();
            targetRot = (Quaternion)stream.ReceiveNext();
        }
    }
}
