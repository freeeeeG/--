using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;
using System;

public class PublicComponentsControllers : MonoBehaviour, IController
{
    private void Awake()
    {
        this.RegisterEvent<GameStartEvent>(e =>
        {
            Debug.Log("PublicComponentsControllers");
        });
    }

    private void Update()
    {

    }

    public IArchitecture GetArchitecture()
    {
        return PublicComponents.Interface;
    }
}
