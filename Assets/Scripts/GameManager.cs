using Unity.Mathematics;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject cherry;
    [SerializeField] private GameObject strawberry;
    [SerializeField] private GameObject grape;
    [SerializeField] private GameObject dekopon;
    [SerializeField] private GameObject persimmon;
    [SerializeField] private GameObject apple;
    [SerializeField] private GameObject pear;
    [SerializeField] private GameObject peach;
    [SerializeField] private GameObject pineapple;
    [SerializeField] private GameObject melon;
    [SerializeField] private GameObject watermelon;

    [SerializeField] private float jarWidth;
    [SerializeField] private float jarHeight;
    [SerializeField] private float limitDepth;

    [SerializeField] private GameObject jarSide;
    [SerializeField] private GameObject jarFloor;
    
    private Vector3 previewPosition = new Vector3(7, 1, 0);
    private Vector3 dropHeight = new Vector3(0, 4, 0);

    private Transform fruitParent;

    private FruitBehaviour currentPreviewFruit;
    private FruitBehaviour currentHeldFruit;

    private bool hasLost;

    void Start()
    {
        fruitParent = new GameObject("FruitParent").transform;
        MergeManager.RegisterFruitParent(fruitParent);
        MergeManager.RegisterFruitGameObjects(cherry, strawberry, grape, dekopon, persimmon, apple, pear, peach, pineapple, melon, watermelon);
        
        BoundsManager.RegisterFruitRadii(cherry, strawberry, grape, dekopon, persimmon, apple, pear, peach, pineapple, melon, watermelon);
        
        SetUpJar();
        
        ResetGame();
    }

    private void SetUpJar()
    {
        GameObject jarParentObject = new GameObject("Jar");
        Transform jarParent = jarParentObject.transform;

        Vector3 jarSidePos = Vector3.right * (jarWidth / 2);

        GameObject side1 = Instantiate(jarSide, -jarSidePos, Quaternion.identity, jarParent);
        GameObject side2 = Instantiate(jarSide, jarSidePos, quaternion.identity, jarParent);

        BoxCollider2D side1Collider = side1.GetComponent<BoxCollider2D>();
        Vector2 sideColliderSize = side1Collider.size;
        float sideYScale = jarHeight / sideColliderSize.y;
        
        Transform side1Transform = side1.transform;
        Vector3 side1Scale = side1Transform.localScale;
        side1Scale.y = sideYScale;
        side1Transform.localScale = side1Scale;

        Transform side2Transform = side2.transform;
        Vector3 side2Scale = side2Transform.localScale;
        side2Scale.y = sideYScale;
        side2Transform.localScale = side2Scale;

        Vector3 jarFloorPos = Vector3.down * (jarHeight / 2);

        GameObject floor = Instantiate(jarFloor, jarFloorPos, Quaternion.Euler(0,0,90), jarParent);
        
        BoxCollider2D floorCollider = floor.GetComponent<BoxCollider2D>();
        Vector2 floorColliderSize = floorCollider.size;
        float floorYScale = jarWidth / floorColliderSize.y;
        
        Transform floorTransform = floor.transform;
        Vector3 floorScale = floorTransform.localScale;
        floorScale.y = floorYScale;
        floorTransform.localScale = floorScale;

        floorTransform.position += Vector3.up * (floorColliderSize.x / 2);
        
        BoxCollider2D limitCollider = GetComponent<BoxCollider2D>();
        Vector2 limitSize = limitCollider.size;

        Transform limitTransform = transform;
        
        Vector3 startScale = limitTransform.localScale;
        startScale.x = (jarWidth - sideColliderSize.x) / limitSize.x;
        limitTransform.localScale = startScale;
        
        BoundsManager.RegisterCentreDisplacement((jarWidth - sideColliderSize.x) / 2);

        limitTransform.position = Vector3.up * ((jarHeight / 2) - limitDepth);
    }

    void Update()
    {
        if (!hasLost)
        {
            if (currentHeldFruit.phase == FruitPhase.Holding && Input.GetKeyDown("mouse 0"))
            {
                DropHeldFruit();
            }

            if (currentHeldFruit.phase == FruitPhase.Falling && currentHeldFruit.canLose)
            {
                HoldPreviewedFruit();
                GetNewPreviewFruit();
            }
        }

        if (Input.GetKeyDown("r"))
        {
            ResetGame();
        }
    }

    private void ResetGame()
    {
        ScoreManager.ResetScore();
        
        foreach (Transform fruitTransform in fruitParent)
        {
            Destroy(fruitTransform.gameObject);
        }
        
        hasLost = false;

        GetNewHeldFruit();
        GetNewPreviewFruit();
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Fruit"))
        {
            FruitBehaviour fruit = collision.gameObject.GetComponent<FruitBehaviour>();
            if (fruit.canLose && !hasLost)
            {
                hasLost = true;
                Debug.Log("you have lost");
            }
        }
    }

    private void DropHeldFruit()
    {
        currentHeldFruit.SetPhase(FruitPhase.Falling);
    }

    private void HoldPreviewedFruit()
    {
        currentHeldFruit = currentPreviewFruit;
        currentHeldFruit.SetPhase(FruitPhase.Holding);
        currentHeldFruit.transform.position = dropHeight;
    }
    
    private void GetNewHeldFruit()
    {
        GameObject nextFruitType = MergeManager.GetStartingFruit();
        GameObject nextFruitObject = Instantiate(nextFruitType, dropHeight, Quaternion.identity, fruitParent.transform);
        
        currentHeldFruit = nextFruitObject.GetComponent<FruitBehaviour>();
        currentHeldFruit.SetPhase(FruitPhase.Holding);
        
    }

    private void GetNewPreviewFruit()
    {
        GameObject nextFruitType = MergeManager.GetStartingFruit();
        GameObject nextFruitObject = Instantiate(nextFruitType, previewPosition, Quaternion.identity, fruitParent.transform);
        
        currentPreviewFruit = nextFruitObject.GetComponent<FruitBehaviour>();
        currentPreviewFruit.SetPhase(FruitPhase.Preview);
        
    }
}
