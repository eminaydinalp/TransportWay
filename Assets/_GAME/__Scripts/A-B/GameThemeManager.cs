using System;
using System.Collections;
using System.Collections.Generic;
using _GAME.__Scripts.Home;
using _GAME.__Scripts.TargetHome;
using Rentire.Core;
using Sirenix.OdinInspector;
using UnityEngine;

public class GameThemeManager : Singleton<GameThemeManager>
{
    [SerializeField] [EnumToggleButtons] private GameThemeMode currentTheme;

    [Header("Zemin Materyalleri")] [SerializeField]
    private Material darkGroundMaterial;

    [SerializeField] private Material lightGroundMaterial;
    [SerializeField] private Material avciGroundMaterial;


    [Header("Ev Outline Sabit Rengi")] [SerializeField]
    private Color homeOutlineStaticColor;
    [SerializeField] private Color homeOutlineStaticColorIn;

    #region Ev

    [Space(10)] [Header("Mor Ev")] [SerializeField]
    private Color homeOutlineOwnPurple;

    [SerializeField] private Material roadMaterialPurpleDark;
    [SerializeField] private Material roadMaterialPurpleLight;

    [Space(5)] [Header("Mavi Ev")] [SerializeField]
    private Color homeOutlineOwnBlue;

    [SerializeField] private Material roadMaterialBlueDark;
    [SerializeField] private Material roadMaterialBlueLight;

    [Space(5)] [Header("Yeşil Ev")] [SerializeField]
    private Color homeOutlineOwnGreen;

    [SerializeField] private Material roadMaterialGreenDark;
    [SerializeField] private Material roadMaterialGreenLight;

    [Space(5)] [Header("Turuncu Ev")] [SerializeField]
    private Color homeOutlineOwnOrange;

    [SerializeField] private Material roadMaterialOrangeDark;
    [SerializeField] private Material roadMaterialOrangeLight;

    [Space(5)] [Header("Pembe Ev")] [SerializeField]
    private Color homeOutlineOwnPink;

    [SerializeField] private Material roadMaterialPinkDark;
    [SerializeField] private Material roadMaterialPinkLight;

    [Space(5)] [Header("Kırmızı Ev")] [SerializeField]
    private Color homeOutlineOwnRed;

    [SerializeField] private Material roadMaterialRedDark;
    [SerializeField] private Material roadMaterialRedLight;

    [Space(5)] [Header("Sarı Ev")] [SerializeField]
    private Color homeOutlineOwnYellow;

    [SerializeField] private Material roadMaterialYellowDark;
    [SerializeField] private Material roadMaterialYellowLight;

    [Space(5)] [Header("Açık Mavi Ev")] [SerializeField]
    private Color homeOutlineOwnLightBlue;

    [SerializeField] private Material roadMaterialLightBlueDark;
    [SerializeField] private Material roadMaterialLightBlueLight;

    #endregion

    [Header("Dark Yol Materyalleri")] [SerializeField]
    private Material darkFirstMaterial;

    [SerializeField] private Material darkSecondMaterial;

    public void SetTheme()
    {
        SetColorful();
        SetDark();
        SetAvci();
    }

    private void SetColorful()
    {
        if (currentTheme != GameThemeMode.Light)
            return;

        var ground = FindObjectOfType<GroundReference>().gameObject;
        var allHomes = FindObjectsOfType<HomeController>();
        var allTargetHomes = FindObjectsOfType<TargetHomeController>();
        foreach (var home in allHomes)
        {
            TruckColor homeColor = home.truckColor;
            switch (homeColor)
            {
                case TruckColor.Purple:
                    home.outlineSprite.color = homeOutlineOwnPurple;

                    var purpleMats = home.splineMeshRenderer.materials;
                    purpleMats[0] = roadMaterialPurpleDark;
                    purpleMats[2] = roadMaterialPurpleLight;
                    home.splineMeshRenderer.materials = purpleMats;
                    break;
                case TruckColor.Blue:
                    home.outlineSprite.color = homeOutlineOwnBlue;

                    var blueMats = home.splineMeshRenderer.materials;
                    blueMats[0] = roadMaterialBlueDark;
                    blueMats[2] = roadMaterialBlueLight;
                    home.splineMeshRenderer.materials = blueMats;
                    break;
                case TruckColor.Green:
                    home.outlineSprite.color = homeOutlineOwnGreen;

                    var greenMats = home.splineMeshRenderer.materials;
                    greenMats[0] = roadMaterialGreenDark;
                    greenMats[2] = roadMaterialGreenLight;
                    home.splineMeshRenderer.materials = greenMats;
                    break;
                case TruckColor.Orange:
                    home.outlineSprite.color = homeOutlineOwnOrange;

                    var orangeMats = home.splineMeshRenderer.materials;
                    orangeMats[0] = roadMaterialOrangeDark;
                    orangeMats[2] = roadMaterialOrangeLight;
                    home.splineMeshRenderer.materials = orangeMats;
                    break;
                case TruckColor.Pink:
                    home.outlineSprite.color = homeOutlineOwnPink;
                    var pinkMats = home.splineMeshRenderer.materials;
                    pinkMats[0] = roadMaterialPinkDark;
                    pinkMats[2] = roadMaterialPinkLight;
                    home.splineMeshRenderer.materials = pinkMats;
                    break;
                case TruckColor.Red:
                    home.outlineSprite.color = homeOutlineOwnRed;
                    var redMats = home.splineMeshRenderer.materials;
                    redMats[0] = roadMaterialRedDark;
                    redMats[2] = roadMaterialRedLight;
                    home.splineMeshRenderer.materials = redMats;
                    break;
                case TruckColor.Yellow:
                    home.outlineSprite.color = homeOutlineOwnYellow;

                    var yellowMats = home.splineMeshRenderer.materials;
                    yellowMats[0] = roadMaterialYellowDark;
                    yellowMats[2] = roadMaterialYellowLight;
                    home.splineMeshRenderer.materials = yellowMats;
                    break;
                case TruckColor.BlueLight:
                    home.outlineSprite.color = homeOutlineOwnLightBlue;

                    var lightBlueMats = home.splineMeshRenderer.materials;
                    lightBlueMats[0] = roadMaterialLightBlueDark;
                    lightBlueMats[2] = roadMaterialLightBlueLight;
                    home.splineMeshRenderer.materials = lightBlueMats;
                    break;
            }
        }

        foreach (var target in allTargetHomes)
        {
            TruckColor homeColor = target.truckColor;
            switch (homeColor)
            {
                case TruckColor.Purple:
                    target.outlineSprite.color = homeOutlineOwnPurple;
                    break;
                case TruckColor.Blue:
                    target.outlineSprite.color = homeOutlineOwnBlue;
                    break;
                case TruckColor.Green:
                    target.outlineSprite.color = homeOutlineOwnGreen;
                    break;
                case TruckColor.Orange:
                    target.outlineSprite.color = homeOutlineOwnOrange;
                    break;
                case TruckColor.Pink:
                    target.outlineSprite.color = homeOutlineOwnPink;
                    break;
                case TruckColor.Red:
                    target.outlineSprite.color = homeOutlineOwnRed;
                    break;
                case TruckColor.Yellow:
                    target.outlineSprite.color = homeOutlineOwnYellow;
                    break;
                case TruckColor.BlueLight:
                    target.outlineSprite.color = homeOutlineOwnLightBlue;
                    break;
            }
        }

        ground.GetComponent<MeshRenderer>().material = lightGroundMaterial;
    }
    
     private void SetAvci()
    {
        if (currentTheme != GameThemeMode.Avci)
            return;

        var ground = FindObjectOfType<GroundReference>().gameObject;
        var allHomes = FindObjectsOfType<HomeController>();
        var allTargetHomes = FindObjectsOfType<TargetHomeController>();
        foreach (var home in allHomes)
        {
            TruckColor homeColor = home.truckColor;
            switch (homeColor)
            {
                case TruckColor.Purple:
                    home.outlineSprite.color = homeOutlineOwnPurple;

                    var purpleMats = home.splineMeshRenderer.materials;
                    purpleMats[0] = roadMaterialPurpleDark;
                    purpleMats[2] = roadMaterialPurpleLight;
                    home.splineMeshRenderer.materials = purpleMats;
                    break;
                case TruckColor.Blue:
                    home.outlineSprite.color = homeOutlineOwnBlue;

                    var blueMats = home.splineMeshRenderer.materials;
                    blueMats[0] = roadMaterialBlueDark;
                    blueMats[2] = roadMaterialBlueLight;
                    home.splineMeshRenderer.materials = blueMats;
                    break;
                case TruckColor.Green:
                    home.outlineSprite.color = homeOutlineOwnGreen;

                    var greenMats = home.splineMeshRenderer.materials;
                    greenMats[0] = roadMaterialGreenDark;
                    greenMats[2] = roadMaterialGreenLight;
                    home.splineMeshRenderer.materials = greenMats;
                    break;
                case TruckColor.Orange:
                    home.outlineSprite.color = homeOutlineOwnOrange;

                    var orangeMats = home.splineMeshRenderer.materials;
                    orangeMats[0] = roadMaterialOrangeDark;
                    orangeMats[2] = roadMaterialOrangeLight;
                    home.splineMeshRenderer.materials = orangeMats;
                    break;
                case TruckColor.Pink:
                    home.outlineSprite.color = homeOutlineOwnPink;
                    var pinkMats = home.splineMeshRenderer.materials;
                    pinkMats[0] = roadMaterialPinkDark;
                    pinkMats[2] = roadMaterialPinkLight;
                    home.splineMeshRenderer.materials = pinkMats;
                    break;
                case TruckColor.Red:
                    home.outlineSprite.color = homeOutlineOwnRed;
                    var redMats = home.splineMeshRenderer.materials;
                    redMats[0] = roadMaterialRedDark;
                    redMats[2] = roadMaterialRedLight;
                    home.splineMeshRenderer.materials = redMats;
                    break;
                case TruckColor.Yellow:
                    home.outlineSprite.color = homeOutlineOwnYellow;

                    var yellowMats = home.splineMeshRenderer.materials;
                    yellowMats[0] = roadMaterialYellowDark;
                    yellowMats[2] = roadMaterialYellowLight;
                    home.splineMeshRenderer.materials = yellowMats;
                    break;
                case TruckColor.BlueLight:
                    home.outlineSprite.color = homeOutlineOwnLightBlue;

                    var lightBlueMats = home.splineMeshRenderer.materials;
                    lightBlueMats[0] = roadMaterialLightBlueDark;
                    lightBlueMats[2] = roadMaterialLightBlueLight;
                    home.splineMeshRenderer.materials = lightBlueMats;
                    break;
            }
        }

        foreach (var target in allTargetHomes)
        {
            TruckColor homeColor = target.truckColor;
            switch (homeColor)
            {
                case TruckColor.Purple:
                    target.outlineSprite.color = homeOutlineOwnPurple;
                    break;
                case TruckColor.Blue:
                    target.outlineSprite.color = homeOutlineOwnBlue;
                    break;
                case TruckColor.Green:
                    target.outlineSprite.color = homeOutlineOwnGreen;
                    break;
                case TruckColor.Orange:
                    target.outlineSprite.color = homeOutlineOwnOrange;
                    break;
                case TruckColor.Pink:
                    target.outlineSprite.color = homeOutlineOwnPink;
                    break;
                case TruckColor.Red:
                    target.outlineSprite.color = homeOutlineOwnRed;
                    break;
                case TruckColor.Yellow:
                    target.outlineSprite.color = homeOutlineOwnYellow;
                    break;
                case TruckColor.BlueLight:
                    target.outlineSprite.color = homeOutlineOwnLightBlue;
                    break;
            }
        }

        ground.GetComponent<MeshRenderer>().material = avciGroundMaterial;
    }

    private void Start()
    {
        #if !UNITY_EDITOR
        var remoteTheme = RemoteManager.Instance.GetGameTheme();
        switch (remoteTheme)
        {
            case 0:
                currentTheme = GameThemeMode.Dark;
                break;
            case 1:
                currentTheme = GameThemeMode.Light;
                break;
            case 2:
                currentTheme = GameThemeMode.Avci;
                break;

        }
#endif
        CallMethodWithDelay(SetTheme, 0.02f);
    }

    private void SetDark()
    {
        if (currentTheme != GameThemeMode.Dark)
            return;

        var ground = FindObjectOfType<GroundReference>().gameObject;
        var allHomes = FindObjectsOfType<HomeController>();
        var allTargetHomes = FindObjectsOfType<TargetHomeController>();

        foreach (var home in allHomes)
        {
            home.outlineSprite.color = homeOutlineStaticColor;
            home.innerSprite.color = homeOutlineStaticColorIn;
            var roadMats = home.splineMeshRenderer.materials;
            roadMats[0] = darkSecondMaterial;
            roadMats[2] = darkFirstMaterial;
            home.splineMeshRenderer.materials = roadMats;
        }

        foreach (var target in allTargetHomes)
        {
            target.outlineSprite.color = homeOutlineStaticColor;
            target.innerSprite.color = homeOutlineStaticColorIn;
        }

        ground.GetComponent<MeshRenderer>().material = darkGroundMaterial;
    }
}

public enum GameThemeMode
{
    Light,
    Dark,
    Avci
    //Mixed
}