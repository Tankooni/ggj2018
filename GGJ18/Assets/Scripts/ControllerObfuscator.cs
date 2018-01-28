using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class ControllerObfuscator : MonoBehaviour {

    // Use this for initialization
    void Start () {
        GameObject controller =
            this.gameObject;

        VRTK_ObjectAppearance.SetRendererHidden(controller);
    }
}
