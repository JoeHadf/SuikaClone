using UnityEditor;
using UnityEditor.Experimental;
using UnityEngine;

public static class MergeManager
{
    private static GameObject cherry;
    private static GameObject strawberry;
    private static GameObject grape;
    private static GameObject dekopon;
    private static GameObject persimmon;
    private static GameObject apple;
    private static GameObject pear;
    private static GameObject peach;
    private static GameObject pineapple;
    private static GameObject melon;
    private static GameObject watermelon;

    private static Transform fruitParent;
    
    public static void RegisterFruitGameObjects(
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
        MergeManager.cherry = cherry;
        MergeManager.strawberry = strawberry;
        MergeManager.grape = grape;
        MergeManager.dekopon = dekopon;
        MergeManager.persimmon = persimmon;
        MergeManager.apple = apple;
        MergeManager.pear = pear;
        MergeManager.peach = peach;
        MergeManager.pineapple = pineapple;
        MergeManager.melon = melon;
        MergeManager.watermelon = watermelon;
    }

    public static void RegisterFruitParent(Transform parent)
    {
        MergeManager.fruitParent = parent;
    }
    
    public static FruitType GetNextFruitType(FruitType type)
    {
        int typeID = (int)type;
        int nextTypeID = (typeID + 1) % 12;
        return (FruitType)nextTypeID;
    }

    public static void MergeFruits(FruitType type,GameObject fruit1, GameObject fruit2)
    {
        Vector3 newPosition = (fruit1.transform.position + fruit2.transform.position) / 2;
        FruitType nextType = GetNextFruitType(type);
        
        if (nextType != FruitType.Null)
        {
            GameObject newFruit = Object.Instantiate(GetFruitGameObject(nextType), newPosition, Quaternion.identity, fruitParent);
            FruitBehaviour newBehaviour = newFruit.GetComponent<FruitBehaviour>();
            newBehaviour.SetPhase(FruitPhase.Falling);

            Rigidbody2D fruit1RB = fruit1.GetComponent<Rigidbody2D>();
            Rigidbody2D fruit2RB = fruit2.GetComponent<Rigidbody2D>();
            Rigidbody2D newFruitRB = newFruit.GetComponent<Rigidbody2D>();
            
            Vector2 newFruitVelocity = ((fruit1RB.velocity + fruit2RB.velocity) / 2).normalized;
            newFruitRB.velocity = newFruitVelocity;
        }
        
        Object.Destroy(fruit1);
        Object.Destroy(fruit2);
        
        ScoreManager.IncrementScoreFromMerge(type);
    }
    public static GameObject GetStartingFruit()
    {
        int randomFruitID = Random.Range(1, 6);
        FruitType randomFruit = (FruitType)randomFruitID;
        return GetFruitGameObject(randomFruit);
    }

    public static GameObject GetFruitGameObject(FruitType type)
    {
        switch (type)
        {
            case FruitType.Cherry:
                return cherry;
            case FruitType.Strawberry:
                return strawberry;
            case FruitType.Grape:
                return grape;
            case FruitType.Dekopon:
                return dekopon;
            case FruitType.Persimmon:
                return persimmon;
            case FruitType.Apple:
                return apple;
            case FruitType.Pear:
                return pear;
            case FruitType.Peach:
                return peach;
            case FruitType.Pineapple:
                return pineapple;
            case FruitType.Melon:
                return melon;
            case FruitType.Watermelon:
                return watermelon;
            default:
                return cherry;
        }
    }
}
