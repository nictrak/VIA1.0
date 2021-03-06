using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class RythmSystem : MonoBehaviour
{
    //Constant
    const int DirectionNumber = 4;

    //Serialized Inspector
    [SerializeField]
    private Vector2 basedVelocityVector; // normal velocity of every arrow
    [SerializeField]
    private GameObject arrowSpawner; // object which arrow will spawn at
    [SerializeField]
    private GameObject arrowChecker; // object which mark x value for checking accuracy
    [SerializeField]
    private float xArrowTerminateValue; // float value of x which arrow will terminate if arrow have x above this value
    [SerializeField]
    private int MinSpawnInterval; // minimum frame for waiting spawn the next arrow
    [SerializeField]
    private int MaxSpawnInterval; // maximum frame for waiting spawn the next arrow
    [SerializeField]
    private float checkerRadius;
    [SerializeField]
    private int maxArrowNumber;
    [SerializeField]
    private GameObject centerPoint;
    [SerializeField]
    private int preFrame;
    [SerializeField]
    private int postFrame;

    //four dirtection prefab
    public GameObject UpArrow;
    public GameObject DownArrow;
    public GameObject LeftArrow;
    public GameObject RightArrow;

    //private variable
    private List<GameObject> arrows; // list of all arrows that spawned
    private int currentSpawninterval; // current spawn interval. Should be changed every spawning
    private int spawnCounter; // spawning counter which count frame that dont spawn
    private int currentArrowNumber;
    private bool isSpawnEnable;
    private List<float> accuracies;
    [SerializeField]
    private float averageAccuracy;
    private SystemState currentState;
    [SerializeField]
    private Animator viaAnimator;
    private int stateCounter;
    public enum SystemState
    {
        Pre,
        Solo,
        Post
    }


    public GameObject CenterPoint { get => centerPoint; set => centerPoint = value; }

    public enum ArrowDirection
    {
        Up,
        Down,
        Left,
        Right
    }
    // Start is called before the first frame update
    void Start()
    {
        arrows = new List<GameObject>();
        currentSpawninterval = -1;
        spawnCounter = 0;
        isSpawnEnable = false;
        currentArrowNumber = 0;
        accuracies = new List<float>();
        currentState = SystemState.Pre;
        viaAnimator.Play("Via2Guitar");
        stateCounter = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //Test only
        PlayerHit();
    }
    private void FixedUpdate()
    {
        SpawningPerFrame();
        RemoveArrowPerFrame();
        if (!isSpawnEnable) averageAccuracy = CalculateAverageAccuracy();
        RunStateMachinePerFrame();
    }
    // return one random ArrowDirection. Direction should be correct.
    public ArrowDirection RandomArrowDirection()
    {
        ArrowDirection direction;
        // random direction
        direction = (ArrowDirection)Random.Range(0, DirectionNumber);
        return direction;
    }
    // return one int which betweeen min and max of interval
    public int RandomSpawnInterval()
    {
        int interval;
        // random interval
        interval = Random.Range(MinSpawnInterval, MaxSpawnInterval);
        return interval;
    }
    // Spawn 
    public GameObject Spawn(ArrowDirection direction, Vector2 velocity)
    {
        GameObject selected;
        GameObject spawned;
        // select directiom
        if(direction == ArrowDirection.Up)
        {
            selected = UpArrow;
        }
        else if (direction == ArrowDirection.Down)
        {
            selected = DownArrow;
        }
        else if (direction == ArrowDirection.Left)
        {
            selected = LeftArrow;
        }
        else
        {
            selected = RightArrow;
        }
        // instantiate
        spawned = Instantiate(selected);
        // set position
        spawned.transform.position = arrowSpawner.transform.position;
        RythmArrow arrow = spawned.GetComponent<RythmArrow>();
        // set velocity
        arrow.VelocityVector = velocity;
        // set x terminate value
        arrow.XTerminateValue = xArrowTerminateValue;
        // add it to list
        arrows.Add(spawned);
        return spawned;
    }
    // Execute per fixed frame. Spawning if spawn counter hit spawn interval
    public void SpawningPerFrame()
    {
        if (isSpawnEnable)
        {
            if (currentArrowNumber < maxArrowNumber)
            {
                // random new spawn interval if interval less than minimum
                if (currentSpawninterval < MinSpawnInterval) currentSpawninterval = RandomSpawnInterval();
                // spawn new arrow and reset counter if counter hit the interval. Else add counter
                if (spawnCounter >= currentSpawninterval)
                {
                    Spawn(RandomArrowDirection(), basedVelocityVector);
                    spawnCounter = 0;
                    currentArrowNumber++;
                }
                else
                {
                    spawnCounter++;
                }
            }
            else
            {
                Debug.Log("Average accuracy: " + averageAccuracy);
                isSpawnEnable = false;
            }
        }
    }
    // Execute per fixed frame. Remove 0 index of arrows if it beyond checker radius
    public void RemoveArrowPerFrame()
    {
        // check if arrows not empty
        if(arrows.Count > 0)
        {
            GameObject firstArrow = arrows[0];
            // check if index 0 of arrows beyond terminate value
            if(firstArrow.transform.position.x > arrowChecker.transform.position.x + checkerRadius)
            {
                // remove from list
                arrows.RemoveAt(0);
                //missing report
                AddMissAccuracy();
            }
        }
    }
    // checking whether player hit the arrow correctly. Return true if hit
    public bool PlayerHit()
    {
        bool isHit = false;
        //Terminate if arrows empty
        if (arrows.Count <= 0) return isHit;
        GameObject firstArrow = arrows[0];
        ArrowDirection direction = firstArrow.GetComponent<RythmArrow>().direction;
        if (Input.GetKeyDown(KeyCode.W))
        {
            Debug.Log(IsHitInChecker());
            if(IsHitInChecker() && IsEqualDirection(direction, ArrowDirection.Up))
            {
                isHit = true;
            }
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            if (IsHitInChecker() && IsEqualDirection(direction, ArrowDirection.Left))
            {
                isHit = true;
            }
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            if (IsHitInChecker() && IsEqualDirection(direction, ArrowDirection.Down))
            {
                isHit = true;
            }
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            if (IsHitInChecker() && IsEqualDirection(direction, ArrowDirection.Right))
            {
                isHit = true;
            }
        }
        if (isHit)
        {
            AddHitAccuracy(arrows[0].transform.position);
            arrows.RemoveAt(0);
            Destroy(firstArrow);
        }
        return isHit;
    }
    // return true only when first arrow is in hitbox
    private bool IsHitInChecker()
    {
        GameObject firstArrow = arrows[0];
        float arrowPositionX = firstArrow.transform.position.x;
        float checkerPositionX = arrowChecker.transform.position.x;
        return (arrowPositionX > checkerPositionX - checkerRadius) && (arrowPositionX < checkerPositionX + checkerRadius);
    }
    // return true if direction is equal
    private bool IsEqualDirection(ArrowDirection direction1, ArrowDirection direction2)
    {
        return direction1 == direction2;
    }

    private void AddHitAccuracy(Vector3 arrowPosition)
    {
        float hitDistance = Mathf.Abs(arrowChecker.transform.position.x - arrowPosition.x);
        float accuracy = 100 - hitDistance / checkerRadius * 100;
        accuracies.Add(accuracy);
    }

    private void AddMissAccuracy()
    {
        accuracies.Add(0f);
    }

    private float CalculateAverageAccuracy()
    {
        float accuracy = -1f;
        if (accuracies.Count <= 0) return accuracy;
        float sumAccuracy = 0;
        for (int i = 0; i < accuracies.Count; i++)
        {
            sumAccuracy += accuracies[i];
        }
        accuracy = sumAccuracy / accuracies.Count;
        return accuracy;
    }

    void OnDrawGizmos()
    {
        // Draw a yellow cube at the transform position
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(arrowChecker.transform.position, new Vector3(checkerRadius, checkerRadius, checkerRadius));
    }
    private void RunStateMachinePerFrame()
    {
        if(currentState == SystemState.Post)
        {
            if (stateCounter >= postFrame)
            {
                SceneManager.LoadScene("S1-Toturial");
                stateCounter = 0;
            }
            else stateCounter++;
        }
        else if (currentState == SystemState.Pre)
        {
            if (stateCounter >= preFrame)
            {
                currentState = SystemState.Solo;
                isSpawnEnable = true;
                viaAnimator.Play("SoloLoop");
                stateCounter = 0;
            }
            else stateCounter++;
        }
        else if (currentState == SystemState.Solo)
        {
            if(arrows.Count == 0 && currentArrowNumber == maxArrowNumber)
            {
                currentState = SystemState.Post;
                currentArrowNumber = 0;
                viaAnimator.Play("Guitar2Via");
            }
        }
    }
}
