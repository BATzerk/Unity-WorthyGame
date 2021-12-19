
// VAR BrattTaylor = "<color=\#6B513B>Bratt Taylor</color>"
VAR wcS = "<color=\#6B513B>" // worthyColorStart
VAR wcE = "</color>" // worthyColorEnd

- -> GameIntro


=== GameIntro ===
- N: Hi, [UserName]! My name is Waddlesworth Worthington.
- N: But you can call me Jill.
* [Hi, Jill.]
- N: Hi!\\n\\nI designed this little game to make you {wcS}MORE WORTHY{wcE}!
- N: We could all use a little more {wcS}worthiness{wcE}, you know?
* [Makes sense.]
* [Whatever, <i>Jill</i>.]
    -- N: That's my name, [UserName]!
- N: Each MINIGAME is designed to make you {wcS}MORE WORTHY{wcE}.
* [Worthy... of what?]
    -- N: Love, belonging, friendship, nice things.\\nAnd more!
    -- N: It’s gonna be sweet.
* [I’m ready.]
    -- N: You're so hot right now!

- N: You’re actually the first person to play my minigames here.\\n\\nI’m excited to see how this goes!
- N: Here is your {wcS}WorthyMeter{wcE}!
- FuncContinue_ShowWorthyMeter
- ShowTapToContinue(1.2)
- N: Try to get this to 100%.
- N: Your goal is to become {wcS}100% worthy{wcE}.
- N: Are you ready?
* [Yes.]
* [No, I have to pee first.]
    -- N: Okay! Let’s both take a quick bathroom break, then. Tap next when you’re back from the bathroom.

--> END


=== PostJokeTeller ===
- N: Ha ha! Those were some funny jokes!
- N: But it doesn't look as if telling jokes changed your deservingness of good friendships.
- N: Maybe the WorthyMeter is broken? Can we check?
- N: Try saying something dumb to bring down your worthiness a little bit.
* [i'm a astronot]
* [I need a vacation... from MY VACATION!!!]
* [soup's up! no wait I mean surf's up oh no]
* [bimgo bamgo bonjo gingoo]
* [I need to put my hands in your pockets. It's an emergency.]
- N: Hmm.
- N: I mean, that was a really dumb thing to say.
- N: ...But apparently you're still 100% worthy of good friendships.
- N: ...
- N: I have an idea! Go pick a friend up from the airport!\\nThat'll DEFINITELY make you more worthy.
- -> END







// - N: Hi, [UserName]! I'm Untitled Circle.
// - N: I know what you're thinking.
// * [Is that your birthname?]
//     -- Yes.
// * [Circles cannot ask questions.]
//     -- While that's correct, we can still make statements!
// * [What? You don't know me.]
//     -- But I'm looking forward to!

// = ExplainWhat
// - AutoAdvanceAfterSpeech
// - N: Well! We're gonna play minigames...
// - FuncContinue_ShowFinalRankSample
// - ShowTapToContinue(1.2)
// - AutoAdvanceAfterSpeech
// - N: Then, by the end, we'll have a <color=\#1F71DE>ranked list</color> of your life priorities today!
// - FuncContinue_RevealMoreFinalRankSample
// - ShowTapToContinue(2)
// - N: Hopefully these results will give you something to think about!
// - FuncContinue_HideFinalRankSample
// * [Why priorities?]
//     -- ->ExplainWhy
// * [NEXT]
//     -- ->END

// = ExplainWhy
// - N: The more clarity about what matters to you, the easier it is to make decisions when your desires conflict.
// * [Got it.]
//     -- N: Awesome.
// * [What if they change?]
//     -- N: Don't worry, [UserName].\\n\\nClarity for today will be enough for today.
// - 
// * {numQsAnswered==1} [What do I do?]
//     -- ->ExplainWhat
// * {numQsAnswered>1} [NEXT]
//     -- ->END








=== GeneralFeelingsQuery ===
VAR comfort=0
VAR guilt=0
VAR unfairness=0
- N: Mind if I ask a few Q's about how you feel?
    * [SURE]
- N: Picking between [UserPrioFirst] and [UserPrioSecond]. Comfort:
    * [HIGH]
        -- ~comfort=2
    * [MEDIUM]
        -- ~comfort=1
    * [LOW]
        -- ~comfort=0
- N: Picking between [UserPrioFirst] and [UserPrioSecond]. Guilt:
    * [HIGH]
        -- ~guilt=2
    * [MEDIUM]
        -- ~guilt=1
    * [LOW]
        -- ~guilt=0
- N: Picking between [UserPrioFirst] and [UserPrioSecond]. Unfairness:
    * [HIGH]
        -- ~unfairness=2
    * [MEDIUM]
        -- ~unfairness=1
    * [LOW]
        -- ~unfairness=0
- N: Comfort: {comfort}. Guilt: {guilt}. Unfairness: {unfairness}.
- -> END




/*
- -> GameIntro
=== GameIntro ===
VAR numQsAnswered = 0



- N: Hi, [UserName]! I'm Zorro.\\nPrepare to become worthy!
- N: We're gonna play some minigames to make you worthy of things.
* [Worthy of what?]
    -- ->ExplainWhat
* [Makes complete sense]
    -- ->ExplainWhy

= ExplainWhat
~ numQsAnswered++
// - N: Sure, let me explain this game:
- AutoAdvanceAfterSpeech
- N: Well! We're gonna play minigames...
- FuncContinue_ShowFinalRankSample
- ShowTapToContinue(1.2)
- AutoAdvanceAfterSpeech
- N: Then, by the end, we'll have a <color=\#1F71DE>ranked list</color> of your life priorities today!
- FuncContinue_RevealMoreFinalRankSample
- ShowTapToContinue(2)
- N: Hopefully these results will give you something to think about!
- FuncContinue_HideFinalRankSample
* {numQsAnswered==1} [Why priorities?]
    -- ->ExplainWhy
* {numQsAnswered>1} [NEXT]
    -- ->END

= ExplainWhy
- ~numQsAnswered++
- N: The more clarity about what matters to you, the easier it is to make decisions when your desires conflict.
* [Got it.]
    -- N: Awesome.
* [What if they change?]
    -- N: Don't worry, [UserName].\\n\\nClarity for today will be enough for today.
- 
* {numQsAnswered==1} [What do I do?]
    -- ->ExplainWhat
* {numQsAnswered>1} [NEXT]
    -- ->END

*/






