using System.Collections.Generic;
using System;
using UnityEngine;
using System.Text.RegularExpressions;

namespace Agame.Run
{
    public class ShortHandManager : ScriptableObjectWithInit
    {
        [System.NonSerialized]
        private Dictionary<string, Func<string>> shortHandDictionary;

        protected override void Init()
        {
            shortHandDictionary = new Dictionary<string, Func<string>>()
                                    {
                { "NEWLINE", () => Environment.NewLine }
                                    };

            ///
            base.Init();
        }

        public string Apply(string str)
        {
            ///
            TryInit();

            ///
            return str.ApplyParameters(GetDescriptionParameterValue);//.Replace('{', '[').Replace('}', ']');
        }

        private string GetDescriptionParameterValue(Match match)
        {
            ///
            var parameter = match.Value;

            ///
            if (string.IsNullOrWhiteSpace(parameter))
            {
                return parameter;
            }

            ///
            Func<string> valueGetter = null;

            ///
            shortHandDictionary.TryGetValue(parameter, out valueGetter);

            ///
            if (parameter[0] == '@')
            {
                ///
                return parameter.Substring(1);
            }

            ///
            if (valueGetter != null)
            {
                return $"<{valueGetter()}>";
            }

            ///
#if UNITY_EDITOR
            var errorMessage = string.Format("<<INVALID_PARAM: {0}>>", parameter);
            Debug.LogError($"{errorMessage}, please add '@' after the '[' to ignore ShortHand");
            return errorMessage;
#else
            return parameter;
#endif
        }
    }

}