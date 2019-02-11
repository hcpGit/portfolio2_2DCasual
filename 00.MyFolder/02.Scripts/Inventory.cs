using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Inventory : Singleton<Inventory> {

    #region -MobEviInven
    public static string GetMobEviKey(QuestPerMob qpm)
    {
        return ((int)qpm.mob).ToString() + "/" + ((int)qpm.evidence).ToString();
    }
    public static string GetMobEviKey(E_Monster mob , E_Evidence evi)
    {
        return ((int)mob).ToString() + "/" + ((int)evi).ToString();
    }

    public static QuestPerMob MakeQPMByInvenKey(string key, int number)
    {
        string[] splitted = key.Split('/');
        if (splitted.Length != 2) Debug.LogError("키 qpm 변환 실패");

        E_Monster mob = E_Monster.GOBLIN;
        E_Evidence evidence = E_Evidence.FINGER;

        switch (splitted[0])
        {
            case "0":
                mob = (E_Monster)0;
                break;
            case "1":
                mob = (E_Monster)1;
                break;
            case "2":
                mob = (E_Monster)2;
                break;
            case "3":
                mob = (E_Monster)3;
                break;
            case "4":
                mob = (E_Monster)4;
                break;
            case "5":
                mob = (E_Monster)5;
                break;
            case "6":
                mob = (E_Monster)6;
                break;
            case "7":
                mob = (E_Monster)7;
                break;

            default:
                Debug.LogError("키 오류");
                break;
        }
        switch (splitted[1])
        {
            case "0":
                evidence = (E_Evidence)0;
                break;
            case "1":
                evidence = (E_Evidence)1;
                break;
            case "2":
                evidence = (E_Evidence)2;
                break;
            case "3":
                evidence = (E_Evidence)3;
                break;
                
            default:
                Debug.LogError("키 오류");
                break;
        }
        QuestPerMob qpm = new QuestPerMob(mob, evidence, number);
        return qpm;
    }

    Dictionary<string, int> mobEvidenceInven = new Dictionary<string, int>();
    public Dictionary<string, int> MobEvidenceInven
    {
        get {
            return mobEvidenceInven;
        }
    }

    public List<QuestPerMob> GetNowMobEvidencesInven()
    {
        List<QuestPerMob> temp = new List<QuestPerMob>();
        Dictionary<string, int>.Enumerator enu = mobEvidenceInven.GetEnumerator();
        while (enu.MoveNext())
        {
            QuestPerMob qpm;
            qpm = Inventory.MakeQPMByInvenKey(enu.Current.Key, enu.Current.Value);
            if (qpm != null)
                temp.Add(qpm);
        }
        return temp;
    }

    public void RenewMobEviInven(Dictionary<string, int> newInven)
    {
        mobEvidenceInven = null;
        mobEvidenceInven = newInven;
    }

    public void AdjustMobEviItemNumber(QuestPerMob qpm, int number) //아예 이 넘버로 수정하라는 얘기
                                                                    //이 넘버 만큼 빼는 함수가 아님!!
                                                                    //그런데 있던 애들 중에서만 불려와줘야함
                                                                    //추가가 절대 아님!!!!
    {
        if (number <= 0)
        {
            mobEvidenceInven.Remove(Inventory.GetMobEviKey(qpm));
            return;
        }
        mobEvidenceInven[Inventory.GetMobEviKey(qpm)] = number;
    }

    public void AddMobEvi(QuestPerMob qpm)
    {
        string key = Inventory.GetMobEviKey(qpm);
        if (mobEvidenceInven.ContainsKey(key))
        {
            mobEvidenceInven[key] += qpm.number;
            return;
        }
        mobEvidenceInven.Add(key, qpm.number);
    }
    public void AddMobEviByList(List<QuestPerMob> addList)
    {
        for (int i = 0; i < addList.Count; i++)
        {
            AddMobEvi(addList[i]);
        }
    }

    public void MinusMobEviItem(List<QuestPerMob> minusList)
    {
        for (int i = 0; i < minusList.Count; i++)
        {
            string key = Inventory.GetMobEviKey(minusList[i]);
            int minusNumber = minusList[i].number;

            if (!mobEvidenceInven.ContainsKey(key))
            {
                Debug.LogError("없는 애를 뺄려고 함.");
                return;
            }
            int nowHaveNum = mobEvidenceInven[key];


            if (minusNumber >= nowHaveNum)//뺴는 수가 초과하니까 아예 삭제.
            {
                mobEvidenceInven.Remove(key);
                continue;
            }

            mobEvidenceInven[key] = mobEvidenceInven[key] - minusNumber;
            //Debug.LogFormat("현재 갖고 있던 수는 = {0} , 뺴려고 했던 수는 = {1}", nowHaveNum, minusNumber);
        }
    }


    #endregion

    #region -Weapon


    public class weaponInven
    {
        public int number;
        public Weapon weapon;
        public weaponInven(Weapon weapon, int num) { this.weapon = weapon; this.number = num; }
    }

    List<weaponInven> weaponInventory = new List<weaponInven>();
    public List<weaponInven> WeaponInventory
    {
        get {
            return weaponInventory;
        }
    }
        

    public List<Weapon> GetWeaponInvenTypes()//현재 인벤에서 하나씩만 카피해서 돌려줌 
        //무기 대여 시 리스트 보여주는 용도라고 보면 됨.
    {
        List<Weapon> temp = new List<Weapon>();
        foreach (weaponInven wi in weaponInventory)
        {
            temp.Add(wi.weapon);
        }
        return temp;
    }

    public void RemoveWeaponsFromInven(List<Weapon> removed)
    {
        foreach (Weapon weapon in removed)
        {
            foreach (weaponInven wi in weaponInventory)
            {
                if (wi.weapon == weapon)
                {
                    wi.number--;
                    continue; 
                }
            }
        }
        int idx = 0;

        while (idx < weaponInventory.Count)
        {
            if (weaponInventory[idx].number <= 0)
            {
                weaponInventory.RemoveAt(idx);
                continue;
            }
            idx++;
        }
    }
    public void AddWeaponsToInven(List<Weapon> newOne)
    {
        bool matched=false ;
        for (int i = 0; i < newOne.Count; i++)
        {
            matched = false;
            E_Weapon newWeapon = newOne[i].weaponType;

            for (int j = 0; j < weaponInventory.Count; j++)
            {
                E_Weapon has = weaponInventory[j].weapon.weaponType;
                if (has == newWeapon)
                {
                    weaponInventory[j].number++;
                    matched = true;
                    break;
                }
            }
            if (!matched)
            {
                weaponInventory.Add(new weaponInven(newOne[i], 1));
            }
        }
        
    }

    public Dictionary<E_Weapon, int> GetWeaponInvenSave()
    {
        Dictionary<E_Weapon, int> temp = new Dictionary<E_Weapon, int>();
        for (int i = 0; i < weaponInventory.Count; i++)
        {
            temp.Add(weaponInventory[i].weapon.weaponType, weaponInventory[i].number);
        }
        return temp;
    }

    #endregion

    public void Load(SaveStructure st)
    {
        RenewMobEviInven(st.inventoryMobEvi);

        Dictionary<E_Weapon, int> weaponTemp = st.inventoryWeapon;
        weaponInventory = new List<weaponInven>();
        Dictionary<E_Weapon, int>.Enumerator enu = weaponTemp.GetEnumerator();
        while (enu.MoveNext())
        {
            weaponInventory.Add(
                new weaponInven(
                WeaponInfoManager.GetInstance().CreateWeapon(enu.Current.Key),
                enu.Current.Value
                ));
        }
    }
}
