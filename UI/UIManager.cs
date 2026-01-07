using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Texture2D targetCursor, menuCursor;
    [SerializeField] private GameObject pauseScreen;
    [SerializeField] private GameObject gameScreen;
    [SerializeField] private GameObject statisticsScreen;
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private GameObject deadScreen;
    [SerializeField] private Image weaponChoosedImage;
    [SerializeField] private Sprite HandsPic, AK47Pic, PistolPic, LaserBlasterPic, LaserSniperPic, LightsaberPic, KnifePic;
    [SerializeField] private ItemManager itemManager;
    [SerializeField] private TMP_Text catridgeText;
    [SerializeField] private TMP_Text bulletText;
    [SerializeField] private TMP_Text firstAidText;
    [SerializeField] private Image weaponReloadImage;
    [SerializeField] private Image firstAidReloadImage;
    [SerializeField] private Image healthBarImage;
    [SerializeField] private float healthBarPadding=0.08f;
    [SerializeField] private Image shieldBarImage;
    [SerializeField] private Sprite[] shieldLevels;
    [SerializeField] private GameObject beetlePoisonIcon;
    [SerializeField] private Image loadingFillBar;

    private Coroutine weaponReloading, firstAidReloading;

    // 1 - pistol, 2 - AK-47, 3 - Laser Blaster, 4 - Laser sniper, 5 - Lightsaber, 6 - Knife
    //private int currentlySelectedWeapon=0;


    private void Start()
    {
        Cursor.SetCursor(targetCursor, Vector2.zero, CursorMode.ForceSoftware);
        gameScreen.SetActive(true);
    }



    private void Update()
    {
        GameObject weapon = itemManager.getCurrentWeapon();

        if(weapon.TryGetComponent<FirearmWeapon>(out FirearmWeapon fWeapon))
        {
            switch(fWeapon.getWeaponType())
            {
                case WeaponTypes.AK47:
                    weaponChoosedImage.sprite = AK47Pic;
                    break;
                case WeaponTypes.LaserBlaster:
                    weaponChoosedImage.sprite = LaserBlasterPic;
                    break;
                case WeaponTypes.LaserSniper:
                    weaponChoosedImage.sprite = LaserSniperPic;
                    break;
                case WeaponTypes.Pistol:
                    weaponChoosedImage.sprite = PistolPic;
                    break;
            }
            
            Color color = weaponChoosedImage.color;
            if(fWeapon.getHaveThisWeapon())
                color.a = 1;
            else
                color.a = 0.4f;
            weaponChoosedImage.color = color;

            catridgeText.text = fWeapon.getCurrentNumOfCatridges().ToString();
            bulletText.text = fWeapon.getCurrentNumOfBullets().ToString();
        }
        else
        {
            MeeleWeapon mWeapon = weapon.GetComponent<MeeleWeapon>();

            switch(mWeapon.getWeaponType())
            {
                case WeaponTypes.Hands:
                    weaponChoosedImage.sprite = HandsPic;
                    break;
                case WeaponTypes.Lightsaber:
                    weaponChoosedImage.sprite = LightsaberPic;
                    break;
                case WeaponTypes.Knife:
                    weaponChoosedImage.sprite = KnifePic;
                    break;
            }
            
            Color color = weaponChoosedImage.color;
            if(mWeapon.getHaveThisWeapon())
                color.a = 1;
            else
                color.a = 0.4f;
            weaponChoosedImage.color = color;

            catridgeText.text = " ";
            bulletText.text = " ";
        }
    }



    public void StartCatridgeReloadAnimation(float time)
    {
        weaponReloading = StartCoroutine(CatridgeReloading(time));
    }



    private IEnumerator CatridgeReloading(float time)
    {
        float timePassed=0f;

        while(timePassed<time)
        {
            weaponReloadImage.fillAmount=timePassed/time;
            timePassed+=0.02f;
            yield return 0.02f;
        }
    }



    public void CancelCatridgeReloadAnimation()
    {
        StopCoroutine(weaponReloading);
        weaponReloadImage.fillAmount=0f;
    }



    public void StartFirstAidAnimation(float time)
    {
        firstAidReloading = StartCoroutine(FirstAidReloading(time));
    }



    private IEnumerator FirstAidReloading(float time)
    {
        float timePassed=0f;

        while(timePassed<time)
        {
            firstAidReloadImage.fillAmount=timePassed/time;
            timePassed+=0.02f;
            yield return 0.02f;
        }
    }



    public void CancelFirstAidAnimation()
    {
        StopCoroutine(firstAidReloading);
        firstAidReloadImage.fillAmount=0f;
    }



    public void ChangeHPBar(float hpLeft, float maxHP)
    {
        healthBarImage.fillAmount = Mathf.Lerp(healthBarPadding,1-healthBarPadding,hpLeft/maxHP);
    }



    public void ChangeShieldBar(int shieldLeft)
    {
        shieldBarImage.sprite = shieldLevels[shieldLeft];
    }



    public void SetNumOfFirstAids(int numOfFirstAids)
    {
        firstAidText.text = numOfFirstAids.ToString();
    }



    public void SetPoisonIcon(PoisonTypes poisonType)
    {
        switch(poisonType)
        {
            case PoisonTypes.Beetle:
                beetlePoisonIcon.SetActive(true);
                break;
        }
    }



    public void RemovePoisonIcon(PoisonTypes poisonType)
    {
        switch(poisonType)
        {
            case PoisonTypes.Beetle:
                beetlePoisonIcon.SetActive(false);
                break;
        }
    }



    public void RemoveAllPoisonIcons()
    {
        beetlePoisonIcon.SetActive(false);
    }



    public void Pause()
    {
        Time.timeScale = 0f;
        pauseScreen.SetActive(true);
        gameScreen.SetActive(false);
    }



    public void Continue()
    {
        Time.timeScale = 1f;
        pauseScreen.SetActive(false);
        gameScreen.SetActive(true);
    }



    public void ExitToMainMenu()
    {
        StartCoroutine(LoadSceneAsync("menu"));
    }


    private IEnumerator LoadSceneAsync(string sceneName)
    {

        loadingScreen.SetActive(true);

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);

        while(!operation.isDone)
        {
            float progressValue = Mathf.Clamp01(operation.progress/0.9f);

            loadingFillBar.fillAmount = progressValue;

            yield return null;
        }
    }



    public void SetDeadScreen()
    {
        gameScreen.SetActive(false);
        deadScreen.SetActive(true);
    }



    public void RemoveDeadScreen()
    {
        gameScreen.SetActive(true);
        deadScreen.SetActive(false);
    }
}
