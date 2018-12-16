using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class MoveActionCreator: ActionCreatorAbstract
{
    MoveAction actionRef;
    public MoveActionCreator(MoveAction a)
    {
        action = a;
        actionRef = a;
    }

    public void setDirection(moveDir direction)
    {
        actionRef.direction = direction;
    }



}