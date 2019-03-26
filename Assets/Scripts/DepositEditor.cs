using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class DepositEditor : ActionEditorAbstract
{
    Deposit actionRef;

    public DepositEditor(Deposit inAction)
    {
        action = inAction;
        actionRef = inAction;
    }

    public void setNeighborIndex(int index)
    {
        actionRef.neighborIndex = index;
    }

    public void setDepositResource(string res)
    {
        actionRef.resourceToDeposit = res;
    }

    public void setAmtToDeposit(float amt)
    {
        actionRef.amountToDeposit = amt;
    }

}