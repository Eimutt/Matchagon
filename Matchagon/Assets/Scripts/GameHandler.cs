using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameHandler : MonoBehaviour
{
    public RewardScreen RewardScreen;
    public WorldMap WorldMap;
    // Start is called before the first frame update
    void Start()
    {
        RewardScreen = GameObject.Find("VictoryScreen").GetComponent<RewardScreen>();
        WorldMap = GameObject.Find("WorldMap").GetComponent<WorldMap>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            EnterShop();
        }
    }

    public void EnterCombat(Encounter encounter)
    {
        SceneManager.LoadScene("BattleScene", LoadSceneMode.Additive);
        var battleScene = SceneManager.GetSceneByName("BattleScene");
        StartCoroutine(SetActive(battleScene, encounter));
        WorldMap.gameObject.SetActive(false);
    }

    public void EnterShop()
    {
        SceneManager.LoadScene("ShopScene", LoadSceneMode.Additive);
        var battleScene = SceneManager.GetSceneByName("ShopScene");
        //StartCoroutine(SetActive(battleScene, encounter));
        WorldMap.gameObject.SetActive(false);
    }


    public IEnumerator SetActive(Scene scene, Encounter encounter)
    {
        int i = 0;
        while (i == 0)
        {
            i++;
            yield return null;
        }
        SceneManager.SetActiveScene(scene);
        GameObject.Find("CombatHandler").GetComponent<CombatHandler>().Encounter = encounter;
        GameObject.Find("EncounterInfo").GetComponent<EncounterInfo>().PopulateUI(encounter.Waves);
        GameObject.Find("CombatHandler").GetComponent<CombatHandler>().active = true;
        yield break;
    }

    public void LeaveCombat()
    {
        SceneManager.UnloadScene("BattleScene");
        RewardScreen.GenerateBattleRewards();
    }

    public void EnterTreasure()
    {
        RewardScreen.GenerateTreasureRewards();
    }

    public void LeaveShop()
    {
        SceneManager.UnloadScene("ShopScene");
        WorldMap.gameObject.SetActive(true);
        WorldMap.ActivateMovement();
    }

    public void CloseRewards()
    {
        RewardScreen.Close();
        WorldMap.gameObject.SetActive(true);
        WorldMap.ActivateMovement();
    }
}
