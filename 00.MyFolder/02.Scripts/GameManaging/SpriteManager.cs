using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteManager : SingletonMono<SpriteManager>{

    public Sprite[] charactorSprites;
    public Sprite[] coloredMobEviSprites;
    public Sprite[] mobStampedSprites;
    public Sprite[] evidenceStampedSprites;
    public Sprite[] numSprites;
    public Sprite[] weaponSprites;
    public Sprite[] upgradeSprites;
    public Sprite[] interiorSprites;
    public Sprite[] paperSprites;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this.gameObject);
    }


    public Sprite GetCharactorSprite(int charaSpriteIdx)
    {
        return charactorSprites[charaSpriteIdx];
    }

    public Sprite GetColoredEvidenceSprite(E_Monster mob, E_Evidence evi)
    {
        int idx = (((int)E_Evidence.MAX) * ((int)mob)) + (int)evi; 
        return coloredMobEviSprites[idx];
    }
    public Sprite GetMobStampedSprite(E_Monster mob)  
    {
        return mobStampedSprites[(int)mob];
    }
    
    
    public Sprite GetEvidenceStampedSprite(E_Evidence evi) 
    {
        return evidenceStampedSprites[(int)evi];
    }

    public Sprite GetNumSprite(int num)
    {
        if (num > 9 || num <0) return null;
        return numSprites[num];
    }

    public Sprite GetWeaponSprite(E_Weapon weapon)
    {
        return weaponSprites[(int)weapon];
    }
    public Sprite GetUpgradeSprite(E_Upgrade upgrade)
    {
        return upgradeSprites[(int)upgrade];
    }
    public Sprite GetInteriorSprite(E_Interior interior)
    {
        return interiorSprites[(int)interior];
    }
    public Sprite GetPaperSprite(E_Paper paper)
    {
        return paperSprites[(int)paper];
    }
}
