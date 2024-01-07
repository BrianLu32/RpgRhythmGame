using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SynchronizerData;

public class BeatObserver : MonoBehaviour
{
   [Range(0, 500)]
	public float beatWindow = 10f;	// in milliseconds
	
	[HideInInspector]
	public BeatType beatMask;

    void Start()
    {
        beatMask = BeatType.None;
    }

    /// <summary>
	/// This method is called by each BeatCounter this object is observing.
	/// </summary>
	/// <param name="beatType">The beat type that invoked this method.</param>
	public void BeatNotify (BeatType beatType)
	{
		beatMask |= beatType;
		StartCoroutine(WaitOnBeat(beatType));
	}

	/// <summary>
	/// This overloaded method is called by each PatternCounter this object is observing. Since pattern counters contain a sequence of 
	/// different beat types, keeping track of the beat type isn't necessary. To test for a beat from the pattern counter, the beat mask
	/// should be checked for the BeatType.OnBeat flag.
	/// </summary>
	public void BeatNotify ()
	{
		beatMask |= BeatType.OnBeat;
		StartCoroutine(WaitOnBeat(BeatType.OnBeat));
	}

	/// <summary>
	/// Clears the bit corresponding to the beat type after a specified duration of time.
	/// </summary>
	/// <param name="beatType">The beat type to clear.</param>
	IEnumerator WaitOnBeat (BeatType beatType)
	{
		yield return new WaitForSeconds(beatWindow / 1000f);
		beatMask ^= beatType;
	}
}
