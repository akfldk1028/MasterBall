// using UnityEngine;

// public class IsometricGridGenerator : MonoBehaviour
// {
//     [Header("Grid Settings")]
//     public GameObject cubePrefab;
//     public int gridSizeX = 20;
//     public int gridSizeY = 20;
//     public float cubeSize = 1.0f;
//     public float spacing = 0.05f;
//     public float gridHeight = 0.2f; // 그리드 타일 높이
    
//     [Header("Color Settings")]
//     public Color redColor = Color.red;
//     public Color yellowColor = Color.yellow;
//     public Color greenColor = Color.green;
//     public Color blueColor = Color.blue;

//     void Start()
//     {
//         CreateGrid();
//     }

//     public void CreateGrid()
//     {
//         // 기존 큐브 제거
//         foreach (Transform child in transform)
//         {
//             Destroy(child.gameObject);
//         }
        
//         // 큐브 크기 + 간격
//         float step = cubeSize + spacing;
        
//         // 그리드 중앙이 (0,0,0)에 오도록 시작점 계산
//         float startX = -(gridSizeX * step) / 2f;
//         float startZ = -(gridSizeY * step) / 2f;
        
//         // 큐브 생성
//         for (int x = 0; x < gridSizeX; x++)
//         {
//             for (int y = 0; y < gridSizeY; y++)
//             {
//                 float posX = startX + x * step + cubeSize/2;
//                 float posZ = startZ + y * step + cubeSize/2;
                
//                 // XZ 평면에 그리드 타일 생성
//                 Vector3 position = new Vector3(posX, 0, posZ);
                
//                 GameObject cube;
//                 if (cubePrefab != null)
//                 {
//                     cube = Instantiate(cubePrefab, position, Quaternion.identity);
//                 }
//                 else
//                 {
//                     cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
//                     cube.transform.position = position;
//                 }
                
//                 // 납작한 타일 형태로 설정
//                 cube.transform.localScale = new Vector3(cubeSize, gridHeight, cubeSize);
//                 cube.transform.parent = this.transform;
                
//                 // 색상 설정
//                 Renderer renderer = cube.GetComponent<Renderer>();
//                 if (renderer != null)
//                 {
//                     Color cubeColor = GetCubeColor(x, y);
//                     renderer.material.color = cubeColor;
//                 }
//             }
//         }
//     }
    
//     Color GetCubeColor(int x, int y)
//     {
//         // 그리드를 4분할
//         bool isRightHalf = x >= gridSizeX / 2;
//         bool isTopHalf = y >= gridSizeY / 2;
        
//         if (!isRightHalf && isTopHalf)
//         {
//             return redColor; // 좌상단: 빨간색
//         }
//         else if (isRightHalf && isTopHalf)
//         {
//             return yellowColor; // 우상단: 노란색
//         }
//         else if (!isRightHalf && !isTopHalf)
//         {
//             return greenColor; // 좌하단: 초록색
//         }
//         else
//         {
//             return blueColor; // 우하단: 파란색
//         }
//     }
// }

using UnityEngine;

public class IsometricGridGenerator : MonoBehaviour
{
    [Header("Grid Settings")]
    public GameObject cubePrefab;
    public GameObject wallPrefab; // New wall prefab
    public int gridSizeX = 20;
    public int gridSizeY = 20;
    public float aspectRatio = 1.5f; // Adjust this value to make the grid appear square

    public float cubeSize = 1.0f;
    public float spacing = 0.05f;
    public float gridHeight = 0.2f;
    public float wallHeight = 1.0f; // Height for walls
    
    [Header("Color Settings")]
    public Color redColor = Color.red;
    public Color yellowColor = Color.yellow;
    public Color greenColor = Color.green;
    public Color blueColor = Color.blue;
    public Color wallColor = Color.green; // Wall color
    
    void Start()
    {
        CreateGrid();
    }
    
    public void CreateGrid()
    {
        // Clear existing objects
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
        
        float step = cubeSize + spacing;
        float startX = -(gridSizeX * step) / 2f;
        
        // Adjust the Z dimension by the aspect ratio to make it appear square
        int adjustedGridSizeY = Mathf.RoundToInt(gridSizeY * aspectRatio);
        float startZ = -(adjustedGridSizeY * step) / 2f;
        
        // Create grid tiles
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < adjustedGridSizeY; y++)
            {
                float posX = startX + x * step + cubeSize/2;
                float posZ = startZ + y * step + cubeSize/2;
                
                Vector3 position = new Vector3(posX, 0, posZ);
                
                GameObject cube;
                if (cubePrefab != null)
                {
                    cube = Instantiate(cubePrefab, position, Quaternion.identity);
                }
                else
                {
                    cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    cube.transform.position = position;
                }
                
                cube.transform.localScale = new Vector3(cubeSize, gridHeight, cubeSize);
                cube.transform.parent = this.transform;
                
                Renderer renderer = cube.GetComponent<Renderer>();
                if (renderer != null)
                {
                    // Adjust the color calculation to account for the new dimensions
                    Color cubeColor = GetCubeColor(x, y, adjustedGridSizeY);
                    renderer.material.color = cubeColor;
                }
            }
        }
        
        // Add walls around the grid - update this to use adjustedGridSizeY
        CreateWalls(startX, startZ, step, adjustedGridSizeY);
    }
        
    void CreateWalls(float startX, float startZ, float step, int adjustedGridSizeY)
    {
        // Calculate the full grid dimensions
        float gridWidth = gridSizeX * step;
        float gridDepth = adjustedGridSizeY * step;
        
        // Create the four walls
        // Top wall (Z+)
        CreateWallRow(startX, startZ + gridDepth, gridWidth, true);
        
        // Bottom wall (Z-)
        CreateWallRow(startX, startZ - step, gridWidth, true);
        
        // Left wall (X-)
        CreateWallRow(startX - step, startZ, gridDepth, false);
        
        // Right wall (X+)
        CreateWallRow(startX + gridWidth, startZ, gridDepth, false);
        
        // Add corner pieces
        CreateCornerPiece(startX - step, startZ - step);
        CreateCornerPiece(startX + gridWidth, startZ - step);
        CreateCornerPiece(startX - step, startZ + gridDepth);
        CreateCornerPiece(startX + gridWidth, startZ + gridDepth);
    }
    void CreateCornerPiece(float x, float z)
    {
        Vector3 position = new Vector3(x + cubeSize/2, wallHeight/2, z + cubeSize/2);
        
        GameObject corner;
        if (wallPrefab != null)
        {
            corner = Instantiate(wallPrefab, position, Quaternion.identity);
        }
        else
        {
            corner = GameObject.CreatePrimitive(PrimitiveType.Cube);
            corner.transform.position = position;
        }
        
        corner.transform.localScale = new Vector3(cubeSize, wallHeight, cubeSize);
        corner.transform.parent = this.transform;
        
        // Set wall color
        Renderer renderer = corner.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material.color = wallColor;
        }
    }
    void CreateWallRow(float startX, float startZ, float length, bool isHorizontal)
    {
        float step = cubeSize + spacing;
        int segments = Mathf.CeilToInt(length / step);
        
        for (int i = 0; i < segments; i++)
        {
            float posX = isHorizontal ? startX + i * step + cubeSize/2 : startX + cubeSize/2;
            float posZ = isHorizontal ? startZ + cubeSize/2 : startZ + i * step + cubeSize/2;
            
            Vector3 position = new Vector3(posX, wallHeight/2, posZ);
            
            GameObject wall;
            if (wallPrefab != null)
            {
                wall = Instantiate(wallPrefab, position, Quaternion.identity);
            }
            else
            {
                wall = GameObject.CreatePrimitive(PrimitiveType.Cube);
                wall.transform.position = position;
            }
            
            // Set wall dimensions
            if (isHorizontal)
            {
                wall.transform.localScale = new Vector3(cubeSize, wallHeight, cubeSize);
            }
            else
            {
                wall.transform.localScale = new Vector3(cubeSize, wallHeight, cubeSize);
            }
            
            wall.transform.parent = this.transform;
            
            // Set wall color
            Renderer renderer = wall.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material.color = wallColor;
            }
        }
    }
    Color GetCubeColor(int x, int y, int adjustedGridSizeY)
    {
        // Grid quadrants based on adjusted dimensions
        bool isRightHalf = x >= gridSizeX / 2;
        bool isTopHalf = y >= adjustedGridSizeY / 2;
        
        if (!isRightHalf && isTopHalf)
        {
            return redColor; // 좌상단: 빨간색
        }
        else if (isRightHalf && isTopHalf)
        {
            return yellowColor; // 우상단: 노란색
        }
        else if (!isRightHalf && !isTopHalf)
        {
            return greenColor; // 좌하단: 초록색
        }
        else
        {
            return blueColor; // 우하단: 파란색
        }
    }
}