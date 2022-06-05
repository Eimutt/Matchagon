using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RosterSlot : MonoBehaviour, IPointerClickHandler
{
    public MinionCard Minion;

    public void OnPointerClick(PointerEventData eventData)
    {
        GameObject.Find("DeckContainer").GetComponent<DeckView>().ToggleMinion(Minion);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
