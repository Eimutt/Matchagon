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
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            //EnterCombat();
        }
    }

    public void EnterCombat(Encounter encounter)
    {
        SceneManager.LoadScene("BattleScene", LoadSceneMode.Additive);
        var battleScene = SceneManager.GetSceneByName("BattleScene");
        StartCoroutine(SetActive(battleScene, encounter));
        WorldMap.gameObject.SetActive(false);
    }

    public void EnterStore()
    {
        SceneManager.LoadScene("StoreScene", LoadSceneMode.Additive);
        var battleScene = SceneManager.GetSceneByName("StoreScene");
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
        GameObject.Find("CombatHandler").GetComponent<CombatHandler>().active = true;
        GameObject.Find("CombatHandler").GetComponent<CombatHandler>().Encounter = encounter;
        yield break;
    }

    public void LeaveCombat()
    {
        SceneManager.UnloadScene("BattleScene");
        RewardScreen.GenerateRewards();
    }

    public void CloseRewards()
    {
        RewardScreen.Close();
        WorldMap.gameObject.SetActive(true);
        WorldMap.ActivateMovement();
    }
}
