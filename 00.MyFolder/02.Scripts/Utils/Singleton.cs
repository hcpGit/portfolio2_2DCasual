using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> where T : class , new(){
    private static volatile T instance;
    private static object obj = new object();

    public static T GetInstance()
    {
        lock (obj)
        {
            if (instance == null)
            {
                instance = new T();

            }
            return instance;
        }
    }
}
