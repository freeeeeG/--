using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;

public class MultiplayerGamePlane : MonoBehaviour, IController
{

    private void Awake() {
        
    }
    private void Start() {
        gameObject.SetActive(false);
    }

    public IArchitecture GetArchitecture()
    {
        return Blockade.Interface;
    }
}
