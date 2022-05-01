using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Minion : MonoBehaviour
{
    private CombatAnimations CombatAnimations;
    public Color ShieldColor;
    [SerializeField]
    private InvulnerabilityColor InvulnerabilityColor;
    public float ColorTintDuration = 1f;
    public int[] Damages = new int[0];

    private GameObject Tooltip;

    public List<Effect> Effects;

    public bool AOE;

    // Start is called before the first frame update
    void Start()
    {

        CombatAnimations = GetComponent<CombatAnimations>();
        InvulnerabilityColor = GetComponent<InvulnerabilityColor>();

        Tooltip = GameObject.Find("TooltipObject");
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
            Debug.Log(gameObject.name + " shields for " + Damages[(int)TypeEnum.Shield]);
            InvulnerabilityColor.SetTintColor(ShieldColor, ColorTintDuration);
            return Damages[(int)TypeEnum.Shield];
        }
        return 0;
    }

    public void OnMouseEnter()
    {
        Tooltip.transform.position = Input.mousePosition;
        //Tooltip.SetActive(true);
        PopulateTooltipObject();
    }

    public void OnMouseExit()
    {
        //Tooltip.SetActive(false);
        Tooltip.transform.position = new Vector3(-1000, 0, 0);
    }

    private void PopulateTooltipObject()
    {
        Tooltip.transform.Find("Fire/Text").GetComponent<Text>().text = Damages[0].ToString();
        Tooltip.transform.Find("Water/Text").GetComponent<Text>().text = Damages[1].ToString();
        Tooltip.transform.Find("Grass/Text").GetComponent<Text>().text = Damages[2].ToString();
        Tooltip.transform.Find("Dark/Text").GetComponent<Text>().text = Damages[3].ToString();
        Tooltip.transform.Find("Light/Text").GetComponent<Text>().text = Damages[4].ToString();
        Tooltip.transform.Find("Shield/Text").GetComponent<Text>().text = Damages[5].ToString();
    }

    public void TriggerStartOfTurnEffects()
    {
        Effects.Where(e => e.EffectType == EffectType.StartOfTurn).ToList().ForEach(e => e.Trigger());
    }

    public void TriggerSummonEffects()
    {
        Effects.Where(e => e.EffectType == EffectType.SummonEffect).ToList().ForEach(e => e.Trigger());
    }
}
