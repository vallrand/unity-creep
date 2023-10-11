# Unity-Creep


* Render Texture (single channel/half float) is used a mask.
  * Alternatively affectors can be passed directly into shader (position + radius). Would require splitting decal into multiple batches to keep number of affectors per decal low. Would also allow culling for decal squares that contain no affectors nearby.
* Custom decal shader is rendering animated texture (along with normals/roughness/etc) onto terrain geometry. Depending on how terrain rendering is setup, could be separate pass like water, or built in into terrain layers rendering.