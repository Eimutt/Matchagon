using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EncounterInfo : MonoBehaviour
{
    public GameObject WavePrefab;
    public Dictionary<int, GameObject> Waves;

    // Start is called before the first frame update
    void Start()
    {
        Waves = new Dictionary<int, GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PopulateUI(List<Wave> waves)
    {
        var yOffset = 0;
        foreach(var wave in waves)
        {
            var waveObject = Instantiate(WavePrefab, new Vector3(0, 0, 0), Quaternion.identity, transform.Find("Waves"));
            waveObject.transform.Find("Unit/Image").GetComponent<Image>().sprite = wave.Enemy.GetComponent<SpriteRenderer>().sprite;
            waveObject.transform.Find("Unit/Name").GetComponent<Text>().text = wave.Enemy.transform.name;
            waveObject.transform.Find("Unit/Amount").GetComponent<Text>().text = wave.Quanitity.ToString() + "x";

            waveObject.transform.Find("RoundText").GetComponent<Text>().text = wave.Turn.ToString();

            waveObject.transform.localPosition = new Vector3(0, yOffset, 0);

            Waves.Add(wave.Turn, waveObject);

            yOffset -= 25;
        }
    }

    public void CrossWave(int wave)
    {
        if(Waves.ContainsKey(wave))
        {
            Waves[wave].transform.Find("RoundText").GetComponent<Text>().color = Color.red;
        }
    }
}
