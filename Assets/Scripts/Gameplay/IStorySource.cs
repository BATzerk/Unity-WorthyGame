using UnityEngine;
using Ink.Runtime;

public interface IStorySource {
    Story MyStory { get; }
    //TextAsset MyStoryText { get; }
    void ResetMyStory();
    /// <summary>
    /// Executes a custom function, identified by the string.
    /// Returns TRUE if we DID recognize/do a function.
    /// </summary>
    /// <param name="funcName">Name of the function (WITHOUT "Func_").</param>
    bool DoFuncFromStory(string funcName);
    
    string FillInBlanks(string str);
}