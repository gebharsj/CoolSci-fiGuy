using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ButtonScript : MonoBehaviour {

    GameObject gameManager;
    bool delayed;

	// Use this for initialization
	void Start () {
        gameManager = GameObject.Find("GameManager");
	}
	
	public void GenerateMaze()
    {
        if (transform.parent.name != "Canvas")
            CloseMenu(transform.parent.gameObject);
        StartCoroutine(Delay(1f));
        gameManager.GetComponent<Maze>().GenerateMaze();
    }

    public void GenerateMaze(GameObject toggleObject)
    {
        if (transform.parent.name != "Canvas")
            CloseMenu(transform.parent.gameObject);
        StartCoroutine(Delay(1f));
        toggleObject.SetActive(!toggleObject.activeInHierarchy);
        gameManager.GetComponent<Maze>().GenerateMaze();
    }

    public void CloseMenu(GameObject menu)
    {
        menu.SetActive(!menu.activeInHierarchy);
    }

    public void ToggleMenu(GameObject menu)
    {
        gameObject.SetActive(!gameObject.activeInHierarchy);
        menu.SetActive(!menu.activeInHierarchy);
    }

    IEnumerator Delay(float waitTime)
    {
        if(!delayed)
        {
            delayed = true;
            yield return new WaitForSeconds(waitTime);
            delayed = false;
        }
    }
}
