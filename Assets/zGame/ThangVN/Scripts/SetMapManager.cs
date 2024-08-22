using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using System;

public class SetMapManager : MonoBehaviour
{
    [SerializeField] GameMode gameMode;
    [SerializeField] public ColorPlateData colorPlateData;
    public List<ColorPlate> listPlates;
    public int level;
    public int rows;
    public int cols;
    float cellSize;
    private ColorPlate[,] colorPlate;
    [SerializeField] ColorPlate colorPlatePrefab;
    [SerializeField] ColorPlate arrowPlatePrefab;
    [SerializeField] ColorPlate emptyHolder;
    [SerializeField] List<ColorPlate> ListColorPlate;
    [SerializeField] Transform holderColorPlate;
    [SerializeField] int goals;
    [SerializeField] int gold;
    [SerializeField] int pigment;

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
        EditAds
    }
    public EditMode editMode;
    public LayerMask layerMask;
    private void Start()
    {
        cellSize = 1.1f;
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

    public void SaveData()
    {
        colorPlateData.goalScore = goals;
        colorPlateData.gold = gold;
        colorPlateData.pigment = pigment;
        string data = JsonUtility.ToJson(colorPlateData);
        File.WriteAllText($"Assets/Resources/LevelData/Level_{level}.json", data);
    }


    public void LoadData()
    {
        string filePath = "";
        if (!SaveGame.Challenges)
        {
            //Debug.Log("Level: " + SaveGame.Level);
            filePath = Resources.Load<TextAsset>($"LevelData/Level_{SaveGame.Level}").ToString();
        }
        else
        {
            filePath = Resources.Load<TextAsset>($"LevelData/Level_{1000}").ToString();
        }
        colorPlateData = JsonUtility.FromJson<ColorPlateData>(filePath);
    }

    void LoadDataSetMap(int level)
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
            ListColorPlate[index].logicVisual.SetSpecialSquare(ListColorPlate[index].status);

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
    public void Init(int rows, int cols, Transform parent, List<ColorPlate> ListColorPlate, ColorPlate colorPlatePrefab)
    {
        GenerateGrid(rows, cols, parent, ListColorPlate, colorPlatePrefab);
    }
    void GenerateGrid(int rows, int cols, Transform parent, List<ColorPlate> ListColorPlate, ColorPlate colorPlatePrefab)
    {
        colorPlate = new ColorPlate[rows, cols];

        var offSetZ = (((float)rows + 1) / 2.0f) * cellSize;
        var offsetX = (((float)cols - 1) / 2.0f) * cellSize;

        Vector3 startPosition = new Vector3(-offsetX, 0, -offSetZ);

        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
            {
                Vector3 position = new Vector3(col, 0, row) * cellSize + startPosition;

                InstantiateColorPlate(row, col, position, parent, ListColorPlate, colorPlatePrefab);
            }
        }


        LinkColorPlate(rows, cols);
    }
    private void InstantiateColorPlate(int row, int col, Vector3 position, Transform parent, List<ColorPlate> ListColorPlate, ColorPlate colorPlatePrefab)
    {
        ColorPlate colorPlate = Instantiate(colorPlatePrefab, parent);
        //colorPlate.transform.position = position;
        colorPlate.transform.position = position;
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
    void SelectPlateNormal(ColorPlate c)
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
    void SetStateArrow(ColorPlate c, Status status, float yAxis)
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
                };
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
                };
            }
        }
    }
    void SelectPlateSpecial(ColorPlate c)
    {
        switch (editMode)
        {
            case EditMode.EditCannotPlace:
                if (c.status != Status.CannotPlace)
                {
                    SpecialData special = new SpecialData();

                    c.status = Status.CannotPlace;
                    c.logicVisual.SetCannotPlace();

                    special.Row = c.Row;
                    special.Col = c.Col;
                    special.type = (int)c.status;

                    for (int i = 0; i < colorPlateData.listSpecialData.Count; i++)
                    {
                        if (colorPlateData.listSpecialData[i].Row == c.Row && colorPlateData.listSpecialData[i].Col == c.Col)
                        {
                            colorPlateData.listSpecialData.Remove(colorPlateData.listSpecialData[i]);
                        };
                    }

                    for (int i = 0; i < colorPlateData.listExistedData.Count; i++)
                    {
                        if (colorPlateData.listExistedData[i].Row == c.Row && colorPlateData.listExistedData[i].Col == c.Col)
                        {
                            colorPlateData.listExistedData.Remove(colorPlateData.listExistedData[i]);

                        };
                    }

                    for (int i = 0; i < colorPlateData.listEmptyData.Count; i++)
                    {
                        if (colorPlateData.listEmptyData[i].Row == c.Row && colorPlateData.listEmptyData[i].Col == c.Col)
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

                    for (int i = colorPlateData.listSpecialData.Count - 1; i >= 0; i--)
                    {
                        if (colorPlateData.listSpecialData[i].Row == c.Row && colorPlateData.listSpecialData[i].Col == c.Col)
                        {
                            colorPlateData.listSpecialData.RemoveAt(i);
                        }
                    }
                }
                break;

            case EditMode.EditLockCoin:

                if (c.status != Status.LockCoin)
                {
                    SpecialData special = new SpecialData();

                    c.status = Status.LockCoin;
                    c.logicVisual.SetLockCoin();

                    special.Row = c.Row;
                    special.Col = c.Col;
                    special.type = (int)c.status;

                    for (int i = 0; i < colorPlateData.listSpecialData.Count; i++)
                    {
                        if (colorPlateData.listSpecialData[i].Row == c.Row && colorPlateData.listSpecialData[i].Col == c.Col)
                        {
                            colorPlateData.listSpecialData.Remove(colorPlateData.listSpecialData[i]);
                        };
                    }

                    for (int i = 0; i < colorPlateData.listExistedData.Count; i++)
                    {
                        if (colorPlateData.listExistedData[i].Row == c.Row && colorPlateData.listExistedData[i].Col == c.Col)
                        {
                            colorPlateData.listExistedData.Remove(colorPlateData.listExistedData[i]);

                        };
                    }

                    for (int i = 0; i < colorPlateData.listEmptyData.Count; i++)
                    {
                        if (colorPlateData.listEmptyData[i].Row == c.Row && colorPlateData.listEmptyData[i].Col == c.Col)
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
                        if (colorPlateData.listArrowData[i].Row == c.Row && colorPlateData.listArrowData[i].Col == c.Col)
                        {
                            c.status = (Status)colorPlateData.listArrowData[i].type;
                            c.logicVisual.normal.SetActive(false);
                            c.logicVisual.arrow.SetActive(true);
                        }
                    }

                    for (int i = colorPlateData.listSpecialData.Count - 1; i >= 0; i--)
                    {
                        if (colorPlateData.listSpecialData[i].Row == c.Row && colorPlateData.listSpecialData[i].Col == c.Col)
                        {
                            colorPlateData.listSpecialData.RemoveAt(i);
                        }
                    }
                }
                break;

            case EditMode.EditFrozenSquare:
                if (c.status != Status.Frozen)
                {
                    SpecialData special = new SpecialData();

                    c.status = Status.Frozen;
                    c.logicVisual.SetFrozen();

                    special.Row = c.Row;
                    special.Col = c.Col;
                    special.type = (int)c.status;

                    for (int i = 0; i < colorPlateData.listSpecialData.Count; i++)
                    {
                        if (colorPlateData.listSpecialData[i].Row == c.Row && colorPlateData.listSpecialData[i].Col == c.Col)
                        {
                            colorPlateData.listSpecialData.Remove(colorPlateData.listSpecialData[i]);
                        };
                    }

                    for (int i = 0; i < colorPlateData.listEmptyData.Count; i++)
                    {
                        if (colorPlateData.listEmptyData[i].Row == c.Row && colorPlateData.listEmptyData[i].Col == c.Col)
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

                    for (int i = colorPlateData.listSpecialData.Count - 1; i >= 0; i--)
                    {
                        if (colorPlateData.listSpecialData[i].Row == c.Row && colorPlateData.listSpecialData[i].Col == c.Col)
                        {
                            colorPlateData.listSpecialData.RemoveAt(i);
                        }
                    }
                }

                break;

            case EditMode.EditAds:
                if (c.status != Status.Ads)
                {
                    SpecialData special = new SpecialData();

                    c.status = Status.Ads;
                    c.logicVisual.SetAds();

                    special.Row = c.Row;
                    special.Col = c.Col;
                    special.type = (int)c.status;

                    for (int i = 0; i < colorPlateData.listSpecialData.Count; i++)
                    {
                        if (colorPlateData.listSpecialData[i].Row == c.Row && colorPlateData.listSpecialData[i].Col == c.Col)
                        {
                            colorPlateData.listSpecialData.Remove(colorPlateData.listSpecialData[i]);
                        };
                    }

                    for (int i = 0; i < colorPlateData.listEmptyData.Count; i++)
                    {
                        if (colorPlateData.listEmptyData[i].Row == c.Row && colorPlateData.listEmptyData[i].Col == c.Col)
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

                    for (int i = colorPlateData.listSpecialData.Count - 1; i >= 0; i--)
                    {
                        if (colorPlateData.listSpecialData[i].Row == c.Row && colorPlateData.listSpecialData[i].Col == c.Col)
                        {
                            colorPlateData.listSpecialData.RemoveAt(i);
                        }
                    }
                }

                break;

            default:
                return;
        }

    }
    void SelectExistedPlate(ColorPlate c)
    {
        if (editMode == EditMode.SelectExistedPlate)
        {
            if (c.status != Status.Existed)
            {
                c.status = Status.Existed;
                ExistedData existed = new ExistedData();
                existed.Row = c.Row;
                existed.Col = c.Col;
                existed.type = (int)c.status;
                c.logicVisual.SetExistedPlate();

                for (int i = 0; i < colorPlateData.listSpecialData.Count; i++)
                {
                    if (colorPlateData.listSpecialData[i].Row == c.Row && colorPlateData.listSpecialData[i].Col == c.Col)
                    {
                        if (colorPlateData.listSpecialData[i].type == (int)Status.CannotPlace || colorPlateData.listSpecialData[i].type == (int)Status.LockCoin)
                        {
                            colorPlateData.listSpecialData.Remove(colorPlateData.listSpecialData[i]);
                            break;
                        }
                        else
                            break;
                    };
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
                    if (colorPlateData.listExistedData[i].Row == c.Row && colorPlateData.listExistedData[i].Col == c.Col)
                    {
                        colorPlateData.listExistedData.Remove(colorPlateData.listExistedData[i]);
                    };
                }
            }
        }
    }
    void ClearPlate(ColorPlate c)
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
    void DeletePlate(ColorPlate c)
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



    //void Generate()
    //{
    //    colorPlate = new ColorPlate[rows, cols];

    //    float gridWidth = (cols - 1) * cellSize;
    //    float gridHeight = (rows - 1) * cellSize;

    //    Vector3 startPosition = new Vector3(-gridWidth / 2, 0, -gridHeight / 2);

    //    for (int row = 0; row < rows; row++)
    //    {
    //        for (int col = 0; col < cols; col++)
    //        {
    //            Vector3 position = startPosition + new Vector3(col * cellSize, 0, row * cellSize);

    //            ColorPlate colorPlate = Instantiate(colorPlatePrefab, holderColorPlate);
    //            colorPlate.transform.localPosition = position;
    //            colorPlate.status = Status.None;
    //            colorPlate.Init();
    //            colorPlate.Initialize(row, col);
    //            this.colorPlate[row, col] = colorPlate;
    //            ListColorPlate.Add(colorPlate);
    //        }
    //    }
    //}
}
