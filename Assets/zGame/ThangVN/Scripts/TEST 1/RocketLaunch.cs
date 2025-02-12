using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketLauch : MonoBehaviour
{
    public float moveSpeed = 20f;


    bool isNotCheck = false;
    Vector3 directionMove = Vector3.zero;
    Vector3 directionLookAt = Vector3.zero;

    Vector3 posTarget;
    bool facingRight = false;

    public float directionAngle;
    public float directionLookAtAngle;

    float turn = 0;
    float lastTurn = 0;
    private float radius;

    private float damage;

    //[SerializeField] private TrailRenderer triler;
    private float giaTocA;

    // Test
    [SerializeField] Transform target;

    void Start()
    {
        Init(target.position, moveSpeed, damage);
    }

    public void Init(Vector3 target, float speed, float damage)
    {
        this.posTarget = target;
        this.moveSpeed = speed;
        this.damage = damage;
        //triler.Clear();
        turn = 0;
        lastTurn = 0;
        isNotCheck = false;
        directionMove = Vector3.zero;
        directionLookAt = Vector3.zero;
        directionAngle = 0;
        directionLookAtAngle = 0;
        giaTocA = 1;
    }

    void Update()
    {
        float deltaTime = Time.deltaTime;

        if (!isNotCheck)
        {
            if (directionMove == Vector3.zero)
            {
                GetDirectionToTarget(transform.position, posTarget, facingRight, out Vector2 dirMove);
                dirMove = dirMove * -1f;
                directionMove = directionLookAt = dirMove;

                float angleNow = Mathf.Atan2(dirMove.y, dirMove.x) * Mathf.Rad2Deg;
                directionAngle = directionLookAtAngle = angleNow;
            }

            GetDirectionToTarget(transform.position, posTarget, facingRight, out Vector2 dirTargetNew);

            float angle = Mathf.Atan2(dirTargetNew.y, dirTargetNew.x) * Mathf.Rad2Deg;
            Quaternion oldAngle = new Quaternion();
            oldAngle.eulerAngles = new Vector3(0, 0, directionLookAtAngle);
            Quaternion newAngle = new Quaternion();
            newAngle.eulerAngles = new Vector3(0, 0, angle);
            float tileApplyAngle = deltaTime * turn;
            if (tileApplyAngle > 1f)
            {
                tileApplyAngle = 1;
                isNotCheck = true;
            }

            Quaternion quaternion = Quaternion.Lerp(oldAngle, newAngle, tileApplyAngle);
            transform.eulerAngles = quaternion.eulerAngles;
            directionAngle = directionLookAtAngle = quaternion.eulerAngles.z;

            directionMove = directionLookAt
                = Quaternion.AngleAxis(quaternion.eulerAngles.z, Vector3.forward) * Vector3.right;


            if (turn < 40f)
            {
                lastTurn += deltaTime * deltaTime * 150f;
                turn += lastTurn;
            }
            else
            {
                isNotCheck = true;
            }
        }

        Vector3 posNew = transform.position;
        giaTocA += Time.deltaTime;
        float step = moveSpeed * deltaTime * giaTocA;
        posNew += new Vector3(directionMove.x * step, directionMove.y * step, 0);

        transform.position = posNew;
        //--------------------- check raycast
        // radius = Time.deltaTime;

        //RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.up, radius);


        //if (hit.collider != null)
        //{

        //    if (hit.collider.CompareTag("Enemy") || hit.collider.CompareTag("Wall"))
        //    {
        //        // Get Pool 
        //        //GameObject g = Law.Pool.ins.GetEffectRocket();
        //        GameObject g = new GameObject();
        //        g.gameObject.transform.position = this.transform.position;

        //        // Effect
        //        //if (g.TryGetComponent(out AOEEffect aoeEffect))
        //        if (g.TryGetComponent(out GameObject aoeEffect))
        //        {
        //            g.SetActive(true);
        //            //aoeEffect.Init(this.damage);
        //            this.gameObject.SetActive(false);
        //        }

        //    }

        //}
    }


    void GetDirectionToTarget(Vector3 startPos, Vector3 endPos, bool isRight, out Vector2 resulf)
    {
        float xDirec = 0;
        float yDirec = 0;
        Vector3 tXFollow = endPos - startPos;
        resulf = (new Vector2(tXFollow.x, tXFollow.y)).normalized;
    }
}