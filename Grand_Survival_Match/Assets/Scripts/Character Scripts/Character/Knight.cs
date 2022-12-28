using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DTT.AreaOfEffectRegions;

public class Knight : CharacterStats
{
    [Header("Skill Datas")]
    public CharacterSkill QSkillData;
    public CharacterSkill ESkillData;
    public CharacterSkill TSkillData;

    [Header("Skill Indicator Prefabs")]
    public GameObject SkillIndicatorAxis;
    public GameObject QSkillIndicator;
    public GameObject ESkillIndicator;
    public GameObject TSkillIndicator;

    [Header("Skill Prefabs")]
    public GameObject QSkillPrefab;
    public GameObject WSkillPrefab;
    public GameObject ESkillPrefab;
    public GameObject RSkillPrefab;
    public GameObject TSkillPrefab;

    bool qSkillOn;
    bool eSkillOn;
    bool tSkillOn;

    public float usingESkill;

    Vector3 position;

    [Header("Other")]
    public Transform player;
    public Rigidbody rigidbody;

    // Start is called before the first frame update
    void Start()
    {
        maxHP = 1500;
        hp = 1500;
        atk = 50;
        def = 50;
        speed = 3.5f;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

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

    protected override void SkillIndicatorActivate()
    {
        #region Q스킬
        if (Input.GetKeyDown(KeyCode.Q) && qCooltime <= 0)
        {
            qSkillOn = true;
            eSkillOn = false;
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

        #region W스킬
        if (Input.GetKeyDown(KeyCode.W) && wCooltime <= 0)
        {
            UseW(5);
        }
        #endregion

        #region E스킬
        if (Input.GetKeyDown(KeyCode.E) && eCooltime <= 0)
        {
            ESkillIndicator.GetComponent<LineRegion>().FillProgress = 0;
            speed -= 2.5f;
            usingESkill = 2.5f;
            def += 25;
            isUnstoppable = true;
        }
        if (Input.GetKey(KeyCode.E) && eCooltime <= 0)
        {
            eSkillOn = true;
            qSkillOn = false;
            tSkillOn = false;

            ESkillIndicator.SetActive(true);
            ESkillIndicator.GetComponent<LineRegion>().FillProgress += Time.deltaTime / 1.5f;

            usingESkill -= Time.deltaTime;
        }
        if ((Input.GetKeyUp(KeyCode.E) && eCooltime <= 0 && eSkillOn) || (eCooltime <= 0 && eSkillOn && usingESkill <= 0))
        {
            player.rotation = SkillIndicatorAxis.transform.rotation;
            UseE(8);
            speed += 2.5f;
            def -= 25;
            isUnstoppable = false;
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
            UseR(5);
        }
        #endregion

        #region T스킬
        if (Input.GetKeyDown(KeyCode.T) && tCooltime <= 0)
        {
            tSkillOn = true;
            qSkillOn = false;
            eSkillOn = false;
        }
        if (Input.GetKey(KeyCode.T) && tSkillOn)
        {
            TSkillIndicator.SetActive(true);
            if (Input.GetMouseButtonDown(1))
            {
                tSkillOn = false;
            }
        }
        if (Input.GetKeyUp(KeyCode.T) && tCooltime <= 0 && tSkillOn)
        {
            UseT(2);
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
        QSkillData.DebuffDatas[0].value = atk;
        QSkillPrefab.GetComponent<SkillPrefab>().Attacker = this.gameObject;
        QSkillPrefab.GetComponent<SkillPrefab>().CharacterSkill = QSkillData;
        Instantiate(QSkillPrefab, gameObject.transform.position, SkillIndicatorAxis.transform.rotation);
    }

    protected override void UseW(float coolTime)
    {
        base.UseW(coolTime);
        Instantiate(WSkillPrefab, gameObject.transform.position, gameObject.transform.rotation);
        StartCoroutine(SetATK(75, 2f));
    }

    protected override void UseE(float coolTime)
    {
        base.UseE(coolTime);
        StartCoroutine(UseEDash());
    }

    IEnumerator UseEDash()
    {
        Vector3 locVel = transform.InverseTransformDirection(rigidbody.velocity);
        locVel.x = 0;
        locVel.x = 5f;
        locVel.y = 0;
        rigidbody.velocity = transform.InverseTransformDirection(locVel);
        yield break;
    }

    protected override void UseR(float coolTime)
    {
        base.UseR(coolTime);
        Instantiate(RSkillPrefab, gameObject.transform.position, gameObject.transform.rotation).transform.parent = player.transform;
        SetHpBarrier(3, 0.1f);
    }

    protected override void UseT(float coolTime)
    {
        base.UseT(coolTime);
        TSkillPrefab.GetComponent<SkillPrefab>().Attacker = this.gameObject;
        TSkillPrefab.GetComponent<SkillPrefab>().CharacterSkill = TSkillData;
        Instantiate(TSkillPrefab, gameObject.transform.position, SkillIndicatorAxis.transform.rotation);
    }
}
