using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    //Cursor textures
    [SerializeField] private Texture2D targetCursor, menuCursor;
    //References to ui screens
    [SerializeField] private GameObject pauseScreen;
    [SerializeField] private GameObject gameScreen;
    [SerializeField] private GameObject statisticsScreen;
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private GameObject deadScreen;
    //Image which shows currently selected weapon 
    [SerializeField] private Image weaponChoosedImage;
    //References to the sprites of all weapons
    [SerializeField] private Sprite HandsPic, AK47Pic, PistolPic, LaserBlasterPic, LaserSniperPic, LightsaberPic, KnifePic;
    [SerializeField] private ItemManager itemManager;
    //Text which says how many magazines player has
    [SerializeField] private TMP_Text catridgeText;
    //Text which says how many bullets player has
    [SerializeField] private TMP_Text bulletText;
    //Text which says how many first aids player has
    [SerializeField] private TMP_Text firstAidText;
    //Image which shows weapon reload progress
    [SerializeField] private Image weaponReloadImage;
    //Image which shows first aid usage progress
    [SerializeField] private Image firstAidReloadImage;
    //Shows how many health player has
    [SerializeField] private Image healthBarImage;
    //As a health bar is behind of the frame which has some thickness, when player
    //has 100% healt or very low health, healthbar may not show decrease in health
    //when player will take damage, so this offset is required to solve this issue
    [SerializeField] private float healthBarPadding=0.08f;
    //This image shows  current shield level
    [SerializeField] private Image shieldBarImage;
    //Array of all sprites for each shield level
    [SerializeField] private Sprite[] shieldLevels;
    [SerializeField] private GameObject beetlePoisonIcon;
    [SerializeField] private Image loadingFillBar;
    //Maximal diferrence between two float numbers to be considered as equal
    [SerializeField] private float epsilon=0.001f;

    //weaponReloading stores Catridge reloading timer
    //firstAidReloading stores firstAidReloading timer
    private Coroutine weaponReloading, firstAidReloading;



    //Initialize UI
    private void Start()
    {
        Cursor.SetCursor(targetCursor, Vector2.zero, CursorMode.ForceSoftware);
        gameScreen.SetActive(true);
        RemoveAllPoisonIcons();

        weaponReloadImage.fillAmount=0f;
        firstAidReloadImage.fillAmount=0f;
    }



    private void Update()
    {
        //Update weapon info every frame
        GameObject weapon = itemManager.getCurrentWeapon();

        //Check which type of weapon is this
        if(weapon.TryGetComponent<FirearmWeapon>(out FirearmWeapon fWeapon))
        {
            //Set correct picture
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
            
            //Check if player has this weapon or not
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

            //Set correct picture
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
            
            //Check if player has this weapon or not
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



    //Start magazine reloading animation
    public void StartCatridgeReloadAnimation(float time)
    {
        weaponReloading = StartCoroutine(CatridgeReloading(time));
    }



    //changes picture fill percentage to show the current progress of reloading
    private IEnumerator CatridgeReloading(float time)
    {
        float timePassed=0f;

        while(timePassed<time)
        {
            weaponReloadImage.fillAmount=timePassed/time;
            timePassed+=0.02f;
            yield return new WaitForSeconds(0.02f);
        }

        weaponReloadImage.fillAmount=0f;
        weaponReloading=null;
    }



    //Cancels magazine reload animation
    public void CancelCatridgeReloadAnimation()
    {
        if(weaponReloading!=null)
            StopCoroutine(weaponReloading);

        weaponReloading=null;
        weaponReloadImage.fillAmount=0f;
    }



    //Start first aid animation
    public void StartFirstAidAnimation(float time)
    {
        firstAidReloading = StartCoroutine(FirstAidReloading(time));
    }



    //changes picture fill percentage to show the current progress of using first aid
    private IEnumerator FirstAidReloading(float time)
    {
        float timePassed=0f;

        while(timePassed<time)
        {
            firstAidReloadImage.fillAmount=timePassed/time;
            timePassed+=0.02f;
            yield return new WaitForSeconds(0.02f);
        }

        firstAidReloadImage.fillAmount=0f;
        firstAidReloading=null;
    }



    //Cancel first aid animation
    public void CancelFirstAidAnimation()
    {
        if(firstAidReloading!=null)
            StopCoroutine(firstAidReloading);
        
        firstAidReloading=null;
        firstAidReloadImage.fillAmount=0f;
    }



    //Set new value to the health bar
    public void ChangeHPBar(float hpLeft, float maxHP)
    {
        healthBarImage.fillAmount = Mathf.Lerp(healthBarPadding,1-healthBarPadding,hpLeft/maxHP);
    }



    //Set new value to shield bar
    public void ChangeShieldBar(int shieldLeft)
    {
        shieldBarImage.sprite = shieldLevels[shieldLeft];
    }



    //Change text of how many first aid left
    public void SetNumOfFirstAids(int numOfFirstAids)
    {
        firstAidText.text = numOfFirstAids.ToString();
    }



    //Show poison icon, so player know what poison is acting on them now
    public void SetPoisonIcon(PoisonTypes poisonType)
    {
        switch(poisonType)
        {
            case PoisonTypes.Beetle:
                beetlePoisonIcon.SetActive(true);
                break;
        }
    }



    //Hide poison icon
    public void RemovePoisonIcon(PoisonTypes poisonType)
    {
        switch(poisonType)
        {
            case PoisonTypes.Beetle:
                beetlePoisonIcon.SetActive(false);
                break;
        }
    }



    //Hides all poison icons
    public void RemoveAllPoisonIcons()
    {
        beetlePoisonIcon.SetActive(false);
    }



    //Pauses the game
    public void Pause()
    {
        Cursor.SetCursor(menuCursor, Vector2.zero, CursorMode.ForceSoftware);
        Time.timeScale = 0f;
        pauseScreen.SetActive(true);
        gameScreen.SetActive(false);
    }



    //Resumes the game
    public void Continue()
    {
        Cursor.SetCursor(targetCursor, Vector2.zero, CursorMode.ForceSoftware);
        Time.timeScale = 1f;
        pauseScreen.SetActive(false);
        gameScreen.SetActive(true);
    }



    //Starts loading screen
    public void ExitToMainMenu()
    {
        StartCoroutine(LoadSceneAsync("menu"));
    }



    //Loading screen shows the progress of loading new scene
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



    //Show dead screen
    public void SetDeadScreen()
    {
        Cursor.SetCursor(menuCursor, Vector2.zero, CursorMode.ForceSoftware);
        gameScreen.SetActive(false);
        deadScreen.SetActive(true);
        Time.timeScale = 0f;
    }



    //Hide dead screen
    public void RemoveDeadScreen()
    {
        Cursor.SetCursor(targetCursor, Vector2.zero, CursorMode.ForceSoftware);
        gameScreen.SetActive(true);
        deadScreen.SetActive(false);

        if(Time.timeScale<epsilon)
            Time.timeScale = 1f;
    }
}
