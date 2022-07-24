using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UnitInfo : MonoBehaviour
{
    private bool Minion;

    private Vector3 startPosition;

    public GameObject EffectPrefab;

    public Dictionary<EffectType, Sprite> typeIcons;
    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position;
        Hide();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PopulateUi(Minion minion)
    {
        Show();
        Minion = true;
        PopulateName(minion.name);
        PopulateElements(minion.Damages);
        PopulateIcon(minion.Sprite);
        PopulateHealth(minion.CurrentHp, minion.MaxHp);
        PopulateDamage(0);
        PopulateEffects(minion.Effects);
    }
    public void PopulateUi(Enemy enemy)
    {
        Show();
        Minion = false;
        PopulateName(enemy.name);
        PopulateIcon(enemy.Sprite);
        PopulateHealth(enemy.CurrentHp, enemy.MaxHp);
        PopulateDamage(enemy.Damage);
        PopulateElements(new int[6]);
        PopulateEffects(enemy.Effects);
    }

    private void PopulateName(string name)
    {
        transform.Find("UnitName").GetComponent<Text>().text = name.Replace("(Clone)", "");
    }
    private void PopulateIcon(Sprite sprite)
    {
        transform.Find("Icon").GetComponent<Image>().sprite = sprite;
    }

    private void PopulateHealth(int currentHealth, int maxHealth)
    {
        transform.Find("HpText").GetComponent<Text>().text = currentHealth.ToString() + "/" + maxHealth.ToString();
    }

    private void PopulateDamage(int damage)
    {
        transform.Find("DamageText").GetComponent<Text>().text = Minion ? "-" : damage.ToString();
    }

    private void PopulateElements(int[] elements)
    {
        var elementsObject = transform.Find("Elements");

        elementsObject.Find("Fire/Text").GetComponent<Text>().text = Minion ? elements[0].ToString() : "-";
        elementsObject.Find("Water/Text").GetComponent<Text>().text = Minion ? elements[1].ToString() : "-";
        elementsObject.Find("Grass/Text").GetComponent<Text>().text = Minion ? elements[2].ToString() : "-";
        elementsObject.Find("Light/Text").GetComponent<Text>().text = Minion ? elements[3].ToString() : "-";
        elementsObject.Find("Mana/Text").GetComponent<Text>().text = Minion ? elements[4].ToString() : "-";
        elementsObject.Find("Shield/Text").GetComponent<Text>().text = Minion ? elements[5].ToString() : "-";
    }

    private void PopulateEffects(List<Effect> effects)
    {
        var y = -25;

        foreach(Effect e in effects.Where(e => e.Visible).ToList())
        {
            var prefab = Instantiate(EffectPrefab);

            prefab.transform.Find("Description").GetComponent<Text>().text = e.Description;

            //prefab.transform.Find("Icon").GetComponent<Image>().sprite = GetTypeIcon(e.EffectType);

            prefab.transform.parent = transform.Find("Effects");
            prefab.transform.localPosition = new Vector3(0, y, 0);
            prefab.transform.localScale = new Vector3(1, 1, 1);


            y -= 25;
        } 
            
    }


    void Show()
    {
        transform.Find("Effects").Clear();
        transform.position = startPosition;
    }

    public void Hide()
    {
        transform.Translate(new Vector3(-1000, 0, 0)); 
    }

    private Sprite GetTypeIcon(EffectType effectType)
    {
        return typeIcons[effectType];
    }
}
