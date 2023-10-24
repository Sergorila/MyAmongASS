using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WireTask : MonoBehaviour
{
    public List<Color> wireColors = new List<Color>();

    public List<Wire> leftWires = new List<Wire>();

    public List<Wire> rightWires = new List<Wire>();

    public Wire CurrentDraggedWire;

    public Wire CurrentHoveredWire;

    public bool IsCompleteTask = false;

    private List<Color> _availableColors = new List<Color>();

    private List<int> _availableLeftIndex;

    private List<int> _availableRightIndex;

    private void Start()
    {
        if (!IsCompleteTask)
        {
            _availableColors = new List<Color>(wireColors);
            _availableLeftIndex = new List<int>();
            _availableRightIndex = new List<int>();

            for (int i = 0; i < leftWires.Count; i++)
            {
                _availableLeftIndex.Add(i);
            }

            for (int i = 0; i < rightWires.Count; i++)
            {
                _availableRightIndex.Add(i);
            }

            while (_availableColors.Count > 0 &&
                _availableLeftIndex.Count > 0 &&
                _availableRightIndex.Count > 0)
            {
                Color pickedColor = _availableColors[Random.Range(0, _availableColors.Count)];
                int pickedLeftIndex = Random.Range(0, _availableLeftIndex.Count);
                int pickedRightIndex = Random.Range(0, _availableRightIndex.Count);

                leftWires[_availableLeftIndex[pickedLeftIndex]].SetColor(pickedColor);
                rightWires[_availableRightIndex[pickedRightIndex]].SetColor(pickedColor);

                _availableColors.Remove(pickedColor);
                _availableLeftIndex.RemoveAt(pickedLeftIndex);
                _availableRightIndex.RemoveAt(pickedRightIndex);
            }

            StartCoroutine(CheckIsCompleteTask());
        }
    }

    private IEnumerator CheckIsCompleteTask()
    {
        while (!IsCompleteTask)
        {
            int successfulWires = 0;
            for (int i = 0; i < rightWires.Count; i++)
            {
                if (rightWires[i].IsSuccess)
                {
                    successfulWires++;
                }
            }

            if (successfulWires == rightWires.Count)
            {
                Debug.Log("YEEEES!!!");
                IsCompleteTask = true;
            }

            yield return new WaitForSeconds(1f);
        }
    }
}
