using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class AvatarBuild : MonoBehaviour
{
    public Transform spawnPoint;
    public TextMeshProUGUI woodCountUI;

    public int currentWood = 100;

    public BuildSO[] allBuildType;

    public float scrollAmountToSwitch = 2;


    private int minimumWoodToBuild;

    private BuildSO selectedBuildType;
    private int selectedBuidIndex;

    private Buildable currentBuildablePlacing;
    private bool isPlacingBuild;
    private bool canPlaceBuild;

    private float scrollAmount = 0;

    public UnityEvent OnSwitchBuild;
    public UnityEvent OnBuild;

    public UnityEvent OnPickupWood;

    void Start()
    {
        isPlacingBuild = false;
        canPlaceBuild = true;
        
        minimumWoodToBuild = allBuildType[0].woodCost;
        if(allBuildType.Length > 1)
        {
            for (int i = 1; i < allBuildType.Length; i++)
            {
                if (allBuildType[i].woodCost < minimumWoodToBuild) minimumWoodToBuild = allBuildType[i].woodCost;
            }
        }

        SetBuildType(0, true);

        UpdateWoodCount();
    }

    void Update()
    {
        /*if (currentWood < selectedBuildType.woodCost)
        {
            if (currentWood < minimumWoodToBuild) return;

            int safeEscape = 0;
            while(currentWood < selectedBuildType.woodCost)
            {
                SetBuildType(selectedBuidIndex + 1);
                safeEscape++;
                if(safeEscape > 100)
                {
                    Debug.LogError("infinite loop avoided !");
                    return;
                }
            }
            return;
        }*/

        //SCROLL
        float scroll = Input.mouseScrollDelta.y;
        if (scroll * scrollAmount < 0) scrollAmount = 0;//If opposite sign, we reset the scroll amount
        scrollAmount += scroll;

        if (scrollAmount < -scrollAmountToSwitch)
        {
            SetBuildType(selectedBuidIndex - 1, false);
            scrollAmount += scrollAmountToSwitch;
        }
        else if (scrollAmount > scrollAmountToSwitch)
        {
            SetBuildType(selectedBuidIndex + 1);
            scrollAmount -= scrollAmountToSwitch;
        }

        if (Input.GetKeyDown(KeyCode.A)) SetBuildType(selectedBuidIndex - 1);
        if (Input.GetKeyDown(KeyCode.E)) SetBuildType(selectedBuidIndex + 1);

        //PLACING
        if (isPlacingBuild)//If we are placing a build
        {
            //Cancel
            if (Input.GetMouseButtonDown(1)) StopPlacing();

            if (Input.GetMouseButtonDown(0))//Confirm
            {
                if(canPlaceBuild)
                {
                    currentWood -= selectedBuildType.woodCost;
                    UpdateWoodCount();

                    currentBuildablePlacing.Place(spawnPoint);
                    StopPlacing(false);

                    OnBuild?.Invoke();

                }else StopPlacing();//We cancel if we can't
            }
        }
        else
        {
            if (Input.GetMouseButtonDown(1))//Start placing
            {
                StartPlacing();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.transform.CompareTag("WoodPiece")) return;

        Destroy(other.gameObject);
        currentWood += 25;
        UpdateWoodCount();

        OnPickupWood?.Invoke();
    }

    private void UpdateWoodCount()
    {
        woodCountUI.text = "Wood: " + currentWood;
    }

    private void StartPlacing()
    {
        canPlaceBuild = selectedBuildType.woodCost <= currentWood;
        currentBuildablePlacing = Instantiate(selectedBuildType.buildPrefab, spawnPoint);
        currentBuildablePlacing.Setup(canPlaceBuild);

        isPlacingBuild = true;
    }

    private void StopPlacing(bool destroyCurrent = true)
    {
        if(destroyCurrent && currentBuildablePlacing != null) Destroy(currentBuildablePlacing.gameObject);
        currentBuildablePlacing = null;

        isPlacingBuild = false;
    }

    private void SetBuildType(int index, bool setup = false)
    {
        if (index < 0) index = allBuildType.Length - 1;
        else if (index >= allBuildType.Length) index = 0;

        if (!setup) OnSwitchBuild?.Invoke();

        //If we don't have enough wood to place this
        /*if (!canPlaceBuild)
        {
            if (currentWood >= minimumWoodToBuild) StopPlacing();
            else
            {
                int indexChange = scrollUp ? 1 : -1;//If we're in scroll up mode, we switch to the greater index
                SetBuildType(selectedBuidIndex + indexChange);
            }
            return;
        }*/

        selectedBuildType = allBuildType[index];
        selectedBuidIndex = index;

        if (isPlacingBuild)
        {
            //We restart the placement with the new build type
            StopPlacing();
            StartPlacing();
        }
    }
}
