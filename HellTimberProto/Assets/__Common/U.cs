using UnityEngine;

/// <summary>
/// Every UtilsFunctions with shorter name
/// </summary>
public class U : UtilsFunctions
{
    /// <summary>
    /// Short for 'UtilsFunction.Log'. Debug.Log with as many parameters as you like
    /// </summary>
    /// <param name="logs">Multiple parameters (separated by a comma)</param>
    public static void L(params object[] logs) => Log(logs);

    /// <summary>
    /// Short for UtilsFunction.LogObject Debug.Log with as many parameters as you like
    /// </summary>
    /// <param name="context"> Context of the Debug.Log</param>
    /// <param name="logs">Multiple parameters (separated by a comma)</param>
    public static void LO(Object context, params object[] logs) => LogObject(context, logs);
}
