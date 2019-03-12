// Artificial Life Simulator
// Copyright (c) 2019 Brett Layman
// This file is subject to the terms and conditions defined in 'LICENSE.txt', which is part of this source code repository.

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