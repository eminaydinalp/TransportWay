using System;
using _GAME.__Scripts.Home;
using _GAME.__Scripts.Package;
using DG.Tweening;
using Rentire.Core;
using Rentire.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TutorialManager : Singleton<TutorialManager>
{
    public static event UnityAction OnPassNextTutorialStep;
    private int _currentTutorialStep = 1;
    public int CurrentTutorialStep => _currentTutorialStep;

    [SerializeField] private GameObject packagePrefab;
    [SerializeField] private Button AddCarButton;
    [SerializeField] private Button ClickSpeedButton;
    [SerializeField] private Button MergeButton;
    [SerializeField] public GameObject handGo;
    [SerializeField] private TMP_Text tutorialText;
    [SerializeField] private GameObject tutorialVignette;
    private GameObject _nodeGo;
    private Vector3 _initialPackagePos;
    private PackageController _initialPackage;

    [HideInInspector] public bool canSpawnCubes = false;

    private void Awake()
    {
        InitialTutorialConfig();
        ShowTutorialStep();
    }


    private void InitialTutorialConfig()
    {
        if (!UserPrefs.GetTutorial())
        {
            canSpawnCubes = true;
            return;
        }

        AddCarButton.gameObject.SetActive(false);
        ClickSpeedButton.gameObject.SetActive(false);
        MergeButton.gameObject.SetActive(false);
        _nodeGo = FindObjectOfType<HomeController>().targetSphere;
    }

    private void NextStep()
    {
        _currentTutorialStep++;
    }

    public void InvokeNextTutorialStep()
    {
        OnPassNextTutorialStep?.Invoke();
    }

    private void OnEnable()
    {
        OnPassNextTutorialStep += ShowTutorialStep;
    }

    private void OnDisable()
    {
        OnPassNextTutorialStep -= ShowTutorialStep;
    }

    private void ShowTutorialStep()
    {
        if (!UserPrefs.GetTutorial())
            return;

        switch (_currentTutorialStep)
        {
            case 1:
                CallMethodWithDelay(SpawnPackageAndShowFinger, 1.5f);
                break;
            case 2:
                CallMethodWithDelay(SpawnOtherPackage, 1);
                break;
            case 3:
                CanSpawnCubesAndActivateButtons();
                break;
            case 4:
                ActivateMergeButton();
                break;
        }
    }

    private void OnApplicationQuit()
    {
        FinishTutorial();
    }

    private void SpawnPackageAndShowFinger()
    {
        var homeController = FindObjectOfType<HomeController>();
        var homePos = homeController.gameObject.transform.position;
        var packageSpawner = FindObjectOfType<PackageSpawner>();
        var cubeStack = Instantiate(packagePrefab, homePos + (Vector3.forward * 5), Quaternion.identity);
        homeController.removeSpline.GetComponent<SpriteRenderer>().DOFade(0, 0);
        _initialPackage = cubeStack.GetComponent<PackageController>();
        _initialPackagePos = cubeStack.GetComponent<PackageController>().transform.position;
        cubeStack.GetComponent<PackageController>().packageSpawner = packageSpawner;
        cubeStack.transform.DOScale(1, 0.3f).From(0).SetEase(Ease.InOutCirc).OnComplete(() =>
        {
            var tutorialText = cubeStack.GetComponent<PackageController>().packageTutorialText;
            tutorialText.gameObject.SetActive(true);
            tutorialText.transform.DOPunchScale(0.6f * Vector3.one, 0.3f, 0, 0.1f);
        });
        CallMethodWithDelay(ShowFingerTutorial, 1.5f);
    }

    private void ShowFingerTutorial()
    {
        foreach (Transform child in _nodeGo.transform)
        {
            child.GetComponent<SpriteRenderer>().color = Color.green;
        }

        tutorialText.gameObject.SetActive(true);
        tutorialText.transform.DOPunchScale(0.05f * Vector3.one, 0.5f, 0, 0.1f).SetLoops(8);

        _nodeGo.transform.DOPunchScale(0.4f * Vector3.one, 0.5f, 0, 0.1f).SetLoops(8).OnComplete(() =>
        {
            tutorialText.gameObject.SetActive(false);
            foreach (Transform child in _nodeGo.transform)
            {
                child.GetComponent<SpriteRenderer>().color = Color.black;
            }
        });
        handGo.SetActive(true);
        handGo.transform.DOMoveZ(handGo.transform.position.z + 7, 2).SetLoops(3).OnComplete(() =>
        {
            handGo.SetActive(false);
        });
        NextStep();
    }

    public void ShowNewAreaWithHand()
    {
        handGo.SetActive(true);
        handGo.transform.localPosition = new Vector3(5.8f, 12, handGo.transform.localPosition.z);
        handGo.transform.DOPunchScale(0.2f * Vector3.one, 0.5f, 0, 0.1f).SetLoops(8)
            .OnComplete(() => handGo.SetActive(false));
    }


    private void SpawnOtherPackage()
    {
        var homeController = FindObjectOfType<HomeController>();
        var packageSpawner = FindObjectOfType<PackageSpawner>();
        if (_initialPackage != null)
        {
            var oldPackage = _initialPackage;
            oldPackage.packageTutorialText.SetActive(false);
        }

        var cubeStack = Instantiate(packagePrefab, _initialPackagePos + (Vector3.right * 3), Quaternion.identity);
        cubeStack.GetComponent<PackageController>().packageSpawner = packageSpawner;
        cubeStack.transform.DOScale(1, 0.3f).From(0).SetEase(Ease.InOutCirc).OnComplete(() =>
        {
            tutorialText.gameObject.SetActive(true);
            tutorialText.text = "You Can Remove Ways";
            var newHand = Instantiate(handGo);
            newHand.SetActive(true);
            newHand.transform.localPosition = new Vector3(2, -15f, handGo.transform.position.z);
            tutorialText.transform.DOPunchScale(0.05f * Vector3.one, 0.5f, 0, 0.1f).SetLoops(8);
            homeController.removeSpline.GetComponent<SpriteRenderer>().DOFade(1, 0);
            newHand.transform.DOPunchScale(0.2f * Vector3.one, 0.5f, 0, 0.1f).SetLoops(8);
            homeController.removeSpline.transform.DOPunchScale(0.4f * Vector3.one, 0.5f, 0, 0.1f).SetLoops(8)
                .OnComplete(() =>
                {
                    tutorialText.gameObject.SetActive(false);
                    newHand.SetActive(false);
                    foreach (Transform child in _nodeGo.transform)
                    {
                        child.GetComponent<SpriteRenderer>().color = Color.green;
                    }

                    _nodeGo.transform.DOPunchScale(0.4f * Vector3.one, 0.5f, 0, 0.1f).SetLoops(10).OnComplete(() =>
                    {
                        tutorialText.gameObject.SetActive(false);
                        foreach (Transform child in _nodeGo.transform)
                        {
                            child.GetComponent<SpriteRenderer>().color = Color.black;
                        }
                    });
                });
        });
        NextStep();
    }

    private void CanSpawnCubesAndActivateButtons()
    {
        canSpawnCubes = true;
        FindObjectOfType<PackageSpawner>().StartSpawning();
        MergeButton.gameObject.SetActive(true);
        MergeButton.interactable = false;
        AddCarButton.gameObject.SetActive(true);
        ClickSpeedButton.gameObject.SetActive(true);
        ClickSpeedButton.interactable = false;
        CallMethodWithDelay(() => ClickSpeedButton.interactable = true, 8);
        NextStep();
    }

    private void ActivateMergeButton()
    {
        AddCarButton.gameObject.SetActive(false);
        ClickSpeedButton.gameObject.SetActive(false);
        tutorialVignette.SetActive(true);
        MergeButton.interactable = true;
        MergeButton.transform.DOPunchScale(0.3f * Vector3.one, 0.5f, 0, 0.2f).SetLoops(8);
        FinishTutorial();
    }

    public void DisableTutorialVignette()
    {
        AddCarButton.gameObject.SetActive(true);
        ClickSpeedButton.gameObject.SetActive(true);

        if (tutorialVignette.activeInHierarchy)
            tutorialVignette.SetActive(false);
    }

    private void FinishTutorial()
    {
        UserPrefs.SetTutorial(false);
        UserPrefs.Save();
    }
}