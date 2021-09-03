using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    private static PlayerControl instance;
    public static PlayerControl Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<PlayerControl>();
            return instance;
        }
    }

    public Animator anim;

    public float MovementSpeed = 2;
    private float movementPercentage;
    private float distToTravel = 1;

    private Tile StartTile;
    private Tile currTile;
    private Tile endTile;
    public bool isMove;
    public bool isDig;
    public bool isIdel;

    [System.Flags]
    public enum Face : int
    {
        Up,
        Down,
        Left,
        Right = 3,
    }
    public Face myFace = Face.Down;

    private int _animH;
    private int _animV;
    private int _animFace;
    private int _animDig;

    private void Awake()
    {
        if (Instance == null)
            instance = this;

        _animH = Animator.StringToHash("H");
        _animV = Animator.StringToHash("V");
        _animFace = Animator.StringToHash("Face");
        _animDig = Animator.StringToHash("sycthe");
    }

    void Start()
    {
        StartTile = endTile = currTile = GameWorldManager.Instance.GetTileAt(transform.position);

        GameWorldManager.ReSetObject += ResetPlayer;
    }

    public void ResetPlayer()
    {
        transform.position = StartTile.position;
        endTile = currTile = StartTile;
        isMove = isDig = false;
    }

    void Update()
    {
        if (ShopManager.Instance && ShopManager.Instance.isShop)
            return;

        Vector2 moveDir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if (!isMove && !isDig)
        {
            if (moveDir != Vector2.zero)
                SetEndTile(moveDir);

            CheckFace(moveDir);
        }

        if (Input.GetKeyDown(KeyCode.Space) && !isMove)
            Use(moveDir);

        if (!isDig)
            Move();
    }

    public void Move()
    {
        if (endTile == null || endTile == currTile)
            return;

        isMove = true;
        float distThisFrame = MovementSpeed * Time.deltaTime;
        movementPercentage += distThisFrame / distToTravel;
        if (movementPercentage >= 1)
        {
            transform.position = endTile.position;
            currTile = endTile;

            movementPercentage = 0;
            isMove = false;

            return;
        }

        transform.position = Vector3.Lerp(currTile.position, endTile.position, movementPercentage);
    }

    public void CheckFace(Vector2 moveDir)
    {
        isIdel = false;
        if (moveDir.x > 0)
            myFace = Face.Right;
        else if (moveDir.x < 0)
            myFace = Face.Left;
        else if (moveDir.y > 0)
            myFace = Face.Up;
        else if (moveDir.y < 0)
            myFace = Face.Down;
        else if (moveDir == Vector2.zero)
            isIdel = true;

        PlayAnim(moveDir);
    }

    public void PlayAnim(Vector2 moveDir)
    {
        if (!isIdel || isDig)
        {
            switch (myFace)
            {
                case Face.Right:
                    SetAnim(moveDir, 1, 0);
                    break;
                case Face.Left:
                    SetAnim(moveDir, -1, 0);
                    break;
                case Face.Up:
                    SetAnim(moveDir, 0, 1);
                    break;
                case Face.Down:
                    SetAnim(moveDir, 0, -1);
                    break;
            }
        }
        else if (!isDig)
            SetAnim(moveDir, 0, 0);
    }

    public void SetAnim(Vector2 moveDir, float h = 0, float v = 0)
    {
        if (moveDir != Vector2.zero)
            if (moveDir.x > 0)
                anim.transform.localScale = new Vector3(-1, 1, 1);
            else
                anim.transform.localScale = new Vector3(1, 1, 1);

        if (isDig)
            anim.SetTrigger(_animDig);

        anim.SetFloat(_animH, h);
        anim.SetFloat(_animV, v);

        anim.SetFloat(_animFace, (int)myFace);
    }

    public void SetEndTile(Vector2 moveDir)
    {
        Tile nextTile = null;
        Vector2 nextPos = new Vector2(currTile.X + moveDir.x, currTile.Y);

        if (moveDir.x != 0)
            nextTile = CheckTile(nextPos, true);

        if (nextTile == null)
        {
            nextPos = new Vector2(currTile.X, currTile.Y + moveDir.y);
            nextTile = CheckTile(nextPos, true);
        }

        if (nextTile != null)
            endTile = nextTile;
    }

    private Tile CheckTile(Vector2 nextPos, bool isWalk = false)
    {
        Tile nextTile = null;
        nextTile = GameWorldManager.Instance.GetTileAt(nextPos);

        bool canWalk = true;
        if (nextTile != null && isWalk)
            canWalk = nextTile.NowTileData.CanWalk;

        if (nextTile != null && canWalk)
        {
            return nextTile;
        }
        return null;
    }

    public void Use(Vector2 moveDir)
    {
        Tile FaceTile = null;
        switch (myFace)
        {
            case Face.Right:
                FaceTile = CheckTile(currTile.position + new Vector3(1, 0, 0));
                break;
            case Face.Left:
                FaceTile = CheckTile(currTile.position + new Vector3(-1, 0, 0));
                break;
            case Face.Up:
                FaceTile = CheckTile(currTile.position + new Vector3(0, 1, 0));
                break;
            case Face.Down:
                FaceTile = CheckTile(currTile.position + new Vector3(0, -1, 0));
                break;
        }

        if (FaceTile == null || !FaceTile.NowTileData.CanDig)
            return;

        if (BackPackManager.Instance.GameObjectItemUIDictionary.Count == BackPackManager.Instance.BagMax)
        {
            ShakeUI.Instance.SetText("­I¥]¤wº¡");
            return;
        }

        isDig = true;

        PlayAnim(moveDir);

        FaceTile.OnDigTile();
    }
}