#if !DISABLESTEAMWORKS
using Steamworks; 
#endif
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UnifiedText))]
public class SteamIdHashDisplayer : MonoBehaviour
{
#if !DISABLESTEAMWORKS
    private static string steamIdHash;

    private static void TryGetHash()
    {
        if (!string.IsNullOrEmpty(steamIdHash))
        {
            return;
        }

        ///
        if (!SteamManager.Initialized)
        {
            return;
        }

        ///
        try
        {
            var steamId = SteamUser.GetSteamID().m_SteamID.ToString();

            ///
            steamIdHash = (steamId + Application.version).HashWithSHA1();

        }
        catch (System.Exception)
        {

            // throw;
        }
    }

    protected void OnEnable()
    {
        TryGetHash();

        ///
        GetComponent<UnifiedText>().Text = steamIdHash;
    } 
#endif
}
