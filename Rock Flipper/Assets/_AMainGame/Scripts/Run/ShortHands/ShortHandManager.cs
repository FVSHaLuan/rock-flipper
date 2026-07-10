using System.Collections.Generic;
using System;
using UnityEngine;
using System.Text.RegularExpressions;

namespace BT.Run
{
    public class ShortHandManager : ScriptableObjectWithInit
    {
        [System.NonSerialized]
        private Dictionary<string, Func<string>> shortHandDictionary;

        protected override void Init()
        {
            shortHandDictionary = new Dictionary<string, Func<string>>()
                                    {
                                        
                                    };

            ///
            base.Init();
        }

        public string Apply(string str)
        {
            ///
            TryInit();

            ///
            return str.ApplyParameters(GetDescriptionParameterValue).Replace('{', '[').Replace('}', ']');
        }

        private string GetDescriptionParameterValue(Match match)
        {
            ///
            var parameter = match.Value;

            ///
            Func<string> valueGetter = null;

            ///
            shortHandDictionary.TryGetValue(parameter, out valueGetter);

            ///
            if (valueGetter != null)
            {
                return valueGetter();
            }

            ///
            return string.Format("<INVALID_PARAM: {0}>", parameter);
        }
    }

}