# 52 ARKit `BlendShapeClip` Creation Script

These scripts are for use in the Unity Editor to generate the 52 ARKit/PerfectSync `BlendShapeClips` needed for enhanced face tracking on VRM models.

`JsonToBlendShapeClips.cs` uses a JSON file to create the `BlendShapeClips` with preset values. `52script_default_settings.json` uses values from [Hinzka's example models](https://github.com/hinzka/52blendshapes-for-VRoid-face), and so is designed for VRoid models specifically. This script does not create the `BlendShapes` on the Face mesh, which are referenced by the clips. (To get those on your model, I recommend a look at [this repo](https://github.com/XOrdinary99/unity-vroid-scripts)).

`BlendShapeClipsToJson.cs` creates a JSON file from the `BlendShapeClips` on the `BlendShapeAvatar`, formatted for re-use with `JsonToBlendShapeClips.cs`. This is mainly useful if you manually tweak some clips and want the same changes in an updated model.

These scripts do not create/export any of the preset clips.