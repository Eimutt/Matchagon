using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalEffectHandler : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DrawCard(int amount)
    {
        for(int i = 0; i < amount; i++)
            GameObject.Find("Player").GetComponent<Player>().DrawCard();
    }

    public void ModifySpawnPower(int amount)
    {
        GameObject.Find("GameHandler").GetComponent<PlayerData>().ModifySpawnPower(amount);
    }
    public void HideRandom(int sourceId, int amount)
    {
        GameObject.Find("Board").GetComponent<Board>().HideRandomSpheres(sourceId, amount);
    }
    public void FreezeRandom(int sourceId, int amount)
    {
        GameObject.Find("Board").GetComponent<Board>().FreezeRandomSpheres(sourceId, amount);
    }

    public void DestroyBoardObjectsFromSource(int sourceId, int amount)
    {
        GameObject.Find("Board").GetComponent<Board>().DestroyBoardObjectsFromSource(sourceId);
    }

    public void RandomToGreen(int amount)
    {
        GameObject.Find("Board").GetComponent<Board>().TransformRandomSpheres(TypeEnum.Grass, amount);
    }

    public void RandomToPlage(int amount)
    {
        GameObject.Find("Board").GetComponent<Board>().TransformRandomSpheres(TypeEnum.Plague, amount);
    }

    public void RandomToShield(int amount)
    {
        GameObject.Find("Board").GetComponent<Board>().TransformRandomSpheres(TypeEnum.Shield, amount);
    }

    public void GreenToRandom(int amount)
    {
        GameObject.Find("Board").GetComponent<Board>().TransformColorToRandomSpheres(TypeEnum.Grass, amount);
    }

    public void ModifyTurnTime(int amount)
    {
        GameObject.Find("CombatHandler").GetComponent<CombatHandler>().IncreaseTimeForNextTurn(amount);
    }
    public void ModifyCombo(int amount)
    {
        GameObject.Find("CombatHandler").GetComponent<CombatHandler>().IncreaseCombo(amount);
    }
    public void ModifyGrassDamageMultiplier(int amount)
    {
        GameObject.Find("Player").GetComponent<Player>().ChangeTypeDamageFlat(TypeEnum.Grass, amount);
    }
    public void ModifyWaterDamageMultiplier(int amount)
    {
        GameObject.Find("Player").GetComponent<Player>().ChangeTypeDamageFlat(TypeEnum.Water, amount);
    }
    public void ModifyFireDamageMultiplier(int amount)
    {
        GameObject.Find("Player").GetComponent<Player>().ChangeTypeDamageFlat(TypeEnum.Fire, amount);
    }

    public void ModifyGlobalDamageFlat(int amount)
    {
        GameObject.Find("Player").GetComponent<Player>().ChangeGlobalDamageFlat(amount);
    }
    public void ModifyGlobalDamagePercentage(int amount)
    {
        GameObject.Find("Player").GetComponent<Player>().ChangeGlobalDamagePercentage(amount);
    }

    public void ModifyShieldValue(int amount)
    {
        GameObject.Find("Player").GetComponent<Player>().GetShield(amount);
    }

    public void ModifySpawnRateFire(int amount)
    {
        GameObject.Find("Board").GetComponent<SphereGenerator>().MultiplyWeight(TypeEnum.Fire, amount);
    }
    public void ModifySpawnRateGrass(int amount)
    {
        GameObject.Find("Board").GetComponent<SphereGenerator>().MultiplyWeight(TypeEnum.Grass, amount);
    }
    public void ModifySpawnRateWater(int amount)
    {
        GameObject.Find("Board").GetComponent<SphereGenerator>().MultiplyWeight(TypeEnum.Water, amount);
    }
    public void ModifySpawnRateMana(int amount)
    {
        GameObject.Find("Board").GetComponent<SphereGenerator>().MultiplyWeight(TypeEnum.Light, amount);
    }
    public void ModifySpawnRateShield(int amount)
    {
        GameObject.Find("Board").GetComponent<SphereGenerator>().MultiplyWeight(TypeEnum.Shield, amount);
    }

    public void DamageAllMinions(int percentage)
    {
        GameObject.Find("Player").GetComponent<Player>().DamageAllMinions(percentage);
    }

    public void HealPlayer(int amount)
    {
        GameObject.Find("GameHandler").GetComponent<PlayerData>().Heal(amount);
    }
}
