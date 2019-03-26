using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// Allows creature to convert one resource into another.
/// </summary>
public class Convert : Action
{
    public string startResource;

    public string endResource;

    /// <summary>
    /// The amount of startResource to be converted.
    /// </summary>
    public float amtToConvert;

    /// <summary>
    /// The amount of endResource is this multiple of the amount of start resource.
    /// </summary>
    public float multiplier;


    /// <summary>
    /// convert a particular amount of startResource into endResource
    /// </summary>
    public override void perform(Creature creature, Ecosystem eco)
    {
        float initialAmt = creature.storedResources[startResource].currentLevel;
        float amountConverted;
        // handle when there is not enough resource stored
        if(initialAmt < amtToConvert)
        {
            amountConverted = initialAmt;
        }
        else
        {
            amountConverted = amtToConvert;
        }

        creature.storedResources[startResource].currentLevel -= amountConverted;

        creature.storedResources[endResource].currentLevel += amountConverted * multiplier;
    }

}