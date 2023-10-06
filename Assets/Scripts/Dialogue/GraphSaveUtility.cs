using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class GraphSaveUtility
{
    private DialogueGraphView _targetGraphView;
    private List<Edge> edges => _targetGraphView.edges.ToList();
    private List<DialogueNode> nodes => _targetGraphView.nodes.ToList().Cast<DialogueNode>().ToList();
    private DialogueContainer _containerCache;
    public static GraphSaveUtility GetInstance(DialogueGraphView targetGraphView)
    {
        return new GraphSaveUtility
        {
            _targetGraphView = targetGraphView
    };
    }

    public void SaveGraph(string filename)
    {
        if(!edges.Any()) return;

        var dialogueContainer = ScriptableObject.CreateInstance<DialogueContainer>();
        var connectedPorts = edges.Where(x => x.input.node != null).ToArray();
        for (var i = 0; i < connectedPorts.Length; i++)
        {
            var outputNode = connectedPorts[i].output.node as DialogueNode;
            var inputNode = connectedPorts[i].input.node as DialogueNode;
            dialogueContainer.NodeLinks.Add(new NodeLinkData
            {
                BaseNodeGUID = outputNode.GUID,
                PortName = connectedPorts[i].output.portName,
                TargetNodeGUID = inputNode.GUID
            }) ;
        }
        foreach(var dialogueNode in nodes.Where(node => !node.EntryPoint))
        {
            dialogueContainer.DialogueNodeData.Add(new DialogueNodeData
            {
                GUID = dialogueNode.GUID,
                DialogueText = dialogueNode.DialogueText,
                Position = dialogueNode.GetPosition().position
            });
        }

        //Auto makes dialogue folder
        if (!AssetDatabase.IsValidFolder("Assets/Dialogues"))
            AssetDatabase.CreateFolder("Assets", "Dialogues");
        AssetDatabase.CreateAsset(dialogueContainer, $"Assets/Dialogues/{filename}.asset");
        AssetDatabase.SaveAssets();
    }
    public void LoadGraph(string filename)
    {
        _containerCache = AssetDatabase.LoadAssetAtPath<DialogueContainer>($"Assets/Dialogues/{filename}.asset");
        if(_containerCache == null)
        {
            EditorUtility.DisplayDialog("File Not Found", "Target dialogue graph file doesnt exist", "ok lol");
            return;
        }

        ClearGraph();
        CreateNodes();
        ConnectNodes();
    }

    private void ConnectNodes()
    {
        for (var i = 0; i < nodes.Count; i++)
        {
            var connections = _containerCache.NodeLinks.Where(x => x.BaseNodeGUID == nodes[i].GUID).ToList();
            for (var j = 0; j < connections.Count; j++)
            {
                var targetNodeGuid = connections[j].TargetNodeGUID;
                var targetNode = nodes.First(x => x.GUID == targetNodeGuid);
                LinkNodes(nodes[i].outputContainer[j].Q<Port>(),(Port) targetNode.inputContainer[0]);
                targetNode.SetPosition(new Rect(
                    _containerCache.DialogueNodeData.First(x => x.GUID == targetNodeGuid).Position,
                    _targetGraphView.defaultNodeSize
                    ));
            }
        }

        nodes.Find(x => x.EntryPoint).GUID = _containerCache.NodeLinks[0].BaseNodeGUID;
        foreach(var node in nodes)
        {
            if (node.EntryPoint) return;
            edges.Where(x => x.input.node == node).ToList()
                .ForEach(edge => _targetGraphView.RemoveElement(edge));
            _targetGraphView.RemoveElement(node);
        }

    }

    private void LinkNodes(Port output, Port input)
    {
        var tempEdge = new Edge
        {
            output = output,
            input = input
        };

        tempEdge.input.Connect(tempEdge);
        tempEdge.output.Connect(tempEdge);
        _targetGraphView.Add(tempEdge);
    }

    private void CreateNodes()
    {
        foreach(var nodeData in _containerCache.DialogueNodeData)
        {
            var tempNode = _targetGraphView.CreateDialogueNode(nodeData.DialogueText);
            tempNode.GUID = nodeData.GUID;
            _targetGraphView.AddElement(tempNode);

            var nodePorts = _containerCache.NodeLinks.Where(x => x.BaseNodeGUID == nodeData.GUID).ToList();
            nodePorts.ForEach(x => _targetGraphView.AddChoicePort(tempNode, x.PortName));
        }
    }

    private void ClearGraph()
    {
        nodes.Find(x => x.EntryPoint).GUID = _containerCache.NodeLinks.Find(x => x.PortName == "Next").BaseNodeGUID;
        //nodes.Find(x => x.EntryPoint).GUID = _containerCache.NodeLinks[0].BaseNodeGUID;
        foreach(var node in nodes)
        {
            if (node.EntryPoint) continue;
            edges.Where(x => x.input.node == node).ToList().ForEach(
                edge => _targetGraphView.RemoveElement(edge));

            _targetGraphView.RemoveElement(node);
        }
    }
}
