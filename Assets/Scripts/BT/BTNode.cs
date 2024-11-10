using System.Collections.Generic;
using UnityEngine;

public abstract class BTNode
{
    public abstract bool Execute(VillagerController villager);
}

public class Selector : BTNode
{
    private List<BTNode> nodes;

    public Selector(List<BTNode> nodes) { this.nodes = nodes; }

    public override bool Execute(VillagerController villager)
    {
        foreach (var node in nodes)
        {
            if (node.Execute(villager)) return true;
        }
        return false;
    }
}

public class Sequence : BTNode
{
    private List<BTNode> nodes;

    public Sequence(List<BTNode> nodes) { this.nodes = nodes; }

    public override bool Execute(VillagerController villager)
    {
        foreach (var node in nodes)
        {
            if (!node.Execute(villager)) return false;
        }
        return true;
    }
}
