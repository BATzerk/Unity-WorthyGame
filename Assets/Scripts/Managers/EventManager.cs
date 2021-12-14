using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EventManager {
	// Actions and Event Variables
	public delegate void NoParamAction ();
	public delegate void BoolAction (bool _bool);
	public delegate void StringAction (string _str);
    public delegate void IntAction (int _int);
    public delegate void StringBoolAction (string _str, bool _bool);

    // Common
    public event NoParamAction ScreenSizeChangedEvent;
    public void OnScreenSizeChanged () { ScreenSizeChangedEvent?.Invoke(); }
    
    
    // Gameplay
    public delegate void SeqAddressAction (SeqAddress addr);
    public event NoParamAction CharFinishedRevealingSpeechTextEvent;
    public event NoParamAction SortedUserPriosEvent;
    public event SeqAddressAction SetCurrSeqAddrEvent;
    
    public void OnCharFinishedRevealingSpeechText() { CharFinishedRevealingSpeechTextEvent?.Invoke(); }
    public void OnSetCurrSeqAddr(SeqAddress addr) { SetCurrSeqAddrEvent?.Invoke(addr); }
    public void OnSortedUserPrios() { SortedUserPriosEvent?.Invoke(); }
    
    
    
    



}




