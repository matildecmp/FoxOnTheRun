using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("Enemy references")]
    private Animator enemyAnim;
    private Rigidbody rb;
    public bool isStaticBear = false; 
    public float speed;
    public GameObject collisionSound;
    private bool isWalking = false;

    [Header("Player references")]
    public Transform player;       
    private float alertThresh;

    // Before Player crosses a certain threshold, Enemies are idle
    void Start()
    {
        enemyAnim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        player = playerObj.transform;

        if (gameObject.name.Contains("Bear"))
        {
            enemyAnim.Play("Bear_IdleCombat"); 
            alertThresh = 60f;
            speed = 5f;
        } 
        else if (gameObject.name.Contains("Sparrow"))
        {
            enemyAnim.Play("Idle_A"); 
            alertThresh = 20f;
            speed = 6f;
        }
    }

    // When Player crosses the threshold, Enemies (except for Static Bears) start walking towards him 
    void Update()
    {
        if (!isStaticBear && !isWalking)
        {
            float distance = Vector3.Distance(transform.position, player.position);

            if (distance <= alertThresh)
            {
                if (gameObject.name.Contains("Bear")){
                    enemyAnim.Play("Bear_WalkForward");
                } else if (gameObject.name.Contains("Sparrow")){
                    enemyAnim.Play("Fly"); 
                }
                isWalking = true;
            }
        }

        if (isWalking)
        {
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }
    }

    // When Player collides with Enemies, play a sound (if Enemy is a Bear, also play the corresponding animation)
    void OnTriggerEnter(Collider other)
    {
        PlayerController playerController = other.GetComponent<PlayerController>();

        if (other.CompareTag("Player") && !playerController.isPowered && playerController.isAlive)
        {
            if (gameObject.name.Contains("Bear"))
            {
                enemyAnim.Play("Bear_GetHitFromFront");
            }
            GameObject sound = Instantiate(collisionSound, transform.position, Quaternion.identity);
            sound.GetComponent<AudioSource>().Play();
            Destroy(sound, 2f);
        } 
    }
}
