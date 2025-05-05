using System.Collections.Generic;
using BehaviorTree;
using UnityEngine;
using System.Linq;
using System;
using UnityEngine.Events;

public class BossAgent : MonoBehaviour {
    public BehaviorTreeAsset treeAsset;
    public Graph graph;
    public GameObject player;
    public GameObject hand;
    public Node doorNode;
    public float grabRange = 3f;

    private Blackboard blackboard;
    private BTNode root;

    public UnityEvent onStartChasingPlayer;
    public UnityEvent onGrabPlayer;
    public UnityEvent onReturnToDoor;


    void Start() {
        // Setup shared data
        blackboard = new Blackboard {
            agent = this.transform,
            enemy = player,
            hand = hand,
            currentTarget = player,
            door = doorNode,
            grabRange = 2.1f,
            arrivalRange = 2.0f,
            moveSpeed = 20f,
            enemyGrabbed = false,
            graph = graph
        };

        root = BuildTreeFromAsset(treeAsset);
    }

    void FixedUpdate() {
        root?.Tick();
        if(blackboard.enemyGrabbed && PlayerController.instance.isPaused == false) {
            PlayerController.instance.pauseControls();
            PlayerController.instance.gameObject.transform.SetParent(transform);
        }
    }

    BTNode BuildTreeFromAsset(BehaviorTreeAsset asset) {
        List<BTNode> builtNodes = new List<BTNode>();

        // First pass: create node instances
        foreach (var data in asset.nodes) {
            Type nodeType = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .FirstOrDefault(t => t.Name == data.typeName && typeof(BTNode).IsAssignableFrom(t));

            BTNode node = null;

            if (nodeType != null) {
                var constructor = nodeType.GetConstructor(new[] { typeof(Vector2), typeof(Blackboard) });
                if (constructor != null)
                    node = (BTNode)constructor.Invoke(new object[] { data.position, blackboard });
                else
                    node = (BTNode)Activator.CreateInstance(nodeType, data.position);
            }

            if (node == null)
                Debug.LogWarning($"Could not create node: {data.typeName}");

            builtNodes.Add(node);
        }

        // Second pass: hook up children
        for (int i = 0; i < asset.nodes.Count; i++) {
            foreach (int childIndex in asset.nodes[i].childrenIndices) {
                if (childIndex >= 0 && childIndex < builtNodes.Count)
                    builtNodes[i].AddChild(builtNodes[childIndex]);
            }
        }

        return builtNodes.Count > 0 ? builtNodes[0] : null;
    }
}
