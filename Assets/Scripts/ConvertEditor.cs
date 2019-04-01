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

<<<<<<< .merge_file_a12308
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
=======
    public void addStartResource(string res, int coefficient)
    {
        actionRef.startResources[res] = coefficient;
    }

    public void addEndResource(string res, int coefficient)
    {
        actionRef.endResources[res] = coefficient;
    }

    public void setAmtToProduce(float amt)
    {
        actionRef.amtToProduce = amt;
    }

>>>>>>> .merge_file_a16464
}