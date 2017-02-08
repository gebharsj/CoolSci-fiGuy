using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SliderScript : MonoBehaviour {

    GameObject gameManager;
    Slider _slider;

	// Use this for initialization
	void Start () {
        gameManager = GameObject.Find("GameManager");
        _slider = GetComponent<Slider>();
    }
	
	public void UpdateValue(string value)
    {
        switch(value)
        {
            case "width":
            case "Width":
            case "xSize":
            case "XSize":
                Maze.xSize = (int)_slider.value;
                break;
            case "height":
            case "Height":
            case "ySize":
            case "YSize":
                Maze.ySize = (int)_slider.value;
                break;
            default:
                Debug.LogError("Entered invalid value!");
                break;
        }
    }

    public void UpdateText(Text _text)
    {
        if (_text)
            _text.text = _slider.value.ToString();
    }
}
