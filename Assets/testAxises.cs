using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testAxises : MonoBehaviour
{
    public GameObject prefab;
    public int Q, R; //center?
    public Vector2Int Direction;

    [SerializeField]
    private Vector3 resultInCube;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        AxialCoordinate neighbor = new AxialCoordinate(Q + Direction.x, R + Direction.y);
        Vector3 worldPosition = neighbor.ToWorldPosition;
        Debug.DrawRay(transform.position, worldPosition, Color.red);
    }
}
