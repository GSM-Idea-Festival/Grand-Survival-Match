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

    [Header("Skill Indicator Prefabs")]
    public GameObject SkillIndicatorAxis;
    public GameObject QSkillIndicator;
    public GameObject UltQSkillIndicator;
    public GameObject WSkillIndicator;
    public GameObject ESkillIndicator;
    public GameObject RSkillIndicator;

    [Header("Skill Prefabs")]
    public GameObject QSkillPrefab;
    public GameObject UltQSkillPrefab;
    public GameObject WSkillPrefab;
    public GameObject ESkillPrefab;
    public GameObject RSkillPrefab;
    public GameObject TSkillPrefab;

    bool qSkillOn;
    bool wSkillOn;
    bool eSkillOn;
    bool rSkillOn;

    float tSkillTime;

    Vector3 position;

    [Header("Other")]
    public Transform player;

    void Start()
    {
        maxHP = 1200;
        hp = 1200;
        atk = 75;
        def = 40;
        speed = 4;
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

        if (tSkillTime > 0)
        {
            TSkillPrefab.SetActive(true);
        }
        else
        {
            TSkillPrefab.SetActive(false);
        }
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
        }
        if (Input.GetKey(KeyCode.Q) && qSkillOn && tSkillTime <= 0)
        {
            QSkillIndicator.SetActive(true);
            if (Input.GetMouseButtonDown(1))
            {
                qSkillOn = false;
            }
        }
        if (Input.GetKey(KeyCode.Q) && qSkillOn && tSkillTime > 0)
        {
            UltQSkillIndicator.SetActive(true);
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
            UltQSkillIndicator.SetActive(false);
        }
        #endregion

        #region W스킬
        if (Input.GetKeyDown(KeyCode.W) && wCooltime <= 0)
        {
            qSkillOn = false;
            wSkillOn = true;
            eSkillOn = false;
            rSkillOn = false;
        }
        if (Input.GetKey(KeyCode.W) && wSkillOn)
        {
            WSkillIndicator.SetActive(true);
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
        }
        #endregion

        #region E스킬
        if (Input.GetKeyDown(KeyCode.E) && eCooltime <= 0)
        {
            qSkillOn = false;
            wSkillOn = false;
            eSkillOn = true;
            rSkillOn = false;
        }
        if (Input.GetKey(KeyCode.E) && eSkillOn)
        {
            ESkillIndicator.SetActive(true);
            if (Input.GetMouseButtonDown(1))
            {
                eSkillOn = false;
            }
        }
        if (Input.GetKeyUp(KeyCode.E) && eCooltime <= 0 && eSkillOn)
        {
            UseE(5);
            eSkillOn = false;
        }
        if (!eSkillOn)
        {
            ESkillIndicator.SetActive(false);
        }
        #endregion

        #region R스킬
        if (Input.GetKeyDown(KeyCode.R) && rCooltime <= 0)
        {
            qSkillOn = false;
            wSkillOn = false;
            eSkillOn = false;
            rSkillOn = true;
        }
        if (Input.GetKey(KeyCode.R) && rSkillOn)
        {
            RSkillIndicator.SetActive(true);
            if (Input.GetMouseButtonDown(1))
            {
                rSkillOn = false;
            }
        }
        if (Input.GetKeyUp(KeyCode.R) && rCooltime <= 0 && rSkillOn)
        {
            UseR(5);
            rSkillOn = false;
        }
        if (!rSkillOn)
        {
            RSkillIndicator.SetActive(false);
        }
        #endregion

        #region T스킬
        if (Input.GetKeyDown(KeyCode.T) && tCooltime <= 0)
        {
            UseT(15);
        }
        #endregion
    }

    protected override void UseQ(float coolTime)
    {
        base.UseQ(coolTime);
        QSkillData.DebuffDatas[0].value = atk;
        QSkillPrefab.GetComponent<SkillPrefab>().Attacker = this.gameObject;
        QSkillPrefab.GetComponent<SkillPrefab>().CharacterSkill = QSkillData;
        UltQSkillPrefab.GetComponent<SkillPrefab>().Attacker = this.gameObject;
        UltQSkillPrefab.GetComponent<SkillPrefab>().CharacterSkill = QSkillData;
        if (tSkillTime <= 0)
        {
            Instantiate(QSkillPrefab, player.transform.position, SkillIndicatorAxis.transform.rotation);
        }
        else
        {
            Instantiate(UltQSkillPrefab, player.transform.position, SkillIndicatorAxis.transform.rotation);
        }
    }

    protected override void UseW(float coolTime)
    {
        base.UseW(coolTime);
    }

    protected override void UseE(float coolTime)
    {
        base.UseE(coolTime);
        ESkillPrefab.GetComponent<SkillPrefab>().Attacker = this.gameObject;
        ESkillPrefab.GetComponent<SkillPrefab>().CharacterSkill = ESkillData;
        Instantiate(ESkillPrefab, player.transform.position, SkillIndicatorAxis.transform.rotation);
    }

    protected override void UseR(float coolTime)
    {
        base.UseR(coolTime);
        RSkillPrefab.GetComponent<SkillPrefab>().Attacker = this.gameObject;
        RSkillPrefab.GetComponent<SkillPrefab>().CharacterSkill = RSkillData;
        Instantiate(RSkillPrefab, player.transform.position, SkillIndicatorAxis.transform.rotation);
    }

    protected override void UseT(float coolTime)
    {
        base.UseT(coolTime);
        tSkillTime = 8;
        SetATK(25, 8);
    }
}
