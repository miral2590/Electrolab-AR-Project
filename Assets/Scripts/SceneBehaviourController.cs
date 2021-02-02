using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneBehaviourController : MonoBehaviour
{
    public GameObject colorCube;
    public Animator animCube;

    public GameObject[] objects;

    public RawImage ssPreview;

    private Texture2D ss;

    public GameObject logo;

    // Start is called before the first frame update
    void Start()
    {
        foreach (GameObject go in objects)
        {
            go.SetActive(false);
        }
        ssPreview.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void ChangeColor(string _color)
    {
        Color col;
        ColorUtility.TryParseHtmlString(_color, out col);
        colorCube.GetComponent<Renderer>().material.color = col;
    }

    public void StartAnim()
    {
        animCube.Play("RotationAnimation");

        gameObject.GetComponent<AudioSource>().Play();

    }

    public void ChangeObjects(int _index)
    {
        foreach(GameObject go in objects)
        {
            go.SetActive(false);
        }

        objects[_index].SetActive(true);
    }

    public void TakeSnapshot()
    {
        GameObject[] gos = ObjectPlacementController.Instance.uiElements;

        foreach(GameObject go in gos)
        {
            go.SetActive(false);
        }

        logo.SetActive(true);
        StartCoroutine(TakeSS());
    }

    IEnumerator TakeSS()
    {
        yield return new WaitForEndOfFrame();

        ss = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);

        ss.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);

        ss.Apply();

        ssPreview.gameObject.SetActive(true);
        ssPreview.texture = ss;

        //Destroy(ss);
    }

    public void CancelSanpshot()
    {
        GameObject[] gos = ObjectPlacementController.Instance.uiElements;

        foreach (GameObject go in gos)
        {
            go.SetActive(true);
        }
        ssPreview.gameObject.SetActive(false);

        logo.SetActive(false);

        Destroy(ss);
    }

    public void ShareSnapshot()
    {
        StartCoroutine(ShareSS());
    }

    IEnumerator ShareSS()
    {
        yield return new WaitForEndOfFrame();

        string timeStamp = System.DateTime.Now.ToString("dd-mm-yyyy_HH:mm:ss");
        string name = $"AR-App-SS {timeStamp}";
        new NativeShare().AddFile(ss, name).SetTitle("Share your AR experience").SetSubject("This is a nice AR App").SetText("This is a nice AR App").Share();

        CancelSanpshot();
    }
}

