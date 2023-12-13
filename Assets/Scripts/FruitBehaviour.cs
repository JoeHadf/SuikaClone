using System;
using Unity.Mathematics;
using UnityEngine;

public class FruitBehaviour : MonoBehaviour
{
    [SerializeField] private FruitType type;

    private Collider2D fruitCollider;
    private Rigidbody2D rigidBody;
    
    public FruitPhase phase { get; private set; }
    public bool canLose { get; private set; }
    
    private bool merging;
    private float jarBounds;

    private void Start()
    {
        this.rigidBody = GetComponent<Rigidbody2D>();
        this.fruitCollider = GetComponent<Collider2D>();
        SetGravity();

        canLose = false;
        merging = false;
        jarBounds = BoundsManager.centreDisplacement - BoundsManager.fruitRadii[type];
    }

    private void Update()
    {
        if (phase == FruitPhase.Holding)
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            float mouseX = mousePosition.x;
            
            float sign = Math.Sign(mousePosition.x);
            float fruitX = sign * Math.Min(math.abs(mouseX), jarBounds);
            
            transform.position = new Vector3(fruitX, transform.position.y, 0);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject collisionGameObject = collision.gameObject;
        if (collisionGameObject.CompareTag("Fruit"))
        {
            SetCanLose();
            FruitBehaviour otherFruit = collisionGameObject.GetComponent<FruitBehaviour>();

            if (type == otherFruit.type)
            {
                if (!merging && !otherFruit.merging)
                {
                    SetMerging();
                    otherFruit.SetMerging();
                    
                    MergeManager.MergeFruits(type, gameObject, collisionGameObject);
                }
            }
        }
        else if (collisionGameObject.CompareTag("Wall"))
        {
            SetCanLose();
        }
    }

    private void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 100, 20), ScoreManager.score.ToString());
    }

    public void SetPhase(FruitPhase phaseToSet)
    {
        this.phase = phaseToSet;
        
        SetGravity();
    }

    public FruitType GetFruitType()
    {
        return type;
    }

    private void SetMerging()
    {
        merging = true;
    }

    private void SetCanLose()
    {
        if (!canLose)
        {
            canLose = true;
        }
    }

    private void SetGravity()
    {
        if (rigidBody != null && fruitCollider != null)
        {
            if (phase == FruitPhase.Preview)
            {
                fruitCollider.enabled = false;
                rigidBody.gravityScale = 0;
                rigidBody.velocity = Vector3.zero;
            }
            else if (phase == FruitPhase.Holding)
            {
                fruitCollider.enabled = false;
                rigidBody.gravityScale = 0;
                rigidBody.velocity = Vector3.zero;
            }
            else if(phase == FruitPhase.Falling)
            {
                fruitCollider.enabled = true;
                rigidBody.gravityScale = 1;
            }
        }
    }
}

public enum FruitPhase
{
    Preview,
    Holding,
    Falling
}

public enum FruitType
{
    Null = 0,
    Cherry = 1,
    Strawberry = 2,
    Grape = 3,
    Dekopon = 4,
    Persimmon = 5,
    Apple = 6,
    Pear = 7,
    Peach = 8,
    Pineapple = 9,
    Melon = 10,
    Watermelon = 11
}
