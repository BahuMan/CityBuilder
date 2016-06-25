using UnityEngine;
using System.Collections;

public class ScraperMaker : MonoBehaviour {

    public Material buildingMaterial;
    public Material roofMaterial;

    public enum MyGenerators { Perlin, NormalDistribution };

    public MyGenerators GenerationFunction = MyGenerators.NormalDistribution;

    public int[] PerlinCharacteristic = new int[] { 8, 4, 2, 1 };
    public int NormalSize = 20;
    public float NormalSigma = 20f;
    public float NormalScale = 20f;

    private float[,] heightMap;



	void Start () {

        if (GenerationFunction == MyGenerators.Perlin)
        {
            heightMap = initPerlinMap(PerlinCharacteristic);
        }
        else
        {
            heightMap = initNormalDistribution(NormalSize);
        }

        for (int x=0; x< heightMap.GetLength(0); ++x)
        {
            for (int y=0; y< heightMap.GetLength(1); ++y)
            {
                GameObject SkyScraper = new GameObject("testje" + x + "," + y);
                SkyScraper.transform.position = new Vector3(x*2, 0f, y*2);
                constructCube(SkyScraper.AddComponent<MeshFilter>(), Random.Range(1f, heightMap[x,y]));
                MeshRenderer scraperRender = SkyScraper.AddComponent<MeshRenderer>();
                scraperRender.material = this.buildingMaterial;
            }
        }
    }

    private float[,] initNormalDistribution(int size)
    {
        float[,] m = new float[size, size];
        float mid = size * .5f;
        for (int i = 0; i < m.GetLength(0); ++i)
        {
            for (int j = 0; j < m.GetLength(1); ++j)
            {
                m[i, j] = NormalScale * (Mathf.Exp(-(j - mid) * (j - mid) / this.NormalSigma) * Mathf.Exp(-(i - mid) * (i - mid) / this.NormalSigma));
            }
        }

        return m;
    }

    private float[,] initPerlinMap(int[] characteristics)
    {
        //size of the map is the square of the number of iterations in Perlin noise:
        float[,] m = new float[characteristics.Length * characteristics.Length, characteristics.Length * characteristics.Length];
        recursivePerlin(0, m, 0, 0,                                           Random.value * characteristics[0], characteristics);
        recursivePerlin(0, m, characteristics.Length, 0,                      Random.value * characteristics[0], characteristics);
        recursivePerlin(0, m, 0, characteristics.Length,                      Random.value * characteristics[0], characteristics);
        recursivePerlin(0, m, characteristics.Length, characteristics.Length, Random.value * characteristics[0], characteristics);
        return m;
    }

    private void recursivePerlin(int iteration, float[,] m, int mx, int my, float PerlinValue, int[] characteristics)
    {
        if (iteration == characteristics.Length)
        {
            if (mx >= m.GetLength(0)) return;
            if (my >= m.GetLength(1)) return;
            m[mx, my] = PerlinValue;
            return;
        }

        for (int px = 0; px < m.Length; px += characteristics.Length - iteration)
        {
            for (int py = 0; py < m.Length; py += characteristics.Length - iteration)
            {
                PerlinValue += Random.value * characteristics[iteration];
                recursivePerlin(iteration+1, m, px, py, PerlinValue, characteristics);
            }
        }

    }

    // Update is called once per frame
    void Update () {
    }

    Mesh constructCube(MeshFilter filter, float height)
    {
        // You can change that line to provide another MeshFilter
        Mesh mesh = filter.mesh;
        mesh.Clear();

        float length = 1f;
        float depth = 1f;

        #region Vertices
        Vector3 p0 = new Vector3(-length * .5f, 0f, depth * .5f);
        Vector3 p1 = new Vector3(length * .5f, 0f, depth * .5f);
        Vector3 p2 = new Vector3(length * .5f, 0f, -depth * .5f);
        Vector3 p3 = new Vector3(-length * .5f, 0f, -depth * .5f);

        Vector3 p4 = new Vector3(-length * .5f, height, depth * .5f);
        Vector3 p5 = new Vector3(length * .5f, height, depth * .5f);
        Vector3 p6 = new Vector3(length * .5f, height, -depth * .5f);
        Vector3 p7 = new Vector3(-length * .5f, height, -depth * .5f);

        Vector3[] vertices = new Vector3[]
        {
	// Bottom
	p0, p1, p2, p3,
 
	// Left
	p7, p4, p0, p3,
 
	// Front
	p4, p5, p1, p0,
 
	// Back
	p6, p7, p3, p2,
 
	// Right
	p5, p6, p2, p1,
 
	// Top
	p7, p6, p5, p4
        };
        #endregion

        #region Normales
        Vector3 up = Vector3.up;
        Vector3 down = Vector3.down;
        Vector3 front = Vector3.forward;
        Vector3 back = Vector3.back;
        Vector3 left = Vector3.left;
        Vector3 right = Vector3.right;

        Vector3[] normales = new Vector3[]
        {
	// Bottom
	down, down, down, down,
 
	// Left
	left, left, left, left,
 
	// Front
	front, front, front, front,
 
	// Back
	back, back, back, back,
 
	// Right
	right, right, right, right,
 
	// Top
	up, up, up, up
        };
        #endregion

        #region UVs
        Vector2 _00 = new Vector2(0f, 0f);
        Vector2 _10 = new Vector2(1f, 0f);
        Vector2 _01 = new Vector2(0f, height);
        Vector2 _11 = new Vector2(1f, height);

        Vector2[] uvs = new Vector2[]
        {
	// Bottom
	_11, _01, _00, _10,
 
	// Left
	_11 , _01, _00, _10,
 
	// Front
	_11, _01, _00, _10,
 
	// Back
	_11, _01, _00, _10,
 
	// Right
	_11, _01, _00, _10,
 
	// Top
	_11, _01, _00, _10,
        };
        #endregion

        #region Triangles
        int[] triangles = new int[]
        {
	// Bottom
	3, 1, 0,
    3, 2, 1,			
 
	// Left
	3 + 4 * 1, 1 + 4 * 1, 0 + 4 * 1,
    3 + 4 * 1, 2 + 4 * 1, 1 + 4 * 1,
 
	// Front
	3 + 4 * 2, 1 + 4 * 2, 0 + 4 * 2,
    3 + 4 * 2, 2 + 4 * 2, 1 + 4 * 2,
 
	// Back
	3 + 4 * 3, 1 + 4 * 3, 0 + 4 * 3,
    3 + 4 * 3, 2 + 4 * 3, 1 + 4 * 3,
 
	// Right
	3 + 4 * 4, 1 + 4 * 4, 0 + 4 * 4,
    3 + 4 * 4, 2 + 4 * 4, 1 + 4 * 4,
 
	// Top
	3 + 4 * 5, 1 + 4 * 5, 0 + 4 * 5,
    3 + 4 * 5, 2 + 4 * 5, 1 + 4 * 5,

        };
        #endregion

        mesh.vertices = vertices;
        mesh.normals = normales;
        mesh.uv = uvs;
        mesh.triangles = triangles;

        mesh.RecalculateBounds();
        mesh.Optimize();

        return mesh;
    }
}
