using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DTT.AreaOfEffectRegions;

public class Wizard : CharacterStats
{
    [Header("Skill Datas")]
    public CharacterSkill QSkillData;
    public CharacterSkill WSkillData;
    public CharacterSkill ESkillData;
    public CharacterSkill RSkillData;
    public CharacterSkill TSkillData;

    [Header("Skill Indicator Prefabs")]
    public GameObject SkillIndicatorAxis;
    public GameObject QSkillIndicator;
    public GameObject WSkillRangeIndicator;
    public GameObject WSkillIndicator;
    public GameObject ESkillRangeIndicator;
    public GameObject ESkillIndicator;
    public GameObject RSkillIndicator;
    public GameObject TSkillIndicator;

    [Header("Skill Prefabs")]
    public GameObject QSkillPrefab;
    public GameObject WSkillPrefab;
    public GameObject ESkillPrefab;
    public GameObject RSkillPrefab;
    public GameObject TSkillPrefab;

    bool qSkillOn;
    bool wSkillOn;
    bool eSkillOn;
    bool rSkillOn;
    bool tSkillOn;

    Vector3 position;
    Vector3 posUp;

    Vector3 WSkillHitPos;
    Vector3 ESkillHitPos;
    

    [Header("Other")]
    public Transform player;

    void Start()
    {
        maxHP = 900;
        atk = 30;
        def = 30;
        speed = 3.5f;
    }
    protected override void Update()
    {
        base.Update();
        SkillIndicatorActivate();

        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);        //게임 화면 내에 마우스 방향으로 레이 발사

        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            position = new Vector3(hit.point.x, hit.point.y, hit.point.z);
        }

        Quaternion transRot = Quaternion.LookRotation(position - player.transform.position);
        transRot.eulerAngles = new Vector3(0, transRot.eulerAngles.y, transRot.eulerAngles.z);

        SkillIndicatorAxis.transform.rotation = Quaternion.Lerp(transRot, SkillIndicatorAxis.transform.rotation, 0f);       //마우스가 보고있는 방향으로 스킬 표시기 회전 설정

        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            if (hit.collider.gameObject != this.gameObject)
            {
                posUp = new Vector3(hit.point.x, 10f, hit.point.z);
                position = hit.point;
            }
        }

        var hitPosDir = (hit.point - transform.position).normalized;
        float wSkillDistance = Vector3.Distance(hit.point, transform.position);
        wSkillDistance = Mathf.Min(wSkillDistance, 5.25f);
        float eSkillDistance = Vector3.Distance(hit.point, transform.position);
        eSkillDistance = Mathf.Min(eSkillDistance, 3.25f);

        WSkillHitPos = transform.position + hitPosDir * wSkillDistance;
        WSkillIndicator.transform.position = new Vector3(WSkillHitPos.x, 0.1f, WSkillHitPos.z);

        ESkillHitPos = transform.position + hitPosDir * eSkillDistance;
        ESkillIndicator.transform.position = new Vector3(ESkillHitPos.x, 0.1f, ESkillHitPos.z);

        TSkillIndicator.transform.position = new Vector3(WSkillHitPos.x, 0.1f, WSkillHitPos.z);
    }
    void SkillIndicatorActivate()
    {
        #region W스킬
        if (Input.GetKeyDown(KeyCode.W) && wCooltime <= 0)
        {
            wSkillOn = true;
            qSkillOn = false;
            eSkillOn = false;
            rSkillOn = false;
            tSkillOn = false;
        }
        if (Input.GetKey(KeyCode.W) && wSkillOn)
        {
            WSkillIndicator.SetActive(true);
            WSkillRangeIndicator.SetActive(true);
            if (Input.GetMouseButtonDown(1))
            {
                wSkillOn = false;
            }
        }
        if (Input.GetKeyUp(KeyCode.W) && wCooltime <= 0 && wSkillOn)
        {
            UseW(5);
            wSkillOn = false;
        }
        if (!wSkillOn)
        {
            WSkillIndicator.SetActive(false);
            WSkillRangeIndicator.SetActive(false);
        }
        #endregion

        #region E스킬
        if (Input.GetKeyDown(KeyCode.E) && eCooltime <= 0)
        {
            eSkillOn = true;
            qSkillOn = false;
            wSkillOn = false;
            rSkillOn = false;
            tSkillOn = false;
        }
        if (Input.GetKey(KeyCode.E) && eSkillOn)
        {
            ESkillIndicator.SetActive(true);
            ESkillRangeIndicator.SetActive(true);
            if (Input.GetMouseButtonDown(1))
            {
                eSkillOn = false;
            }
        }
        if (Input.GetKeyUp(KeyCode.E) && eCooltime <= 0 && eSkillOn)
        {
            UseE(6);
            eSkillOn = false;
        }
        if (!eSkillOn)
        {
            ESkillIndicator.SetActive(false);
            ESkillRangeIndicator.SetActive(false);
        }
        #endregion

        #region T스킬
        if (Input.GetKeyDown(KeyCode.T) && tCooltime <= 0)
        {
            tSkillOn = true;
            qSkillOn = false;
            wSkillOn = false;
            eSkillOn = false;
            rSkillOn = false;
        }
        if (Input.GetKey(KeyCode.T) && tSkillOn)
        {
            TSkillIndicator.SetActive(true);
            WSkillRangeIndicator.SetActive(true);
            if (Input.GetMouseButtonDown(1))
            {
                tSkillOn = false;
            }
        }
        if (Input.GetKeyUp(KeyCode.T) && tCooltime <= 0 && tSkillOn)
        {
            UseT(6);
            tSkillOn = false;
        }
        if (!tSkillOn)
        {
            TSkillIndicator.SetActive(false);
        }
        #endregion
    }

    protected override void UseQ(float coolTime)
    {
        base.UseQ(coolTime);
    }

    protected override void UseW(float coolTime)
    {
        base.UseW(coolTime);
        WSkillPrefab.GetComponent<SkillPrefab>().Attacker = this.gameObject;
        WSkillPrefab.GetComponent<SkillPrefab>().CharacterSkill = WSkillData;
        Instantiate(WSkillPrefab, WSkillHitPos, gameObject.transform.rotation);
    }

    protected override void UseE(float coolTime)
    {
        base.UseE(coolTime);
        ESkillPrefab.GetComponent<SkillPrefab>().Attacker = this.gameObject;
        ESkillPrefab.GetComponent<SkillPrefab>().CharacterSkill = ESkillData;
        Instantiate(ESkillPrefab, ESkillHitPos, gameObject.transform.rotation);
    }

    protected override void UseR(float coolTime)
    {
        base.UseR(coolTime);
    }

    protected override void UseT(float coolTime)
    {
        base.UseT(coolTime);
        TSkillPrefab.GetComponent<SkillPrefab>().Attacker = this.gameObject;
        TSkillPrefab.GetComponent<SkillPrefab>().CharacterSkill = TSkillData;
        Instantiate(TSkillPrefab, WSkillHitPos, gameObject.transform.rotation);
    }
}
