using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuScene : MonoBehaviour {
    public GameObject previewContainer;
    public Text previewName;
    public GameObject blockPrefab;
    public Material[] mats;
    private int saveCounter;
    private int previewIndex;
    private Vector3 startClick;
    private void Start()
    {
        
        saveCounter = 0;
        previewIndex = 0;
        while (PlayerPrefs.HasKey(saveCounter.ToString()))
        {
           
           
            saveCounter++;
        }
        BuildPreview(previewIndex);
    }
    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            startClick = Input.mousePosition;
        }
        if(Input.GetMouseButtonUp(0))
        {
            Vector3 delta = Input.mousePosition - startClick;
            if(Mathf.Abs(delta.x)>2.5f)
            {
                if (delta.x < 0)
                    swipeleft(true);
                else
                    swipeleft(false);
            }
        }
        RotatePreview();
    }
    private void swipeleft(bool left)
    {
        if(left)
        {
            previewIndex -= 1;
            if(previewIndex<0)
            {
                previewIndex = saveCounter-1;
            }
        }
        else
        {
            previewIndex += 1;
            if(previewIndex>saveCounter-1)
            {
                previewIndex = 0;
            }
        }
        BuildPreview(previewIndex);
    }
    private void RotatePreview()
    {
        previewContainer.transform.RotateAround(new Vector3(5,0,5),Vector3.up,35*Time.deltaTime);
    }
    private void BuildPreview(int key)
    {
        if(!PlayerPrefs.HasKey(key.ToString()))
        {
            return;
        }
        foreach(Transform t in previewContainer.transform)
        {
            Destroy(t.gameObject);
        }
        string Data = PlayerPrefs.GetString(key.ToString());
        string[] blockData = Data.Split('%');
        previewName.text = blockData[0];
        
        for (int i = 1; i < blockData.Length - 1; i++)
        {
            string[] currentBlock = blockData[i].Split('|');
            int x = int.Parse(currentBlock[0]);
            int y = int.Parse(currentBlock[1]);
            int z = int.Parse(currentBlock[2]);
            int c = int.Parse(currentBlock[3]);
            Block b = new Block()
            {
                color = (BlockColor)c
            };
            GameObject go = Instantiate(blockPrefab);
            go.transform.SetParent(previewContainer.transform);
           
            go.transform.position = new Vector3(x, y, z);
            go.GetComponent<Renderer>().material = mats[(int)c];
        }
    }
    public void OnPlayClick()
    {
        SceneManager.LoadScene("main");
    }
}
