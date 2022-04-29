using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameHandler : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            EnterCombat();
        }
    }

    public void EnterCombat()
    {
        SceneManager.LoadScene("BattleScene", LoadSceneMode.Additive);
        var battleScene = SceneManager.GetSceneByName("BattleScene");
        StartCoroutine(SetActive(battleScene));
    }
    public IEnumerator SetActive(Scene scene)
    {
        int i = 0;
        while (i == 0)
        {
            i++;
            yield return null;
        }
        SceneManager.SetActiveScene(scene);
        GameObject.Find("CombatHandler").GetComponent<CombatHandler>().active = true;
        yield break;
    }

    public void LeaveCombat()
    {
        SceneManager.UnloadScene("BattleScene");

    }
}
