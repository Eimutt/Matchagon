using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SphereGenerator : MonoBehaviour
{
    public List<TypeWeight> TypeWeights;
    public GameObject SphereObject;
    private int WeightSum;

    private void Awake()
    {

        WeightSum = TypeWeights.Sum(t => t.Weight);
    }

    public Sphere GenerateRandomSphere(float i, float j)
    {
        var sphereCopy = Instantiate(SphereObject, new Vector3(i, j, 0), Quaternion.identity, gameObject.transform);

        var sphere = sphereCopy.GetComponent<Sphere>();

        var randomInt = Random.Range(0, WeightSum);

        foreach (TypeWeight typeWeight in TypeWeights)
        {
            if (randomInt < typeWeight.Weight)
            {
                sphere.Type = typeWeight.Type;
                sphereCopy.GetComponent<SpriteRenderer>().sprite = typeWeight.Sprite;
                break;
            }
            randomInt -= typeWeight.Weight;
        }

        return sphere;
    }

    public Sprite GetColorSprite(TypeEnum typeEnum)
    {
        return TypeWeights.First(t => t.Type == typeEnum).Sprite;
    }
}

public abstract class GenericTypeWeight
{
    public TypeEnum Type;
    public int Weight;
    public Sprite Sprite;
}
[System.Serializable]
public class TypeWeight : GenericTypeWeight
{
    
}
