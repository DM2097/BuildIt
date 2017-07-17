using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class Block
{
    public Transform Blocktransform;
    public BlockColor color;
}
public enum BlockColor
{
Brick=0,
Cement=1, 
Grass=2
}
public struct BlockAction
{
    public bool delete;
    public Vector3 index;
    public BlockColor color;
}
public class GameManager : MonoBehaviour {
    public static GameManager Instance { set; get; }
    public float blockSize = 0.25f;
    private EventSystem es;
    public Block[,,] blocks = new Block[20, 20, 20];
    public Button deletebutton;
    public Sprite[] deleteButtons;

    private GameObject baseobject;
    public GameObject blockprefab;
    public BlockColor selectedColor;
    public Material[] blockMaterials;
    private Vector3 offset;
    private Vector3 basecenter = new Vector3(1.25f, 0, 1.25f);
    private bool isdeleting;
    private BlockAction previewAction;
	// Use this for initialization
	void Start () {
        Instance = this;
        baseobject = GameObject.Find("Base");
         offset=  (Vector3.one*0.5f)/4;
        selectedColor = BlockColor.Grass;
        es = FindObjectOfType<EventSystem>();
    }
    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            if(es.IsPointerOverGameObject())
            {

            }
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 30.0f))
            {
                
              
                if (isdeleting )
                {
                    if(hit.transform.name != "Base")
                    {
                        Vector3 oldCubeIndex = BlockPosition(hit.point - hit.normal * (blockSize - 0.01f));
                        BlockColor previousColor = blocks[(int)oldCubeIndex.x, (int)oldCubeIndex.y, (int)oldCubeIndex.z].color;
                        //index -= hit.normal * blockSize;
                        Destroy(blocks[(int)oldCubeIndex.x, (int)oldCubeIndex.y, (int)oldCubeIndex.z].Blocktransform.gameObject);
                        blocks[(int)oldCubeIndex.x, (int)oldCubeIndex.y, (int)oldCubeIndex.z] = null;
                        previewAction = new BlockAction()
                        {
                            delete = true,
                            index = oldCubeIndex,
                            color = previousColor
                        
                        };
                    }
                    
                    return;
                }
                Vector3 index = BlockPosition(hit.point);
                int x = (int)index.x;
                int y = (int)index.y;
                int z = (int)index.z;
                if (blocks[x, y, z] == null)
                {
                    GameObject go = CreateBlock();
                    
                    PositionBlock(go.transform, index);

                    blocks[x, y, z] = new Block
                    {
                        Blocktransform = go.transform,
                        color = selectedColor
                    };
                    previewAction = new BlockAction()
                    {
                        delete = false,
                        index = new Vector3(x, y, z),
                        color = selectedColor

                    };

                }
                else
                {
                    if(isdeleting)
                    {

                    }
                    GameObject go = CreateBlock();
                   
                   
                    Vector3 newIndex = BlockPosition(hit.point + (hit.normal*blockSize));
                    blocks[(int)newIndex.x, (int)newIndex.y, (int)newIndex.z] = new Block
                    {
                        Blocktransform = go.transform,
                        color = selectedColor
                    };
                    PositionBlock(go.transform, newIndex);
                    previewAction = new BlockAction()
                    {
                        delete = false,
                        index = newIndex,
                        color = selectedColor,

            };
                }
               
            
                }

            }
        }
    private GameObject CreateBlock()
    {
       GameObject go= Instantiate(blockprefab) as GameObject;
        go.GetComponent<Renderer>().material = blockMaterials[(int)selectedColor];
        go.transform.localScale = Vector3.one * blockSize;
        return go;
    }
    public GameObject CreateBlock(int x,int y,int z,Block b)
    
    {
        GameObject go = Instantiate(blockprefab) as GameObject;
        go.GetComponent<Renderer>().material = blockMaterials[(int)b.color];
        go.transform.localScale = Vector3.one * blockSize;
        b.Blocktransform = go.transform;
        blocks[x,y,z] = b;
        PositionBlock(b.Blocktransform, new Vector3(x, y, z));
        return go;
    }
    private Vector3 BlockPosition(Vector3 hit)
    {
        int x = (int)(hit.x/blockSize);
        int y = (int)(hit.y / blockSize);
        int z = (int)(hit.z / blockSize);
       
        return new Vector3(x, y, z);
    }
    public void PositionBlock(Transform t,Vector3 index)
    {
        t.position = ((index*blockSize) + offset) + (baseobject.transform.position - basecenter);
    }

    public void changeBlockmaterial(int color)
    {
        selectedColor = (BlockColor)color;
       
    }
    public void ToggleDelete()
    {
        isdeleting = !isdeleting;
        deletebutton.image.sprite = (isdeleting) ? deleteButtons[0] : deleteButtons[1];
    }
    public void RefreshGrid()
    {
        for(int i=0;i<20;i++)
            for (int j = 0; j < 20; j++)
                for (int k = 0; k < 20; k++)
                {
                    if (blocks[i, j, k] == null)
                        continue;
                    Destroy(blocks[i, j, k].Blocktransform.gameObject);
                    blocks[i, j, k] = null;
                }

                    blocks = new Block[20, 20, 20];
    }
    public void undo()
    {
        if(previewAction.delete)
        {
            GameObject go = CreateBlock();


            
            blocks[(int)previewAction.index.x, (int)previewAction.index.y, (int)previewAction.index.z] = new Block
            {
                Blocktransform = go.transform,
                color = selectedColor
            };
            PositionBlock(go.transform, previewAction.index);
            previewAction = new BlockAction()
            {
                delete = false,
                index = previewAction.index,
                color = previewAction.color

            };

        }
        else
        {


           
                
                //index -= hit.normal * blockSize;
                Destroy(blocks[(int)previewAction.index.x, (int)previewAction.index.y, (int)previewAction.index.z].Blocktransform.gameObject);
                blocks[(int)previewAction.index.x, (int)previewAction.index.y, (int)previewAction.index.z] = null;
                previewAction = new BlockAction()
                {
                    delete = true,
                    index = previewAction.index,
                    color = previewAction.color

                };
            
        }
    }
     
        
}
