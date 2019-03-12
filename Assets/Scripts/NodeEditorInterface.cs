// Artificial Life Simulator
// Copyright (c) 2019 Brett Layman
// This file is subject to the terms and conditions defined in 'LICENSE.txt', which is part of this source code repository.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// Interface that allows all specific node creator objects to be stored by NodeCreator class.
/// </summary>
public interface NodeEditorInterface
{
    Node getNode();
}