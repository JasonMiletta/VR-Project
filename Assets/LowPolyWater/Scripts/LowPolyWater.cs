using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class LowPolyWater : MonoBehaviour {
    public Material material;
    public Camera _camera;

    void Start() {
        if (material == null) return;
        if(material.GetFloat("_EdgeBlend") > 0.1f) {
            SetupCamera();
        }
    }

    void SetupCamera() {
        if (_camera == null) _camera = Camera.main;
        _camera.depthTextureMode |= DepthTextureMode.Depth;
    }

    #region Editor code
    #if UNITY_EDITOR
    // the code below only compiles inside the editor!

    public int size = 30;
    public float scale = 1;
    [Range(0,1)]
    public float noise = 0;
    public enum GridType {Hexagonal, Square};
    public GridType gridType;

    MeshFilter[] mfs;
    bool generate;
    float radius;
    float prevScale;
    float delta;
    List<Vector3> verts = new List<Vector3>();
    List<int> inds = new List<int>();

    const int maxVerts = ushort.MaxValue;
    const float sin60 = 0.86602540378f;
    const float inv_tan60 = 0.57735026919f;

    void OnEnable() {
        Start();
        mfs = GetComponentsInChildren<MeshFilter>();
        prevScale = scale;
        Scale();
    }

    void OnValidate() {
        size = Mathf.Clamp(size, 1, 256);
        mfs = GetComponentsInChildren<MeshFilter>();
        radius = size;

        generate = GUI.changed || mfs==null || mfs.Length == 0;
        if (!generate) {
            for (int i = 0; i < mfs.Length; i++) {
                if(mfs[i].sharedMesh == null) {
                    generate = true;
                    break;
                }
            }
        }
    }

    void Scale() {
        if (material == null) return;
        material.SetFloat("__Scale", scale);
    }

    void Update() {
        if (material == null) return;

        if (material.GetFloat("_EdgeBlend") > 0.1f) {
            SetupCamera();
        }

        var lScale = transform.localScale;
        if (prevScale != scale) {
            lScale.x = scale;
            lScale.z = scale;
            transform.localScale = lScale;
            prevScale = scale;
        } else if (lScale.x != scale || lScale.z != scale) {
            scale = Mathf.Min(lScale.x, lScale.z);
        }

        Scale();

        if (!generate) return;
        generate = false;
        mfs = GetComponentsInChildren<MeshFilter>();
        if (gridType == GridType.Hexagonal) {
            GenerateHexagonal();
        } else {
            GenerateSquare();
        }
    }

    float Encode(Vector3 v) {
        var uv0 = Mathf.Round((v.x + 5) * 10000f);
        var uv1 = Mathf.Round((v.z + 5) * 10000f) / 100000f;
        return uv0 + uv1;
    }

    void BakeMesh(float rotation = 0f) {
        var uvs = new List<Vector2>(inds.Count);
        var splitIndices = new List<int>(inds.Count);
        var splitVertices = new List<Vector3>(inds.Count);

        for (int i = 0; i < inds.Count; i += 3) {
            splitIndices.Add(i % maxVerts);
            splitIndices.Add((i + 1) % maxVerts);
            splitIndices.Add((i + 2) % maxVerts);

            var v0 = verts[inds[i]];
            var v1 = verts[inds[i + 1]];
            var v2 = verts[inds[i + 2]];

            splitVertices.Add(v0);
            splitVertices.Add(v1);
            splitVertices.Add(v2);

            var uv = new Vector2();
            uv.x = Encode(v0 - v1);
            uv.y = Encode(v0 - v2);
            uvs.Add(uv);

            uv.x = Encode(v1 - v2);
            uv.y = Encode(v1 - v0);
            uvs.Add(uv);

            uv.x = Encode(v2 - v0);
            uv.y = Encode(v2 - v1);
            uvs.Add(uv);
        }

        // clear all filters
        for (int i = 0; i < mfs.Length; i++) {
            DestroyImmediate(mfs[i].gameObject);
        }

        int numGO = Mathf.CeilToInt(splitVertices.Count / (float)maxVerts);
        mfs = new MeshFilter[numGO];
        for (int i = 0, pos = 0; i < numGO; i++, pos += maxVerts) {
            var go = new GameObject("WaterChunk");
            go.transform.parent = transform;
            go.transform.localPosition = Vector3.zero;
            go.transform.localRotation = Quaternion.Euler(0, rotation, 0);
            go.transform.localScale = Vector3.one;
            var mf = go.AddComponent<MeshFilter>();
            var mr = go.AddComponent<MeshRenderer>();
            mr.sharedMaterial = material;
            mr.receiveShadows = false;
            mr.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;

            var mesh = new Mesh();
            mesh.name = "WaterChunk";

            var len = i == numGO - 1 ? splitVertices.Count - pos : maxVerts;

            mesh.SetVertices(splitVertices.GetRange(pos, len));
            mesh.SetTriangles(splitIndices.GetRange(pos, len), 0);
            mesh.SetUVs(0, uvs.GetRange(pos, len));

            mf.mesh = mesh;

            mfs[i] = mf;
        }
    }

    void Add(List<Vector3> verts, Vector3 toAdd) {
        if(noise > 0) {
            var n = UnityEngine.Random.insideUnitCircle* noise *delta / 2f;
            toAdd.x += n.x;
            toAdd.z += n.y;
        }
        verts.Add(toAdd);
    }

    void GenerateSquare() {
        verts.Clear();
        inds.Clear();
        var numVerts = size*2;
        delta = radius*2f*sin60 / numVerts;
        var deltaX = Vector3.right * delta;
        var vO = new Vector3(-radius *sin60, 0, -radius *sin60);

        for (int j = 0; j < numVerts + 1; j++) {
            bool reverse = j % 2 != 0;
            var v = vO + Vector3.forward * j * delta;
            int cols = numVerts + (reverse ? 2 : 1);
            for (int i = 0; i < cols; i++) {
                Add(verts, v);
                if (reverse && (i==0||i== cols-2)) {
                    v += deltaX / 2f;
                } else {
                    v += deltaX;
                }
            }
        }
        int iCur = 0;
        for (int j = 0; j < numVerts; j++) {
            bool reverse = j % 2 != 0;
            int ofs = numVerts + (reverse ? 2 : 1);
            int cols = numVerts + (reverse ? 0 : 0);

            int iForw = iCur + ofs;

            for (int i = 0; i < cols; i++) {
                int iRight = iCur + 1;
                int iForwRight = iForw + 1;

                inds.Add(iCur);
                if (reverse) {
                    inds.Add(iForw);
                    inds.Add(iRight);
                    inds.Add(iForw);
                    inds.Add(iForwRight);
                    inds.Add(iRight);
                } else {
                    inds.Add(iForwRight);
                    inds.Add(iRight);
                    inds.Add(iCur);
                    inds.Add(iForw);
                    inds.Add(iForwRight);
                }
                iCur = iRight;
                iForw = iForwRight;
            }
            inds.Add(iCur);
            if (reverse) {
                inds.Add(iForw );
                inds.Add(iCur + 1);
                iCur+=2;
            } else {
                inds.Add(iForw);
                inds.Add(iForw + 1);
                iCur++;
            }
        }

        BakeMesh(90);
    }

    void GenerateHexagonal() {
        verts.Clear();
        inds.Clear();

        float delta = radius / size;
        int vertIndex = 0;
        int curNumPoints = 0;
        int prevNumPoints = 0;
        int numPointsCol0 = 2 * size + 1;
        int colMin = -size;
        int colMax = size;

        for (int i = colMin; i <= colMax; i++) {
            float x = sin60 * delta * i;

            int numPointsColi = numPointsCol0 - Mathf.Abs(i);

            int rowMin = -size;
            if (i < 0) rowMin += Mathf.Abs(i);

            int rowMax = rowMin + numPointsColi - 1;

            curNumPoints += numPointsColi;

            for (int j = rowMin; j <= rowMax; j++) {
                float z = inv_tan60 * x + delta * j;

                var v = new Vector3(x, 0, z);
                if (noise > 0) {
                    var n = UnityEngine.Random.insideUnitCircle * noise * delta/2f;
                    v.x += n.x;
                    v.z += n.y;
                }
                verts.Add(v);

                if (vertIndex < (curNumPoints - 1)) {
                    if (i >= colMin && i < colMax) {
                        int padLeft = 0;
                        if (i < 0) padLeft = 1;
                        inds.Add(vertIndex);
                        inds.Add(vertIndex + 1);
                        inds.Add(vertIndex + numPointsColi + padLeft);
                    }

                    if (i > colMin && i <= colMax) {
                        int padRight = 0;
                        if (i > 0)  padRight = 1;
                        inds.Add(vertIndex+1);
                        inds.Add(vertIndex);
                        inds.Add(vertIndex - prevNumPoints + padRight);
                    }
                }

                vertIndex++;
            }

            prevNumPoints = numPointsColi;
        }

        BakeMesh();
    }



    #endif
    #endregion
}

