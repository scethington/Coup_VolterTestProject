using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorManager : MonoBehaviour
{
    public static ColorManager instance;

    [SerializeField] private Color _unrevealed;
    [SerializeField] private Color _inactive;
    [SerializeField] private Color _red;
    [SerializeField] private Color _purple;
    [SerializeField] private Color _blue;
    [SerializeField] private Color _green;
    [SerializeField] private Color _black;

    Dictionary<string, Color> colorDict = new Dictionary<string, Color>();

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        colorDict.Add("Unrevealed", _unrevealed);
        colorDict.Add("Inactive", _inactive);
        colorDict.Add("Contessa", _red);
        colorDict.Add("Duke", _purple);
        colorDict.Add("Captain", _blue);
        colorDict.Add("Ambassador", _green);
        colorDict.Add("Assassin", _black);
    }

    public Color GetColor(string name)
    {
        if (!colorDict.ContainsKey(name)) return Color.white;
        return colorDict[name];
    }
}
