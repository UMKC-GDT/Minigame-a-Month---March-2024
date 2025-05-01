using System;
using System.Collections.Generic;
using BehaviorTree;
using UnityEngine;

public class BossAgent : MonoBehaviour {
    public BehaviorTreeAsset treeAsset;
    private BTNode root;
    private Blackboard blackboard;

    void Start() {
        blackboard = new Blackboard {
            agent = this.transform,
            enemy = GameObject.FindWithTag("Enemy"),
            enemyGrabbed = false
        };

        root = BuildTreeFromAsset(treeAsset);
    }

    void Update() {
        root?.Tick();
    }

    BTNode BuildTreeFromAsset(BehaviorTreeAsset asset) {
        List<BTNode> builtNodes = new List<BTNode>();

        // First pass: create nodes
        foreach (var data in asset.nodes) {
            Type nodeType = Type.GetType("BehaviorTree." + data.typeName);
            if (nodeType == null) {
                Debug.LogError($"Node type {data.typeName} not found");
                continue;
            }

            BTNode node = null;

            // Create node and pass blackboard if needed
            if (typeof(BTNode).IsAssignableFrom(nodeType)) {
                var constructor = nodeType.GetConstructor(new[] { typeof(Vector2), typeof(Blackboard) });
                if (constructor != null)
                    node = (BTNode)constructor.Invoke(new object[] { data.position, blackboard });
                else
                    node = (BTNode)Activator.CreateInstance(nodeType, data.position);
            }

            builtNodes.Add(node);
        }

        // Second pass: link children
        for (int i = 0; i < builtNodes.Count; i++) {
            var nodeData = asset.nodes[i];
            foreach (var childIndex in nodeData.childrenIndices) {
                builtNodes[i].AddChild(builtNodes[childIndex]);
            }
        }

        return builtNodes.Count > 0 ? builtNodes[0] : null;
    }
}
