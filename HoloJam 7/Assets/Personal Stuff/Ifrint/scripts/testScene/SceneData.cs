using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName ="Scene/SceneData")]
public class SceneData : ScriptableObject
{
    public List<SceneStep> steps = new List<SceneStep>();
}