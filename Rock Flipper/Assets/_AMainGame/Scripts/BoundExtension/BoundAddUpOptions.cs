using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BT
{
    [System.Serializable]
    public struct BoundAddUpOptions
    {
        public BoundAddUpOption upOption;
        public BoundAddUpOption downOption;
        public BoundAddUpOption leftOption;
        public BoundAddUpOption rightOption;

        public static BoundAddUpOptions AllChooseMax => new BoundAddUpOptions()
        {
            downOption = BoundAddUpOption.ChooseMax,
            leftOption = BoundAddUpOption.ChooseMax,
            rightOption = BoundAddUpOption.ChooseMax,
            upOption = BoundAddUpOption.ChooseMax,
        };

        public static BoundAddUpOptions AllAddictive => new BoundAddUpOptions()
        {
            downOption = BoundAddUpOption.Addictive,
            leftOption = BoundAddUpOption.Addictive,
            rightOption = BoundAddUpOption.Addictive,
            upOption = BoundAddUpOption.Addictive,
        };
    }
}