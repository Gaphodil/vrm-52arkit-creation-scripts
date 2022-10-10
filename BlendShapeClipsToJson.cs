using System.IO;
using UnityEditor;
using UnityEngine;
using VRM;

namespace Assets
{
    public class BlendShapeClipsToJson : MonoBehaviour
    {
        public string settingsFile = "Assets/52script_export_settings.json";
        public bool prettyPrint = true;

        [System.Serializable]
        public struct JsonBinding
        {
            public string blendshape;
            public float weight;
        }
        [System.Serializable]
        public class JsonClip
        {
            public string name;
            public JsonBinding[] values;
        }
        // for JsonUtility array limitation
        [System.Serializable]
        public class JsonHelp
        {
            public JsonClip[] list;
        }
        
        // runs once on pressing play
        void Start()
        {
            var proxy = GetComponent<VRMBlendShapeProxy>();
            var mesh = GetComponentInChildren<SkinnedMeshRenderer>().sharedMesh;
            var avatar = proxy.BlendShapeAvatar;

            //var successes = 0;
            // get each clip
            var jsonHelp = new JsonHelp();
            jsonHelp.list = new JsonClip[blendShapeNames.Length];
            for (int i = 0; i < blendShapeNames.Length; i++)
            {
                var name = blendShapeNames[i];
                var clip = avatar.GetClip(BlendShapeKey.CreateUnknown(name));
                var values = clip.Values;

                var jsonClip = new JsonClip();
                jsonClip.name = name;
                jsonClip.values = new JsonBinding[values.Length];

                // assign the combination of BlendShapes to the clip (usually only one)
                for (int j = 0; j < values.Length; j++)
                {
                    var index = values[j].Index;
                    var weight = values[j].Weight;
                    var blendshape = mesh.GetBlendShapeName(index);
                    jsonClip.values[j] = new JsonBinding
                    {
                        blendshape = blendshape,
                        weight = weight
                    };
                }

                jsonHelp.list[i] = jsonClip;
            }

            // write file and re-import
            File.WriteAllText(settingsFile, JsonUtility.ToJson(jsonHelp, prettyPrint));
            AssetDatabase.ImportAsset(settingsFile);
        }
        
        private readonly string[] blendShapeNames = {
            "BrowDownLeft", "BrowDownRight", "BrowInnerUp", "BrowOuterUpLeft", "BrowOuterUpRight",
            "CheekPuff", "CheekSquintLeft", "CheekSquintRight", "EyeBlinkLeft", "EyeBlinkRight",
            "EyeLookDownLeft", "EyeLookDownRight", "EyeLookInLeft", "EyeLookInRight", "EyeLookOutLeft",
            "EyeLookOutRight", "EyeLookUpLeft", "EyeLookUpRight", "EyeSquintLeft", "EyeSquintRight",
            "EyeWideLeft", "EyeWideRight", "JawForward", "JawLeft", "JawOpen", "JawRight", "MouthClose",
            "MouthDimpleLeft", "MouthDimpleRight", "MouthFrownLeft", "MouthFrownRight", "MouthFunnel",
            "MouthLeft", "MouthLowerDownLeft", "MouthLowerDownRight", "MouthPressLeft", "MouthPressRight",
            "MouthPucker", "MouthRight", "MouthRollLower", "MouthRollUpper", "MouthShrugLower",
            "MouthShrugUpper", "MouthSmileLeft", "MouthSmileRight", "MouthStretchLeft", "MouthStretchRight",
            "MouthUpperUpLeft", "MouthUpperUpRight", "NoseSneerLeft", "NoseSneerRight", "TongueOut"
        };  
    }
}