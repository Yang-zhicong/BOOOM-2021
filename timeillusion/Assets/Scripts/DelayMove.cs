using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DelayMove : MonoBehaviour
{


    public float speed;
    public float delay;

    public Transform body;
    public Transform soul;


    public GameObject playerBody;


    private Queue<Vector3> positions;

    private void Awake()
    {

        float fixedTime = Time.fixedDeltaTime;
        int delayCount = Mathf.CeilToInt(delay / fixedTime);
        positions = new Queue<Vector3>(delayCount);
        for(int i = 0; i< delayCount; i++)
        {
            positions.Enqueue(soul.position);
        }
    }

    private void FixedUpdate()
    {
        positions.Enqueue(soul.position);
        lastSoulPos = positions.Dequeue();
    }

    // Update is called once per frame
    void Update()
    {
        SoulMove();
        BodyMove();
        
    }

    void SoulMove()
    {
        Vector2 move = Vector2.zero;
        if (Input.GetKey(KeyCode.W))
        {
            move.y++;
        }
        if (Input.GetKey(KeyCode.S))
        {
            move.y--;
        }
        if (Input.GetKey(KeyCode.D))
        {
            move.x++;
        }
        if (Input.GetKey(KeyCode.A))
        {
            move.x--;
        }
        move = move.normalized * speed;
        Vector3 pos = soul.position;
        pos.x += move.x;
        pos.z += move.y;
        soul.position = pos;
    }

    Vector3 lastSoulPos;
    void BodyMove()
    {
        float scale = (Time.time - Time.fixedTime) / Time.fixedDeltaTime;
        Vector3 pos = Vector3.Lerp(body.position, lastSoulPos, scale);
        body.position = lastSoulPos;
    }
}
