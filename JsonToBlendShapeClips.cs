using System.IO;
using UnityEngine;
using VRM;

namespace Assets
{
    public class JsonToBlendShapeClips : MonoBehaviour
    {
        public string settingsFile = "Assets/52script_default_settings.json";
        public string blendShapeClipsFolder;

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
            public static JsonHelp FromJson(string settingsfile)
            {
                var json = File.ReadAllText(settingsfile);
                return JsonUtility.FromJson<JsonHelp>(json);
            }
        }
        
        // runs once on pressing play
        void Start()
        {
            var proxy = GetComponent<VRMBlendShapeProxy>();
            var mesh = GetComponentInChildren<SkinnedMeshRenderer>().sharedMesh;
            var avatar = proxy.BlendShapeAvatar;
            var jsonHelp = JsonHelp.FromJson(settingsFile);
            var clips = jsonHelp.list;

            var successes = 0;
            // create and assign new BlendShapeClips
            for (int i = 0; i < clips.Length; i++)
            {
                var name = clips[i].name;
                var values = clips[i].values;
                
                var newClip = BlendShapeAvatar.CreateBlendShapeClip(
                    Path.Combine(blendShapeClipsFolder, name, ".asset"));
                newClip.Values = new BlendShapeBinding[values.Length];
                
                // assign the combination of BlendShapes to the clip (usually only one)
                for (int j = 0; j < values.Length; j++)
                {
                    var blendshape = values[j].blendshape;
                    var weight = values[j].weight;

                    var index = GetIndexForBinding(mesh, blendshape);
                    if (index >= 0)
                    {
                        newClip.Values[j] = CreateBinding("Face", index, weight);
                    }
                }
                
                if (newClip.Values.Length > 0)
                {
                    var key = BlendShapeKey.CreateUnknown(name);
                    avatar.SetClip(key, newClip);
                    successes += 1;
                }
            }

            Debug.LogFormat("Created {0} of {1} clips", successes, clips.Length);
        }
        
        // can only use index with vrm
        public static BlendShapeBinding CreateBinding(string relativePath, int index, float weight)
        {
            return new BlendShapeBinding
            {
                RelativePath = relativePath,
                Index = index,
                Weight = weight
            };
        }

        public static int GetIndexForBinding(Mesh mesh, string blendshapeName)
        {
            var ind = mesh.GetBlendShapeIndex(blendshapeName);
            if (ind == -1)
            {
                Debug.LogError("Blendshape " + blendshapeName + " not found!");
            }
            return ind;
        }
        
        void Demo(BlendShapeAvatar avatar)
        {
            var filename = "TestClip";
            var testClip = BlendShapeAvatar.CreateBlendShapeClip(Path.Combine(blendShapeClipsFolder, filename));
            var testClip2 = avatar.GetClip(BlendShapePreset.Sorrow);
            testClip.Values = testClip2.Values.Clone() as BlendShapeBinding[];
            
            avatar.SetClip(BlendShapeKey.CreateUnknown("TestClip"), testClip); // testclip now identical to sorrow

        }
    }
}