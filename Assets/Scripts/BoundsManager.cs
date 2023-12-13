using System.Collections.Generic;
using UnityEngine;

public static class BoundsManager
{
    public static float centreDisplacement { get; private set; }

    public static Dictionary<FruitType, float> fruitRadii { get; private set; }

    static BoundsManager()
    {
        centreDisplacement = 0;
        fruitRadii = new Dictionary<FruitType, float>();
    }

    public static void RegisterCentreDisplacement(float displacement)
    {
        centreDisplacement = displacement;
    }
    
    public static void RegisterFruitRadii(
        GameObject cherry,
        GameObject strawberry,
        GameObject grape,
        GameObject dekopon,
        GameObject persimmon,
        GameObject apple,
        GameObject pear,
        GameObject peach,
        GameObject pineapple,
        GameObject melon,
        GameObject watermelon)
    {
        RegisterFruitRadiusOfObject(cherry);
        RegisterFruitRadiusOfObject(strawberry);
        RegisterFruitRadiusOfObject(grape);
        RegisterFruitRadiusOfObject(dekopon);
        RegisterFruitRadiusOfObject(persimmon);
        RegisterFruitRadiusOfObject(apple);
        RegisterFruitRadiusOfObject(pear);
        RegisterFruitRadiusOfObject(peach);
        RegisterFruitRadiusOfObject(pineapple);
        RegisterFruitRadiusOfObject(melon);
        RegisterFruitRadiusOfObject(watermelon);
    }

    private static void RegisterFruitRadiusOfObject(GameObject fruitObject)
    {
        CircleCollider2D fruitCollider = fruitObject.GetComponent<CircleCollider2D>();
        FruitBehaviour fruitBehaviour = fruitObject.GetComponent<FruitBehaviour>();
        
        float fruitRadius = fruitCollider.radius;
        float fruitScale = fruitObject.transform.localScale.x;
        
        fruitRadii.Add(fruitBehaviour.GetFruitType(), fruitRadius * fruitScale);
    }
    
}
