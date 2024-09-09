using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Node
{
    public int index;
    Node next;
}

public class TestStack : MonoBehaviour
{
    public Stack<Node> stack;
    public void Pop(Node node)
    {

    }
}
