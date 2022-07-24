using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Minion : Unit
{
    private CombatAnimations CombatAnimations;
    public Color ShieldColor;
    [SerializeField]
    public Color DamagedColor;
    private InvulnerabilityColor InvulnerabilityColor;
    public float ColorTintDuration = 1f;
    public int[] Damages = new int[0];


    private UnitInfo UnitInfo;

    public List<Effect> Effects;

    public bool AOE;
    public bool AttackOnShield;

    public int position;

    public bool Dead;
    // Start is called before the first frame update
    void Start()
    {

        CombatAnimations = GetComponent<CombatAnimations>();
        InvulnerabilityColor = GetComponent<InvulnerabilityColor>();

        UnitInfo = GameObject.Find("UnitInfo").GetComponent<UnitInfo>();
        TriggerSummonEffects();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int GetShield()
    {
        if(Damages[(int)TypeEnum.Shield] > 0)
        {
           // Debug.Log(gameObject.name + " shields for " + Damages[(int)TypeEnum.Shield]);
            InvulnerabilityColor.SetTintColor(ShieldColor, ColorTintDuration);
            return Damages[(int)TypeEnum.Shield];
        }
        return 0;
    }

    public void OnMouseEnter()
    {
    }

    public void OnMouseExit()
    {
    }

    public void OnMouseDown()
    {
        UnitInfo.PopulateUi(this);
    }


    public void TriggerStartOfTurnEffects()
    {
        Effects.Where(e => e.EffectType == EffectType.StartOfTurn).ToList().ForEach(e => e.Trigger());
    }

    public void TriggerSummonEffects()
    {
        Effects.Where(e => e.EffectType == EffectType.SummonEffect).ToList().ForEach(e => e.Trigger());
    }

    public void TakeDamage(int damage)
    {
        var healthDamage = Mathf.Max(0, damage);


        CurrentHp -= healthDamage;
        InvulnerabilityColor.SetTintColor(DamagedColor, ColorTintDuration);
        GameObject.Find("CombatHandler").GetComponent<DamageTextHandler>().SpawnDamageText(transform.position, Color.red, healthDamage, 1);

        float percentage = (float)CurrentHp / (float)MaxHp;
        transform.Find("HpBarSlider(Clone)/Slider").GetComponent<Slider>().value = percentage;

        if(CurrentHp <= 0)
        {
            Dead = true;
        }
    }
}
