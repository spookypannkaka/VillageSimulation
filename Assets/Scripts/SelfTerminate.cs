using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfTerminate : MonoBehaviour
{
    public void DestroySelf() {
        Destroy(gameObject);
    }

}
