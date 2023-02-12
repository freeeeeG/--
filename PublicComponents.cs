using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;
public class PublicComponents : Architecture<PublicComponents>
{
    protected override void Init()
    {
        this.RegisterSystem<FactorySystem>(new FactorySystem());
    }
}

