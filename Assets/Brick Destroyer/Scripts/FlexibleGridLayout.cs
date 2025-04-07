using UnityEngine;
using UnityEngine.UI;

namespace BrickDestroyer
{
    [RequireComponent(typeof(GridLayoutGroup), typeof(RectTransform))]
    public class FlexibleGridLayout : MonoBehaviour
    {
        private RectTransform rectTransform;
        private GridLayoutGroup gridLayoutGroup;

        // Number of columns in the grid
        [SerializeField] private int columns = 5;
        // Spacing between grid cells
        [SerializeField] private float spacing = 5f;

        void Awake()
        {
            // Cache required components
            rectTransform = GetComponent<RectTransform>();
            gridLayoutGroup = GetComponent<GridLayoutGroup>();

            // Initialize grid layout settings
            UpdateGridLayout();
        }

        /// <summary>
        /// Updates the grid layout by adjusting the cell size and offset.
        /// </summary>
        void UpdateGridLayout()
        {
            // Get the width of the parent RectTransform
            float width = rectTransform.rect.width;

            // Calculate the cell size based on the width and column count
            float cellSize = (width / columns) - spacing;

            // Apply the calculated cell size to the GridLayoutGroup
            gridLayoutGroup.cellSize = new Vector2(cellSize, cellSize);

            // Adjust the RectTransform offset to maintain proper layout positioning
            rectTransform.offsetMin = new Vector2(0, width * -0.5f);
        }
    }
}
