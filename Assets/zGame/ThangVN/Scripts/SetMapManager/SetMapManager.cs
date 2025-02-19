using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class SetMapManager : MonoBehaviour
{
    [SerializeField] GameMode gameMode;
    [SerializeField] public ColorPlateData colorPlateData;
    public List<ColorPlate> listPlates;
    public int level;
    public int rows;
    public int cols;
    public float cellSize;
    private ColorPlate[,] colorPlate;
    [SerializeField] ColorPlate colorPlatePrefab;
    [SerializeField] List<ColorPlate> ListColorPlate;
    [SerializeField] Transform holderColorPlate;
    [SerializeField] private int goals;
    [SerializeField] private int gold;
    [SerializeField] private int pigment;

    public enum EditMode
    {
        EditLockCoin,
        EditFrozenSquare,
        EditCannotPlace,
        SelectExistedPlate,
        SelectPlateArrowLeft,
        SelectPlateArrowRight,
        SelectPlateArrowUp,
        SelectPlateArrowDown,
        ClearPlate,
        DeletePlate,
        EditAds,
        EditWood,
        EditPoison,
        EditBags
    }

    public EditMode editMode;
    public LayerMask layerMask;

    private void Start()
    {
        cellSize = 0.75f;
        if (gameMode == GameMode.Edit)
        {
            LoadDataSetMap(level);
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out var hit, 100f, layerMask))
            {
                ColorPlate c = hit.collider.GetComponent<ColorPlate>();
                SelectPlateNormal(c);
                SelectPlateSpecial(c);
                SelectExistedPlate(c);
                ClearPlate(c);
                DeletePlate(c);
            }
        }
    }

    #region SaveLoadData

    public void SaveData()
    {
        colorPlateData.goalScore = goals;
        colorPlateData.gold = gold;
        colorPlateData.pigment = pigment;
        string data = JsonUtility.ToJson(colorPlateData);
        File.WriteAllText($"Assets/Resources/LevelData/Level_{level}.json", data);
    }

    private void LoadDataSetMap(int level)
    {
        string filePath = $"LevelData/Level_{level}";
        TextAsset textAsset = Resources.Load<TextAsset>(filePath);

        if (textAsset != null)
        {
            LoadExistedData(textAsset);
        }
        else
        {
            Debug.Log("empty data");
            Init(rows, cols, holderColorPlate, ListColorPlate, colorPlatePrefab);
            colorPlateData.cols = cols;
            colorPlateData.rows = rows;
        }
    }

    private void LoadExistedData(TextAsset textAsset)
    {
        Debug.Log(textAsset.ToString());
        colorPlateData = JsonUtility.FromJson<ColorPlateData>(textAsset.ToString());


        rows = colorPlateData.rows;
        cols = colorPlateData.cols;
        Init(rows, cols, holderColorPlate, ListColorPlate, colorPlatePrefab);


        for (int i = 0; i < colorPlateData.listSpecialData.Count; i++)
        {
            int index = colorPlateData.listSpecialData[i].Row * cols + colorPlateData.listSpecialData[i].Col;

            ListColorPlate[index].status = (Status)colorPlateData.listSpecialData[i].type;
            ListColorPlate[index].logicVisual
                .SetSpecialSquare(ListColorPlate[index].status, colorPlateData.listSpecialData[i].Row);

            //if (ListColorPlate[index].status == Status.Frozen)
            //{
            //    ListColorPlate[index].countFrozen = 3;
            //}

            if (ListColorPlate[index].status == Status.LockCoin)
            {
                ListColorPlate[index].txtPointUnlock.gameObject.SetActive(true);
                ListColorPlate[index].pointToUnLock = colorPlateData.listSpecialData[i].pointUnlock;
                ListColorPlate[index].txtPointUnlock.text = ListColorPlate[index].pointToUnLock.ToString();
            }
        }

        for (int i = 0; i < colorPlateData.listArrowData.Count; i++)
        {
            int index = colorPlateData.listArrowData[i].Row * cols + colorPlateData.listArrowData[i].Col;
            bool isLocked = false;
            if (ListColorPlate[index].status == Status.LockCoin) isLocked = true;

            ListColorPlate[index].status = (Status)colorPlateData.listArrowData[i].type;
            ListColorPlate[index].logicVisual.SetDirectionArrow(ListColorPlate[index].status, isLocked);
        }

        for (int i = 0; i < colorPlateData.listExistedData.Count; i++)
        {
            int index = colorPlateData.listExistedData[i].Row * cols + colorPlateData.listExistedData[i].Col;
            ListColorPlate[index].logicVisual.SetExistedPlate();
        }

        for (int i = 0; i < colorPlateData.listEmptyData.Count; i++)
        {
            int index = colorPlateData.listEmptyData[i].Row * cols + colorPlateData.listEmptyData[i].Col;
            ListColorPlate[index].logicVisual.DeletePlate();
        }

        goals = colorPlateData.goalScore;
        gold = colorPlateData.gold;
        pigment = colorPlateData.pigment;
    }

    #endregion

    #region InitGrid

    public void Init(int rows, int cols, Transform parent, List<ColorPlate> ListColorPlate, ColorPlate colorPlatePrefab)
    {
        GenerateGrid(rows, cols, parent, ListColorPlate, colorPlatePrefab);
    }

    void GenerateGrid(int rows, int cols, Transform parent, List<ColorPlate> ListColorPlate,
        ColorPlate colorPlatePrefab)
    {
        colorPlate = new ColorPlate[rows, cols];

        var offSetY = (((float)rows + 1) / 2.0f) * cellSize;
        var offsetX = (((float)cols - 1) / 2.0f) * cellSize;

        Vector3 startPosition = new Vector3(-offsetX, -offSetY, 0);

        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
            {
                Vector3 position = new Vector3(col, row, 0) * cellSize + startPosition;

                InstantiateColorPlate(row, col, position, parent, ListColorPlate, colorPlatePrefab);
            }
        }


        LinkColorPlate(rows, cols);
    }

    float offSetX;
    float offSetY;

    public void InitArrowPlates(int rows, int cols, List<ColorPlate> listColorPlate, Transform parent,
        ColorPlate colorPlatePrefab, List<ColorPlate> listArrowPlate)
    {
        offSetX = listColorPlate[1].transform.position.x - listColorPlate[0].transform.position.x;
        offSetY = listColorPlate[cols].transform.position.y - listColorPlate[0].transform.position.y;

        float scale;
        if (cols >= rows) scale = 6f / cols;
        else scale = 6f / rows;

        InitArrowUp(rows, cols, new Vector3(0, 0, 90f), "ArrowUp", new Vector3(0, -1.44f, 0), parent, listColorPlate,
            colorPlatePrefab, listArrowPlate, scale);
        InitArrowRight(rows, cols, new Vector3(0, 0, 0), "ArrowRight", new Vector3(-2.5f, 0, 0), parent, listColorPlate,
            colorPlatePrefab, listArrowPlate, scale);
        InitArrowLeft(rows, cols, new Vector3(0, 0, 180f), "ArrowLeft", new Vector3(2.5f, 0, 0), parent, listColorPlate,
            colorPlatePrefab, listArrowPlate, scale);
    }

    void InitArrowUp(int rows, int cols, Vector3 rotation, string arrowName, Vector3 basePosition, Transform parent,
        List<ColorPlate> listColorPlate, ColorPlate colorPlatePrefab, List<ColorPlate> listArrowPlate, float scale)
    {
        for (int i = 0; i < cols; i++)
        {
            ColorPlate arrow = Instantiate(colorPlatePrefab, parent);
            arrow.gameObject.layer = 6;
            arrow.status = Status.Up;
            //arrow.Initialize(-1, i);

            for (int j = 0; j < rows; j++)
            {
                if (colorPlate[j, i].status == Status.Empty) continue;

                arrow.LinkColorPlateArrow(colorPlate[j, i]);
                arrow.Initialize(j - 1, i);
                break;
            }

            arrow.logicVisual.transform.localEulerAngles = rotation;
            arrow.logicVisual.transform.localPosition = new Vector3(0, 0.2f, 0);
            //arrow.transform.localEulerAngles = rotation;
            parent.localScale = new Vector3(scale, scale, scale);

            Vector3 pos = new Vector3();
            if (GameManager.IsBonusGame())
            {
                pos = new Vector3(listColorPlate[0].transform.position.x + i * offSetX,
                    basePosition.y - 0.2f * scale - 2.2f, 0);
            }
            else
            {
                pos = new Vector3(listColorPlate[0].transform.position.x + i * offSetX, basePosition.y - 0.2f * scale,
                    0);
            }

            arrow.transform.position = pos;

            int max = rows >= cols ? rows : cols;
            if (max != 6)
            {
                arrow.logicVisual.transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
            }

            arrow.name = arrowName;
            listArrowPlate.Add(arrow);
        }
    }

    void InitArrowRight(int rows, int cols, Vector3 rotation, string arrowName, Vector3 basePosition, Transform parent,
        List<ColorPlate> listColorPlate, ColorPlate colorPlatePrefab, List<ColorPlate> listArrowPlate, float scale)
    {
        for (int i = 0; i < rows; i++)
        {
            ColorPlate arrow = Instantiate(colorPlatePrefab, parent);
            arrow.gameObject.layer = 6;
            arrow.status = Status.Right;
            for (int j = 0; j < cols; j++)
            {
                if (colorPlate[i, j].status == Status.Empty) continue;

                arrow.LinkColorPlateArrow(colorPlate[i, j]);
                arrow.Initialize(i, j - 1);
                break;
            }

            arrow.logicVisual.transform.localEulerAngles = rotation;
            arrow.logicVisual.transform.localPosition = new Vector3(0.2f, 0, 0);

            //arrow.transform.localEulerAngles = rotation;
            parent.localScale = new Vector3(scale, scale, scale);

            arrow.transform.position = new Vector3(basePosition.x - 0.2f * scale,
                listColorPlate[0].transform.position.y + i * offSetY, 0);

            int max = rows >= cols ? rows : cols;

            if (max != 6)
            {
                arrow.logicVisual.transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
            }

            arrow.name = arrowName;
            listArrowPlate.Add(arrow);
        }
    }


    void InitArrowLeft(int rows, int cols, Vector3 rotation, string arrowName, Vector3 basePosition, Transform parent,
        List<ColorPlate> listColorPlate, ColorPlate colorPlatePrefab, List<ColorPlate> listArrowPlate, float scale)
    {
        for (int i = 0; i < rows; i++)
        {
            ColorPlate arrow = Instantiate(colorPlatePrefab, parent);
            arrow.gameObject.layer = 6;
            arrow.status = Status.Left;
            for (int j = cols - 1; j >= 0; j--)
            {
                if (colorPlate[i, j].status == Status.Empty) continue;

                arrow.LinkColorPlateArrow(colorPlate[i, j]);
                arrow.Initialize(i, j + 1);
                break;
            }

            arrow.logicVisual.transform.localEulerAngles = rotation;
            arrow.logicVisual.transform.localPosition = new Vector3(-0.2f, 0, 0);

            //arrow.transform.localEulerAngles = rotation;
            parent.localScale = new Vector3(scale, scale, scale);

            int max = rows >= cols ? rows : cols;

            if (max != 6)
            {
                arrow.logicVisual.transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
            }

            arrow.transform.position = new Vector3(basePosition.x + 0.2f * scale,
                listColorPlate[0].transform.position.y + i * offSetY, 0);
            arrow.name = arrowName;
            listArrowPlate.Add(arrow);
        }
    }

    private void InstantiateColorPlate(int row, int col, Vector3 position, Transform parent,
        List<ColorPlate> ListColorPlate, ColorPlate colorPlatePrefab)
    {
        ColorPlate colorPlate = Instantiate(colorPlatePrefab, parent);
        //colorPlate.transform.position = position;
        colorPlate.transform.localPosition = position;
        colorPlate.status = Status.None;
        colorPlate.Init();
        colorPlate.Initialize(row, col);
        this.colorPlate[row, col] = colorPlate;
        ListColorPlate.Add(colorPlate);
    }

    void LinkColorPlate(int rows, int cols)
    {
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
            {
                ColorPlate currentCell = colorPlate[row, col];
                //Debug.Log(currentCell.name);

                if (col > 0)
                    currentCell.LinkColorPlate(colorPlate[row, col - 1]); //left
                if (col < cols - 1)
                    currentCell.LinkColorPlate(colorPlate[row, col + 1]); //right
                if (row > 0)
                    currentCell.LinkColorPlate(colorPlate[row - 1, col]); //bottom
                if (row < rows - 1)
                    currentCell.LinkColorPlate(colorPlate[row + 1, col]); //up
            }
        }
    }

    #endregion

    #region SetVisualColorPlate

    private void SelectPlateNormal(ColorPlate c)
    {
        switch (editMode)
        {
            case EditMode.SelectPlateArrowLeft:
                SetStateArrow(c, Status.Left, 90f);
                break;

            case EditMode.SelectPlateArrowRight:
                SetStateArrow(c, Status.Right, -90f);
                break;

            case EditMode.SelectPlateArrowUp:
                SetStateArrow(c, Status.Up, 180f);
                break;

            case EditMode.SelectPlateArrowDown:
                SetStateArrow(c, Status.Down, 0f);
                break;

            default:
                return;
        }
    }

    private void SetStateArrow(ColorPlate c, Status status, float yAxis)
    {
        if (c.status != status)
        {
            ArrowData normal = new ArrowData();

            c.status = status;
            c.logicVisual.SetPlateArrow();
            c.logicVisual.arrow.transform.localEulerAngles = new Vector3(0, yAxis, 0);

            normal.Row = c.Row;
            normal.Col = c.Col;
            normal.type = (int)c.status;

            for (int i = 0; i < colorPlateData.listArrowData.Count; i++)
            {
                if (colorPlateData.listArrowData[i].Row == c.Row && colorPlateData.listArrowData[i].Col == c.Col)
                {
                    colorPlateData.listArrowData.Remove(colorPlateData.listArrowData[i]);
                }

                ;
            }

            for (int i = 0; i < colorPlateData.listEmptyData.Count; i++)
            {
                if (colorPlateData.listEmptyData[i].Row == c.Row && colorPlateData.listEmptyData[i].Col == c.Col)
                {
                    colorPlateData.listEmptyData.Remove(colorPlateData.listEmptyData[i]);
                }
            }

            colorPlateData.listArrowData.Add(normal);
        }
        else
        {
            c.status = Status.None;
            c.logicVisual.Refresh();

            for (int i = 0; i < colorPlateData.listArrowData.Count; i++)
            {
                if (colorPlateData.listArrowData[i].Row == c.Row && colorPlateData.listArrowData[i].Col == c.Col)
                {
                    colorPlateData.listArrowData.Remove(colorPlateData.listArrowData[i]);
                }

                ;
            }
        }
    }

    private void SelectPlateSpecial(ColorPlate c)
    {
        switch (editMode)
        {
            case EditMode.EditCannotPlace:
                if (c.status != Status.CannotPlace)
                {
                    c.status = Status.CannotPlace;
                    c.logicVisual.SetCannotPlace();
                    SpecialData special = new SpecialData
                    {
                        Row = c.Row,
                        Col = c.Col,
                        type = (int)c.status
                    };

                    for (int i = 0; i < colorPlateData.listSpecialData.Count; i++)
                    {
                        if (colorPlateData.listSpecialData[i].Row == c.Row &&
                            colorPlateData.listSpecialData[i].Col == c.Col)
                        {
                            colorPlateData.listSpecialData.Remove(colorPlateData.listSpecialData[i]);
                        }

                        ;
                    }

                    for (int i = 0; i < colorPlateData.listExistedData.Count; i++)
                    {
                        if (colorPlateData.listExistedData[i].Row == c.Row &&
                            colorPlateData.listExistedData[i].Col == c.Col)
                        {
                            colorPlateData.listExistedData.Remove(colorPlateData.listExistedData[i]);
                        }

                        ;
                    }

                    for (int i = 0; i < colorPlateData.listEmptyData.Count; i++)
                    {
                        if (colorPlateData.listEmptyData[i].Row == c.Row &&
                            colorPlateData.listEmptyData[i].Col == c.Col)
                        {
                            colorPlateData.listEmptyData.Remove(colorPlateData.listEmptyData[i]);
                        }
                    }

                    colorPlateData.listSpecialData.Add(special);
                }
                else
                {
                    RefreshVisual(c);
                }

                break;

            case EditMode.EditLockCoin:

                if (c.status != Status.LockCoin)
                {
                    c.status = Status.LockCoin;
                    c.logicVisual.SetLockCoin();
                    SpecialData special = new SpecialData
                    {
                        Row = c.Row,
                        Col = c.Col,
                        type = (int)c.status
                    };


                    for (int i = 0; i < colorPlateData.listSpecialData.Count; i++)
                    {
                        if (colorPlateData.listSpecialData[i].Row == c.Row &&
                            colorPlateData.listSpecialData[i].Col == c.Col)
                        {
                            colorPlateData.listSpecialData.Remove(colorPlateData.listSpecialData[i]);
                        }

                        ;
                    }

                    for (int i = 0; i < colorPlateData.listExistedData.Count; i++)
                    {
                        if (colorPlateData.listExistedData[i].Row == c.Row &&
                            colorPlateData.listExistedData[i].Col == c.Col)
                        {
                            colorPlateData.listExistedData.Remove(colorPlateData.listExistedData[i]);
                        }

                        ;
                    }

                    for (int i = 0; i < colorPlateData.listEmptyData.Count; i++)
                    {
                        if (colorPlateData.listEmptyData[i].Row == c.Row &&
                            colorPlateData.listEmptyData[i].Col == c.Col)
                        {
                            colorPlateData.listEmptyData.Remove(colorPlateData.listEmptyData[i]);
                        }
                    }

                    colorPlateData.listSpecialData.Add(special);
                }
                else
                {
                    c.status = Status.None;
                    c.logicVisual.Refresh();
                    c.txtPointUnlock.text = "";

                    for (int i = 0; i < colorPlateData.listArrowData.Count; i++)
                    {
                        if (colorPlateData.listArrowData[i].Row == c.Row &&
                            colorPlateData.listArrowData[i].Col == c.Col)
                        {
                            c.status = (Status)colorPlateData.listArrowData[i].type;
                            c.logicVisual.normal.SetActive(false);
                            c.logicVisual.arrow.SetActive(true);
                        }
                    }

                    for (int i = colorPlateData.listSpecialData.Count - 1; i >= 0; i--)
                    {
                        if (colorPlateData.listSpecialData[i].Row == c.Row &&
                            colorPlateData.listSpecialData[i].Col == c.Col)
                        {
                            colorPlateData.listSpecialData.RemoveAt(i);
                        }
                    }
                }

                break;

            case EditMode.EditFrozenSquare:
                if (c.status != Status.Frozen)
                {
                    c.status = Status.Frozen;
                    c.logicVisual.SetFrozenVisual(c.Row);
                    SpecialData special = new SpecialData
                    {
                        Row = c.Row,
                        Col = c.Col,
                        type = (int)c.status
                    };


                    for (int i = 0; i < colorPlateData.listSpecialData.Count; i++)
                    {
                        if (colorPlateData.listSpecialData[i].Row == c.Row &&
                            colorPlateData.listSpecialData[i].Col == c.Col)
                        {
                            colorPlateData.listSpecialData.Remove(colorPlateData.listSpecialData[i]);
                        }

                        ;
                    }

                    for (int i = 0; i < colorPlateData.listEmptyData.Count; i++)
                    {
                        if (colorPlateData.listEmptyData[i].Row == c.Row &&
                            colorPlateData.listEmptyData[i].Col == c.Col)
                        {
                            colorPlateData.listEmptyData.Remove(colorPlateData.listEmptyData[i]);
                        }
                    }

                    colorPlateData.listSpecialData.Add(special);
                }
                else
                {
                    RefreshVisual(c);
                }

                break;

            case EditMode.EditAds:
                if (c.status != Status.Ads)
                {
                    c.status = Status.Ads;
                    c.logicVisual.SetAds();
                    SpecialData special = new SpecialData
                    {
                        Row = c.Row,
                        Col = c.Col,
                        type = (int)c.status
                    };

                    for (int i = 0; i < colorPlateData.listExistedData.Count; i++)
                    {
                        if (colorPlateData.listExistedData[i].Row == c.Row &&
                            colorPlateData.listExistedData[i].Col == c.Col)
                        {
                            colorPlateData.listExistedData.Remove(colorPlateData.listExistedData[i]);
                        }

                        ;
                    }

                    for (int i = 0; i < colorPlateData.listSpecialData.Count; i++)
                    {
                        if (colorPlateData.listSpecialData[i].Row == c.Row &&
                            colorPlateData.listSpecialData[i].Col == c.Col)
                        {
                            colorPlateData.listSpecialData.Remove(colorPlateData.listSpecialData[i]);
                        }

                        ;
                    }

                    for (int i = 0; i < colorPlateData.listEmptyData.Count; i++)
                    {
                        if (colorPlateData.listEmptyData[i].Row == c.Row &&
                            colorPlateData.listEmptyData[i].Col == c.Col)
                        {
                            colorPlateData.listEmptyData.Remove(colorPlateData.listEmptyData[i]);
                        }
                    }

                    colorPlateData.listSpecialData.Add(special);
                }
                else
                {
                    RefreshVisual(c);
                }

                break;

            case EditMode.EditPoison:
                if (c.status != Status.Poison)
                {
                    c.logicVisual.SetPoison(c.Row);
                    if (c.status == Status.Existed)
                    {
                        c.logicVisual.existed.SetActive(true);
                    }

                    c.status = Status.Poison;
                    SpecialData special = new SpecialData
                    {
                        Row = c.Row,
                        Col = c.Col,
                        type = (int)c.status
                    };

                    for (int i = 0; i < colorPlateData.listSpecialData.Count; i++)
                    {
                        if (colorPlateData.listSpecialData[i].Row == c.Row &&
                            colorPlateData.listSpecialData[i].Col == c.Col)
                        {
                            colorPlateData.listSpecialData.Remove(colorPlateData.listSpecialData[i]);
                        }

                        ;
                    }

                    for (int i = 0; i < colorPlateData.listEmptyData.Count; i++)
                    {
                        if (colorPlateData.listEmptyData[i].Row == c.Row &&
                            colorPlateData.listEmptyData[i].Col == c.Col)
                        {
                            colorPlateData.listEmptyData.Remove(colorPlateData.listEmptyData[i]);
                        }
                    }

                    colorPlateData.listSpecialData.Add(special);
                }
                else
                {
                    RefreshVisual(c);
                }

                break;

            case EditMode.EditWood:
                if (c.status != Status.Wood)
                {
                    c.status = Status.Wood;
                    c.logicVisual.SetWood();

                    SpecialData special = new SpecialData
                    {
                        Row = c.Row,
                        Col = c.Col,
                        type = (int)c.status
                    };

                    for (int i = 0; i < colorPlateData.listExistedData.Count; i++)
                    {
                        if (colorPlateData.listExistedData[i].Row == c.Row &&
                            colorPlateData.listExistedData[i].Col == c.Col)
                        {
                            colorPlateData.listExistedData.Remove(colorPlateData.listExistedData[i]);
                        }

                        ;
                    }

                    for (int i = 0; i < colorPlateData.listSpecialData.Count; i++)
                    {
                        if (colorPlateData.listSpecialData[i].Row == c.Row &&
                            colorPlateData.listSpecialData[i].Col == c.Col)
                        {
                            colorPlateData.listSpecialData.Remove(colorPlateData.listSpecialData[i]);
                        }

                        ;
                    }

                    for (int i = 0; i < colorPlateData.listEmptyData.Count; i++)
                    {
                        if (colorPlateData.listEmptyData[i].Row == c.Row &&
                            colorPlateData.listEmptyData[i].Col == c.Col)
                        {
                            colorPlateData.listEmptyData.Remove(colorPlateData.listEmptyData[i]);
                        }
                    }

                    colorPlateData.listSpecialData.Add(special);
                }
                else
                {
                    RefreshVisual(c);
                }

                break;

            case EditMode.EditBags:
                if (c.status != Status.Bag)
                {
                    c.status = Status.Bag;
                    c.logicVisual.SetBagVisual();

                    SpecialData special = new SpecialData
                    {
                        Row = c.Row,
                        Col = c.Col,
                        type = (int)c.status
                    };

                    for (int i = 0; i < colorPlateData.listExistedData.Count; i++)
                    {
                        if (colorPlateData.listExistedData[i].Row == c.Row &&
                            colorPlateData.listExistedData[i].Col == c.Col)
                        {
                            colorPlateData.listExistedData.Remove(colorPlateData.listExistedData[i]);
                        }

                        ;
                    }

                    for (int i = 0; i < colorPlateData.listSpecialData.Count; i++)
                    {
                        if (colorPlateData.listSpecialData[i].Row == c.Row &&
                            colorPlateData.listSpecialData[i].Col == c.Col)
                        {
                            colorPlateData.listSpecialData.Remove(colorPlateData.listSpecialData[i]);
                        }

                        ;
                    }

                    for (int i = 0; i < colorPlateData.listEmptyData.Count; i++)
                    {
                        if (colorPlateData.listEmptyData[i].Row == c.Row &&
                            colorPlateData.listEmptyData[i].Col == c.Col)
                        {
                            colorPlateData.listEmptyData.Remove(colorPlateData.listEmptyData[i]);
                        }
                    }

                    colorPlateData.listSpecialData.Add(special);
                }
                else
                {
                    RefreshVisual(c);
                }

                break;
            default:
                return;
        }
    }

    private void RefreshVisual(ColorPlate c)
    {
        c.status = Status.None;
        c.logicVisual.Refresh();

        for (int i = colorPlateData.listSpecialData.Count - 1; i >= 0; i--)
        {
            if (colorPlateData.listSpecialData[i].Row == c.Row &&
                colorPlateData.listSpecialData[i].Col == c.Col)
            {
                colorPlateData.listSpecialData.RemoveAt(i);
            }
        }
    }

    private void SelectExistedPlate(ColorPlate c)
    {
        if (editMode == EditMode.SelectExistedPlate)
        {
            if (c.status != Status.Existed)
            {
                c.status = Status.Existed;
                ExistedData existed = new ExistedData
                {
                    Row = c.Row,
                    Col = c.Col,
                    type = (int)c.status
                };
                c.logicVisual.SetExistedPlate();

                for (int i = 0; i < colorPlateData.listSpecialData.Count; i++)
                {
                    if (colorPlateData.listSpecialData[i].Row == c.Row &&
                        colorPlateData.listSpecialData[i].Col == c.Col)
                    {
                        // if (colorPlateData.listSpecialData[i].type == (int)Status.CannotPlace ||
                        //     colorPlateData.listSpecialData[i].type == (int)Status.LockCoin)
                        if (CheckClearExisted(colorPlateData.listSpecialData[i]))
                        {
                            colorPlateData.listSpecialData.Remove(colorPlateData.listSpecialData[i]);
                            break;
                        }
                        else
                            break;
                    }

                    ;
                }

                for (int i = 0; i < colorPlateData.listEmptyData.Count; i++)
                {
                    if (colorPlateData.listEmptyData[i].Row == c.Row && colorPlateData.listEmptyData[i].Col == c.Col)
                    {
                        colorPlateData.listEmptyData.Remove(colorPlateData.listEmptyData[i]);
                    }
                }

                colorPlateData.listExistedData.Add(existed);
            }
            else
            {
                c.status = Status.None;
                c.logicVisual.existed.SetActive(false);

                for (int i = 0; i < colorPlateData.listExistedData.Count; i++)
                {
                    if (colorPlateData.listExistedData[i].Row == c.Row &&
                        colorPlateData.listExistedData[i].Col == c.Col)
                    {
                        colorPlateData.listExistedData.Remove(colorPlateData.listExistedData[i]);
                    }

                    ;
                }
            }
        }
    }

    private bool CheckClearExisted(SpecialData c)
    {
        bool result = false;

        if (c.type == (int)Status.CannotPlace || c.type == (int)Status.LockCoin || c.type == (int)Status.Bag ||
            c.type == (int)Status.Wood)
        {
            return result = true;
        }

        return result;
    }

    private void ClearPlate(ColorPlate c)
    {
        if (editMode == EditMode.ClearPlate)
        {
            c.status = Status.None;
            c.logicVisual.Refresh();
            c.txtPointUnlock.text = "";

            for (int i = 0; i < colorPlateData.listSpecialData.Count; i++)
            {
                if (colorPlateData.listSpecialData[i].Row == c.Row && colorPlateData.listSpecialData[i].Col == c.Col)
                {
                    colorPlateData.listSpecialData.Remove(colorPlateData.listSpecialData[i]);
                }
            }

            for (int i = 0; i < colorPlateData.listArrowData.Count; i++)
            {
                if (colorPlateData.listArrowData[i].Row == c.Row && colorPlateData.listArrowData[i].Col == c.Col)
                {
                    colorPlateData.listArrowData.Remove(colorPlateData.listArrowData[i]);
                }
            }

            for (int i = 0; i < colorPlateData.listExistedData.Count; i++)
            {
                if (colorPlateData.listExistedData[i].Row == c.Row && colorPlateData.listExistedData[i].Col == c.Col)
                {
                    colorPlateData.listExistedData.Remove(colorPlateData.listExistedData[i]);
                }
            }
        }
    }

    private void DeletePlate(ColorPlate c)
    {
        if (editMode == EditMode.DeletePlate)
        {
            if (c.status != Status.Empty)
            {
                c.status = Status.Empty;
                c.logicVisual.DeletePlate();
                EmptyData empty = new EmptyData();
                empty.Row = c.Row;
                empty.Col = c.Col;
                empty.type = (int)c.status;
                c.txtPointUnlock.text = "";

                for (int i = 0; i < colorPlateData.listSpecialData.Count; i++)
                {
                    if (colorPlateData.listSpecialData[i].Row == c.Row &&
                        colorPlateData.listSpecialData[i].Col == c.Col)
                    {
                        colorPlateData.listSpecialData.Remove(colorPlateData.listSpecialData[i]);
                    }
                }

                for (int i = 0; i < colorPlateData.listArrowData.Count; i++)
                {
                    if (colorPlateData.listArrowData[i].Row == c.Row && colorPlateData.listArrowData[i].Col == c.Col)
                    {
                        colorPlateData.listArrowData.Remove(colorPlateData.listArrowData[i]);
                    }
                }

                for (int i = 0; i < colorPlateData.listExistedData.Count; i++)
                {
                    if (colorPlateData.listExistedData[i].Row == c.Row &&
                        colorPlateData.listExistedData[i].Col == c.Col)
                    {
                        colorPlateData.listExistedData.Remove(colorPlateData.listExistedData[i]);
                    }
                }

                colorPlateData.listEmptyData.Add(empty);
            }
            else
            {
                c.status = Status.None;
                c.logicVisual.Refresh();

                for (int i = 0; i < colorPlateData.listEmptyData.Count; i++)
                {
                    if (colorPlateData.listEmptyData[i].Row == c.Row && colorPlateData.listEmptyData[i].Col == c.Col)
                    {
                        colorPlateData.listEmptyData.Remove(colorPlateData.listEmptyData[i]);
                    }
                }
            }
        }
    }

    #endregion
}