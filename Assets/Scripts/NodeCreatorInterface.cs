using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// Interface that allows all specific node creator objects to be stored by NodeCreator class.
/// </summary>
public interface NodeCreatorInterface
{
    Node getNode();
}