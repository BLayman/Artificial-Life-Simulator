// Artificial Life Simulator
// Copyright (c) 2019 Brett Layman
// This file is subject to the terms and conditions defined in 'LICENSE.txt', which is part of this source code repository.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// The comm network must individually process each comm signal stored in the creatures commList, and save the outputs as action recommendations.
/// </summary>


public class CommSignal
{
    /// <summary>
    /// Stores message, position, and phenotype.
    /// </summary>
    public Dictionary<string, bool[]> commProperties;
}