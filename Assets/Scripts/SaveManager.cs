using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SaveManager : MonoBehaviour {

    public GameObject saveMenu;
    public GameObject confirmMenu;
    public Transform SaveList;
    public GameObject savePrefab;
    private int saveCounter=0;
    public InputField filenameInput;
    private bool isSaving;
    public Dictionary<string, int> saves;
    private void Start()
    {
        RefreshSaves();
    }
    private void RefreshSaves()
    {
        saves= new Dictionary<string, int>();
        saveCounter = 0;
        while (PlayerPrefs.HasKey(saveCounter.ToString()))
        {
            string name = PlayerPrefs.GetString(saveCounter.ToString());
            saves.Add(name.Split('%')[0], saveCounter);
            saveCounter++;
        }
    }
    public void OnSaveMenuClick()
    {
        saveMenu.SetActive(true);
        RefreshSaveList();
    }
    public void OnSaveClick()
    {
        saveMenu.SetActive(false);
        confirmMenu.SetActive(true);
        
        isSaving = true;
        
    }
    public void OnLoadClick()
    {
        saveMenu.SetActive(false);
        confirmMenu.SetActive(true);
       
        isSaving = false;
        
    }
    public void OnCancelClick()
    {
        saveMenu.SetActive(false);
        
    }
    public void OnConfirmOk()
    {

        if (isSaving)
            Save();
        else
            Load();
        confirmMenu.SetActive(false);
    }

    public void OnConfirmCancel()
    {
        confirmMenu.SetActive(false);
    }
    public void OnDelete()
    {
        string filename = filenameInput.text;
        int k;
        saves.TryGetValue(filename, out k);
        if(!saves.ContainsValue(k))
        {
            return;
        }
        PlayerPrefs.DeleteKey(k.ToString());
        saveCounter--;
        while (PlayerPrefs.HasKey((k + 1).ToString()))
        {
            string data = PlayerPrefs.GetString((k + 1).ToString());
            PlayerPrefs.SetString(k.ToString(), data);
            PlayerPrefs.DeleteKey((k+1).ToString());
            k++;
            
        }
        RefreshSaves();
        saveMenu.SetActive(false);
    }
    private void Save()
    {

        string filename = filenameInput.text;
        bool isUsed = saves.ContainsKey(filename);

        
        if(string.IsNullOrEmpty(filename))
        {
            filename = saveCounter.ToString();
        }
        string saveData = filename + '%';
        Block[,,] b = GameManager.Instance.blocks;

        for (int i = 0; i < 20; i++)
        {
            for (int j = 0; j < 20; j++)
            {
                for (int k = 0; k < 20; k++)

                {
                    Block currentBlock = b[i, j, k];
                    if (currentBlock == null)
                        continue;
                    saveData += i.ToString() + "|" +
                               j.ToString() + "|" +
                               k.ToString() + "|" +
                               ((int)currentBlock.color).ToString() + "%";
                }
            }
        }
        if(isUsed)
        {
            int k;
            saves.TryGetValue(filename, out k);
            PlayerPrefs.SetString(saveCounter.ToString(), saveData);
        }
        else
        {
            
            saves.Add(filename, saveCounter);
            PlayerPrefs.SetString(saveCounter.ToString(), saveData);
            saveCounter++;
        }
        
       
    }
    private void Load()
    {
        string filename = filenameInput.text;
        int k;
        saves.TryGetValue(filename, out k);
        if(!saves.ContainsValue(k))
        {
            return;
        }
        string save=PlayerPrefs.GetString(k.ToString());
        string[] blockData = save.Split('%');
        GameManager.Instance.RefreshGrid();
        for(int i=1;i<blockData.Length-1;i++)
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
            GameManager.Instance.CreateBlock(x, y, z, b);
        }
            
    }
    private void RefreshSaveList()
    {
        foreach(Transform t in SaveList)
        {
            Destroy(t.gameObject); 
        }
        for(int i=0;i<saveCounter;i++)
        {
            GameObject go = Instantiate(savePrefab) as GameObject;
            go.transform.SetParent(SaveList);
            string[] saveData = PlayerPrefs.GetString(i.ToString()).Split('%');

            go.GetComponentInChildren<Text>().text = saveData[0];
            string s = saveData[0];
            go.GetComponent<Button>().onClick.AddListener(() => OnSaveClick(s));

        }
    }
    private void OnSaveClick(string name)
    {
        filenameInput.text = name;
    }
}
