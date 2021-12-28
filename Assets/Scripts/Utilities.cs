using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utilities : MonoBehaviour
{
    void DestroyYourself() {
        Destroy(this.gameObject);
    }

    void DisableYourself() {
        this.gameObject.SetActive(false);
    }
}
