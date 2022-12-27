using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterHpBar : MonoBehaviour
{
    public GameObject trackingTarget { private get; set; }
    [SerializeField] GameObject layer;
    [SerializeField]Text playerName;
    [SerializeField]RectTransform hpSlider;
    [SerializeField]RectTransform shiledSlider;
    [SerializeField] RectTransform smothGraphic;
    [SerializeField] Text text;

    float hp;
    float maxHp;
    float barrier;

    private void Update()
    {
        if (trackingTarget != null)
        {
            layer.SetActive(true);
            transform.position = trackingTarget.transform.position;
        }
        else
        {
            layer.SetActive(false);
        }

        //SetUIValue(hp, maxHp, barrier);

        if (smothGraphic != null && hp + barrier <= maxHp)
        {
            smothGraphic.anchorMax = new Vector2(Mathf.Lerp(smothGraphic.anchorMax.x, hpSlider.anchorMax.x, Time.deltaTime * 5), 1);
        }
        else
        {
            smothGraphic.anchorMax = new Vector2(Mathf.Lerp(smothGraphic.anchorMax.x, shiledSlider.anchorMax.x, Time.deltaTime * 5), 1);
        }
    }

    public void SetUIValue(float hp,float maxHp,float barrier)
    {
        this.hp = hp;
        this.maxHp = maxHp;
        this.barrier = barrier;
        if (maxHp > 0)
        {
            if (hp + barrier <= maxHp)
            {
                hpSlider.anchorMax = new Vector2(hp / (maxHp), 1);
                shiledSlider.anchorMax = new Vector2((hp + barrier) / (maxHp), 1);
            }
            else
            {
                hpSlider.anchorMax = new Vector2(hp / (hp + barrier), 1);
                shiledSlider.anchorMax = new Vector2((hp + barrier) / (hp + barrier), 1);
            }
        }
        shiledSlider.anchorMin = new Vector2(hpSlider.anchorMax.x, 0);
        if (text != null)
        {
/*            if (barrier == 0f)
            {*/
                text.text = (hp + barrier) + "/" + maxHp;
/*            }
            else
            {
                text.text = hp + "+" + barrier + "/" + maxHp;
            }*/
        }
    }

    public void SetPlayerName(string name)
    {
        playerName.text = name;
    }
}
