using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minion : MonoBehaviour
{
    private CombatAnimations CombatAnimations;
    public Color ShieldColor;
    [SerializeField]
    private InvulnerabilityColor InvulnerabilityColor;
    public float ColorTintDuration = 1f;
    public int[] Damages = new int[0];

    // Start is called before the first frame update
    void Start()
    {

        CombatAnimations = GetComponent<CombatAnimations>();
        InvulnerabilityColor = GetComponent<InvulnerabilityColor>();
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
}
