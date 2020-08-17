using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public Material[] materialsTop, materialsBottom; // 2 entries each
    bool isLighted;

    public void Initialize(Vector3 position_, Quaternion rotation_, Material[] materialsTop_, Material[] materialsBottom_, bool isLighted_ = false)
    {
        transform.position = position_;
        transform.rotation = rotation_;
        materialsTop = materialsTop_;
        materialsBottom = materialsBottom_;
        isLighted = isLighted_;
        SetMaterials();
    }
        
    public void SetMaterials()
    {
        int lightedIdx = isLighted ? 1 : 0;
        GetComponent<MeshRenderer>().materials = new Material[2] {materialsTop[lightedIdx], materialsBottom[lightedIdx]};
    }

    public void setLight(bool isLighted_)
    {
        isLighted = isLighted_;
        SetMaterials();
    }

    public void switchLight()
    {
        setLight(!isLighted);
    }
}
