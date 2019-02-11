using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonMono<T> : MonoBehaviour where T : class, new()
{
    private static volatile T instance = default(T);
    private static object obj = new object();

    public static T GetInstance()
    {
        lock (obj)
        {
            
            if (instance == null)
            {
                Debug.LogError(typeof(T) + "모노 싱글턴 사용 오류");
            }
            
            return instance;
        }
    }

    protected virtual void Awake()
    {
        lock (obj)
        {
            if (EqualityComparer<T>.Default.Equals(instance, default(T)))
            {
                Debug.Log(this.gameObject + "싱글턴 모노 이퀄 ");
                instance = this as T;
            }
            else if (!(EqualityComparer<T>.Default.Equals(instance, default(T))))
            {
                Debug.Log(this.gameObject + "싱글턴 제약으로 삭제");
                DestroyImmediate(this.gameObject);
                return;
            }

            /*

            if (instance == null)
            {
                instance = this as T;
            }
            
            else if (){
                //Destroy(this.gameObject);
                Debug.Log(this.gameObject);
            }
            */
            
        }
    }
}
