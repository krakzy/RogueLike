using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Actor))]

public class Enemy : MonoBehaviour
{
    // Start is called before the first frame update
    private void Start()
    {
        GameManager.Get.AddEnemy(GetComponent<Actor>());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
