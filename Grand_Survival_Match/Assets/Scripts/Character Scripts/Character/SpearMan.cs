using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpearMan : CharacterStats
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
    public GameObject WSkillIndicator;
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

    [Header("Other")]
    public Transform player;

    void Start()
    {
        
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
    }

    void SkillIndicatorActivate()
    {
        #region Q스킬
        if (Input.GetKeyDown(KeyCode.Q) && qCooltime <= 0)
        {
            qSkillOn = true;
            wSkillOn = false;
            eSkillOn = false;
            rSkillOn = false;
            tSkillOn = false;
        }
        if (Input.GetKey(KeyCode.Q) && qSkillOn)
        {
            QSkillIndicator.SetActive(true);
            if (Input.GetMouseButtonDown(1))
            {
                qSkillOn = false;
            }
        }
        if (Input.GetKeyUp(KeyCode.Q) && qCooltime <= 0 && qSkillOn)
        {
            UseQ(2);
            qSkillOn = false;
        }
        if (!qSkillOn)
        {
            QSkillIndicator.SetActive(false);
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
    }

    protected override void UseE(float coolTime)
    {
        base.UseE(coolTime);
    }

    protected override void UseR(float coolTime)
    {
        base.UseR(coolTime);
    }

    protected override void UseT(float coolTime)
    {
        base.UseT(coolTime);
    }
}
