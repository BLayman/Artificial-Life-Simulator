using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class MoveActionEditor: ActionEditorAbstract
{
    MoveAction actionRef;
    public MoveActionEditor(MoveAction a)
    {
        action = a;
        actionRef = a;
    }

    public void setDirection(moveDir direction)
    {
        actionRef.direction = direction;
    }



}