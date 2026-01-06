using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Texture2D targetCursor, menuCursor;
    [SerializeField] private GameObject pauseScreen;
    [SerializeField] private GameObject gameScreen;
    [SerializeField] private GameObject statisticsScreen;
    [SerializeField] private GameObject deadScreen;

    // 1 - pistol, 2 - AK-47, 3 - Laser Blaster, 4 - Laser sniper, 5 - Lightsaber, 6 - Knife
    private int currentlySelectedWeapon=0;


    private void Start()
    {
        Cursor.SetCursor(targetCursor, Vector2.zero, CursorMode.ForceSoftware);
        gameScreen.SetActive(true);
    }



    private void Update()
    {
        
    }



    public void SwitchWeaponPicture(WeaponTypes weaponType)
    {
        
    }



    public void SetNewCatridgeNumber(int catridgeNum)
    {
        
    }



    public void SetNewBulletNumber(int bulletNum)
    {
        
    }



    public void StartCatridgeReloadAnimation(float time)
    {
        
    }



    public void StartFirstAidAnimation(float time)
    {
        
    }



    public void ChangeHPBar(int hpLeft)
    {
        
    }



    public void ChangeShieldBar(int shieldLeft)
    {
        
    }



    public void SetNumOfFirstAids(int numOfFirstAids)
    {
        
    }



    public void SetPoisonIcon(int poisonLevel)
    {
        
    }



    public void Pause()
    {
        
    }



    public void Continue()
    {
        
    }



    public void ExitToMainMenu()
    {
        
    }
}
