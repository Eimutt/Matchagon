using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageNumber : MonoBehaviour
{
    public float speed;
    public float growTime;
    public float duration;
    private float timer;
    public float Xvariance;

    public float fadeoutDuration;
    private bool fadeout;

    public float startScale;

    private int damage;
    private int combo;
    public int c;
    // Start is called before the first frame update
    void Start()
    {
        transform.localScale = new Vector3(startScale, startScale, startScale);
        //transform.localPosition = new Vector3(0, 0, 0);
        float randomDif = Random.Range(-Xvariance, Xvariance);
        transform.Translate(Vector3.right * randomDif);
        c = 1;
    }

    // Update is called once per frame
    void Update()
    {

        transform.position = transform.position + new Vector3(0, speed * Time.deltaTime, 0);
        timer += Time.deltaTime;
        if (fadeout)
        {
            Color tmp = GetComponent<TextMesh>().color;
            tmp.a = 1 - (timer/fadeoutDuration);
            GetComponent<TextMesh>().color = tmp;

            GetComponent<TextMesh>().text = (damage * c).ToString();

            if (timer > fadeoutDuration)
            {
                Destroy(gameObject);
            }
        } else
        {

            var min = startScale + ((1 - startScale) * (timer / growTime));

            if (timer > (duration / 2) * (float)c / (float)combo) {

                c = (c == combo) ? c : c + 1; 
                GetComponent<TextMesh>().text = (damage * c).ToString();
            }


            transform.localScale = new Vector3(min, min, min);
            if (timer > duration)
            {
                timer = 0;
                fadeout = true;
            }
        }
    }

    public void Init(Color color, int damage, int combo)
    {
        GetComponent<TextMesh>().color = color;
        GetComponent<TextMesh>().text = damage.ToString();
        this.damage = damage;
        this.combo = combo;
    }
}
