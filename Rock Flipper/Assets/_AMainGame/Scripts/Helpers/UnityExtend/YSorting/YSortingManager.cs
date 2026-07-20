using UnityEngine;

public class YSortingManager : MonoBehaviourWithInit
{
    private static YSortingManager instance;
    public static YSortingManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindAnyObjectByType<YSortingManager>();
                if (instance == null)
                {
                    GameObject obj = new GameObject("YSortingManager");
                    instance = obj.AddComponent<YSortingManager>();
                }
            }

            ///
            return instance;
        }
    }

    protected override void ExtendedAwake()
    {
        ///
        base.ExtendedAwake();

        ///
        instance = this;
    }
}
