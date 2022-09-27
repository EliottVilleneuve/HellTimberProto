using UnityEngine;

public class UtilsFunctions
{
    /// <summary>
    /// Debug.Log with as many parameters as you like
    /// </summary>
    /// <param name="logs">Multiple parameters (separated by a comma)</param>
    public static void Log(params object[] logs)
    {
        Debug.Log(ObjectsToSingleString(logs));
    }

    /// <summary>
    /// Debug.Log with as many parameters as you like
    /// </summary>
    /// <param name="context"> Context of the Debug.Log</param>
    /// <param name="logs">Multiple parameters (separated by a comma)</param>
    public static void LogObject(Object context, params object[] logs)
    {
        Debug.Log(ObjectsToSingleString(logs), context);
    }

    public static bool R(Vector3 origin, Vector3 direction, out RaycastHit hit, float maxDistance = Mathf.Infinity, int layerMask = Physics.AllLayers)
    {
        return Physics.Raycast(origin, direction, out hit, maxDistance, layerMask);
    }

    public static bool RD(Vector3 origin, Vector3 direction, out RaycastHit hit, Color debugColor, float maxDistance = Mathf.Infinity, int layerMask = Physics.AllLayers)
    {
        Debug.DrawRay(origin, direction.normalized * maxDistance, debugColor);
        return Physics.Raycast(origin, direction, out hit, maxDistance, layerMask);
    }

    #region PRIVATE METHODS

    private static string ObjectsToSingleString(params object[] logs)
    {
        if (logs.Length < 1) return "";

        string log = "";

        for (int i = 0; i < logs.Length; i++)
        {
            string fetchedString = logs[i].ToString();

            if (fetchedString == "") continue;

            log += (i == 0 ? "" : " ") + fetchedString;
        }

        if (log == "") Debug.LogWarning("Log unimplemented");

        return log;
    }

    #endregion
}
