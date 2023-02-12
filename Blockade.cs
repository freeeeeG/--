using UnityEngine;
using QFramework;
public class Blockade : Architecture<Blockade>
{
    protected override void Init()
    {
        Debug.Log("Blockade Init");
        this.RegisterSystem<NetWorkSystem>(new NetWorkSystem());
        this.RegisterSystem<AchievementSystem>(new AchievementSystem());
    }
}
