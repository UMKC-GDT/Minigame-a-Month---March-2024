using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "BehaviorTree/TreeAsset")]
public class BehaviorTreeAsset : ScriptableObject {
    public List<BTNodeData> nodes = new List<BTNodeData>();
}

[System.Serializable]
public class BTNodeData {
    public string typeName;
    public Vector2 position;
    public List<int> childrenIndices = new List<int>();
}
