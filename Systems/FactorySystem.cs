using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;
using System;

interface IGood
{
    void ReturnFactory();
}

public abstract class AbstractGood : MonoBehaviour,IGood
{
    public void ReturnFactory()
    {
        Debug.Log("ReturnFactory");
        PublicComponents.Interface.GetSystem<FactorySystem>().Good2Factory(this.gameObject, this.GetType());
        this.gameObject.SetActive(false);
    }

}


interface IFactorySystem : ISystem
{
    GameObject UseGoods<T>() where T : AbstractGood;
}

public class FactorySystem : AbstractSystem, IFactorySystem
{
    public Dictionary<Type, Stack<GameObject>> FactoryMap = new Dictionary<Type, Stack<GameObject>>();

    protected override void OnInit() { }

    public GameObject UseGoods<T>() where T : AbstractGood
    {
        Debug.Log(typeof(T).Name);
        if (FactoryMap.ContainsKey(typeof(T)) && FactoryMap[typeof(T)].Count > 0)
        {
            Debug.Log(FactoryMap[typeof(T)].Count);
            return FactoryMap[typeof(T)].Pop();
        }
        else
        {
            Debug.Log(typeof(T).Name);
            if(!FactoryMap.ContainsKey(typeof(T)))
            FactoryMap.Add(typeof(T), new Stack<GameObject>());
            for(int i = 0; i < 10; i++)
            {
                GameObject go = GameObject.Instantiate(Resources.Load<GameObject>(typeof(T).Name));
                go.SetActive(false);
                FactoryMap[typeof(T)].Push(go);
                Debug.Log(FactoryMap[typeof(T)].Count);
            }
            Debug.Log(FactoryMap[typeof(T)].Count);
            return FactoryMap[typeof(T)].Pop();
        }
    }
    public void Good2Factory(GameObject good, Type type)
    {
        if (FactoryMap.ContainsKey(type))
        {
            FactoryMap[type].Push(good);
        }
        else
        {
            FactoryMap.Add(type, new Stack<GameObject>());
            FactoryMap[type].Push(good);
        }
    }
}
