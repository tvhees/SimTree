using UnityEngine;
using System.Collections;

public class Menu : MonoBehaviour {

    public Camera menuCamera;

    void Awake() {
        PlayerManager.Instance.menuCamera = menuCamera;
    }
}
