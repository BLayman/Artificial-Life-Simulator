using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAction : Action
{
    public int direction;

    public MoveAction() { }

    public MoveAction(int dir)
    {
        direction = dir;
    }

    public override void perform(Creature creature)
    {
        throw new System.NotImplementedException();
    }

    
}