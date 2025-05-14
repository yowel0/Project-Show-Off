using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SMItemDropper : MonoBehaviour
{
    [Header("Balancing")]
    [Tooltip("Time between reaching destination and dropping item")]
    [SerializeField] float prepareTime;
    [Tooltip("Time between dropping item and moving to next destination")]
    [SerializeField] float cooldownTime;
    [Tooltip("Time it takes to move to the next destination")]
    [SerializeField] float moveTime;

    [Tooltip("Minimum distance a new position will have")]
    [SerializeField] float minDistanceToLast;

    [Header("Setup")]
    [SerializeField] Transform minLocationTransform;
    [SerializeField] Transform maxLocationTransform;
    [SerializeField] float hoverHeight;
    [SerializeField] GameObject[] items;

    Queue<int> itemQueue = new Queue<int>();

    Vector3 minLocation;
    Vector3 maxLocation;
    Vector3 lastLocation;
    Vector3 newLocation;
    float timer;

    enum State
    {
        moving,
        preparing,
        cooldown,
        idle,
        pregame
    }
    State currentState;

    private void Start()
    {
        currentState = State.idle;
        if (hoverHeight <= 0) hoverHeight = transform.position.y;
        minLocation = new Vector3(minLocationTransform.position.x, hoverHeight, minLocationTransform.position.z);
        maxLocation = new Vector3(maxLocationTransform.position.x, hoverHeight, maxLocationTransform.position.z);
    }

    public void PrepareGame()
    {
        timer = 0;
        currentState = State.pregame;
        lastLocation = transform.position;
        newLocation = (minLocation + maxLocation) / 2f;
    }
    public void StartGame()
    {
        currentState = State.moving;
        newLocation = GetNextPosition();
        timer = 0;
    }

    public void StopGame()
    {
        currentState = State.idle;
    }

    private void FixedUpdate()
    {
        switch (currentState)
        {
            case State.moving: Movement(); break;

            case State.preparing: 
                if (timer >= prepareTime)
                {
                    DropItem();
                    currentState = State.cooldown;
                    timer -= prepareTime;
                }
                break;

            case State.cooldown:
                if (timer >= cooldownTime)
                {
                    newLocation = GetNextPosition();
                    currentState = State.moving;
                    timer -= cooldownTime;
                }
                break;

            case State.pregame:
                transform.position = Vector3.Lerp(lastLocation, newLocation, timer/3f);
                break;

            default:
                timer = 0;
                break;
        }


        timer += Time.fixedDeltaTime;
    }

    void Movement()
    {
        float percent = timer / moveTime;
        transform.position = Vector3.Lerp(lastLocation, newLocation, percent);

        if (percent > 1)
        {
            currentState = State.preparing;
            timer -= moveTime;
        }
    }

    /*IEnumerator StartCycle()
    {
        Vector3 newPos = GetNextPosition();
        
        // Moving to place
        yield return new WaitForSeconds(prepareTime);

        DropItem();

        yield return new WaitForSeconds(cooldownTime);

        // Repeat

    }*/


    void DropItem()
    {
        int itemID = GetRandomItem();

        GameObject itemToDrop = Instantiate(items[itemID], transform.position, Quaternion.identity);


    }

    int GetRandomItem()
    {
        if (itemQueue.Count == 0) AddItemsToQueue();

        return itemQueue.Dequeue();
    }


    void AddItemsToQueue()
    {
        List<int> _items = new List<int>();

        for (int i = 0; i < PlayerManager.Instance.GetPlayerCount(); i++)
        {
            _items.Add(i);
            _items.Add(i);
        }

        for (int i = _items.Count; i > 0; i--)
        {
            int rand = Random.Range(0, i);
            itemQueue.Enqueue(_items[rand]);
            _items.RemoveAt(rand);
        }

    }

    private Vector3 GetNextPosition(float depth = 0)
    {
        // if randomness goes on for too long, just drop in the middle
        if (depth >= 5) return (minLocation+maxLocation)/2f;
        float randX = Random.Range(minLocation.x, maxLocation.x);
        float randZ = Random.Range(minLocation.z, maxLocation.z);

        Vector3 nextPos = new Vector3(randX, hoverHeight, randZ);
        if (Vector3.Distance(lastLocation, nextPos) < minDistanceToLast) nextPos = GetNextPosition(depth+1);

        lastLocation = transform.position;
        return nextPos;
    }



}
