using UnityEngine;

public class AIControl : MonoBehaviour {

    GameObject[] goalLocations;
    UnityEngine.AI.NavMeshAgent agent;
    Animator anim;
    float speedMult;
    float detecttionRadius = 5;
    float fleeRadius = 10;

    void ResetAgent()
    {
        speedMult = Random.Range(0.1f, 1.5f);
        agent.speed = 2 * speedMult;
        agent.angularSpeed = 120;
        anim.SetFloat("speedMult", speedMult);
        anim.SetTrigger("isWalking");
        agent.ResetPath();
    }

    public void DetectNewObstacle(Vector3 position)
    {
        if(Vector3.Distance(position, this.transform.position) < detecttionRadius)
        {
            Vector3 fleeDirection = (this.transform.position - position).normalized;
            Vector3 newgoal = this.transform.position + fleeDirection * fleeRadius;

            UnityEngine.AI.NavMeshPath path = new UnityEngine.AI.NavMeshPath();
            agent.CalculatePath(newgoal, path);

            if(path.status != UnityEngine.AI.NavMeshPathStatus.PathInvalid)
            {
                agent.SetDestination(path.corners[path.corners.Length - 1]);
                anim.SetTrigger("isRunning");
                agent.speed = 10;
                agent.angularSpeed = 500;
            }
        }
    }

    // Use this for initialization
    void Start() {
        goalLocations = GameObject.FindGameObjectsWithTag("goal");
        agent = this.GetComponent<UnityEngine.AI.NavMeshAgent>();
        agent.SetDestination(goalLocations[Random.Range(0, goalLocations.Length)].transform.position);
        anim = this.GetComponent<Animator>();
        anim.SetFloat("wOffset", Random.Range(0, 1));
        ResetAgent();
        //anim.SetTrigger("isWalking");
        //float sm = Random.Range(0.1f, 1.5f);
        //anim.SetFloat("speedMult", sm);
        //agent.speed *= sm;
    }

    // Update is called once per frame
    void Update() {

        if (agent.remainingDistance < 1) 
        {
            ResetAgent();
            agent.SetDestination(goalLocations[Random.Range(0, goalLocations.Length)].transform.position);
        }
    }
}
