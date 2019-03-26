using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class ConvertEditor : ActionEditorAbstract
{
    Convert actionRef;

    public ConvertEditor(Convert inAction)
    {
        action = inAction;
        actionRef = inAction;
    }

    public void setStartResource(string res)
    {
        actionRef.startResource = res;
    }

    public void setEndResource(string res)
    {
        actionRef.endResource = res;
    }

    public void setAmtToConvert(float amt)
    {
        actionRef.amtToConvert = amt;
    }

    public void setMultiplier(float multi)
    {
        actionRef.multiplier = multi;
    }
}