using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;

public class ActivationTest {
    Creature creat;
    Network network;
    OutputNode outNode;
    double activInput;

    [SetUp]
    protected void SetUp()
    {
        // Use the Assert class to test conditions.
        creat = new Creature();
        network = new Network();
        network.net.Add(new List<Node>());
        network.net.Add(new List<Node>());
        BiasNode inNode1 = new BiasNode();
        inNode1.value = 2;
        BiasNode inNode2 = new BiasNode();
        inNode2.value = 3;

        network.net[0].Add(inNode1);
        network.net[0].Add(inNode2);

        outNode = new OutputNode(network, creat, 1);
        activInput = 2 * outNode.weights[0] + 3 * outNode.weights[1];
    }

    [Test]
    public void LogisticActivFunctCorrect() {

        outNode.setActivBehavior(new LogisticActivBehavior());
        network.net[1].Add(outNode);
        outNode.updateValue();

        double predicted = 1 / (1 + System.Math.Exp(-1 * activInput));

        Assert.AreEqual(predicted, outNode.value, .000001);
    }

    [Test]
    public void tanhActivFunctCorrect()
    {
        outNode.setActivBehavior(new TanhActivBehav());
        network.net[1].Add(outNode);
        outNode.updateValue();

        double predicted = System.Math.Tanh(activInput);

        Assert.AreEqual(predicted, outNode.value, .000001);
    }

    /*
    // A UnityTest behaves like a coroutine in PlayMode
    // and allows you to yield null to skip a frame in EditMode
    [UnityTest]
    public IEnumerator ActivationTestWithEnumeratorPasses() {
        // Use the Assert class to test conditions.
        // yield to skip a frame
        yield return null;
    }
    */
}
