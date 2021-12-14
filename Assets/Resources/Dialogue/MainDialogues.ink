
// VAR numberOneTitle = "<color=\#AD1FDE>President of the World</color>"
// VAR topPrioTitle = "undefined"
// VAR BrattTaylor = "<color=\#6B513B>Bratt Taylor</color>"

- -> GameIntro
=== GameIntro ===
VAR numQsAnswered = 0
// - N: TEST 0.
// - FuncContinue_ShowFinalRankSample
// - N: TEST 1A.
// - N: TEST 1B.
// - FuncContinue_HideFinalRankSample
// - N: TEST 2.
// - FuncContinue_ShowFinalRankSample
// - N: TEST 3.
// - FuncContinue_HideFinalRankSample
// - N: TEST 4.



- N: Hi, [UserName]! I'm Zorro.\\nWelcome to The Priorities Game!
- N: My goal is to improve the clarity of your life priorities today.
* [How?]
    -- ->ExplainHow
* [Why?]
    -- ->ExplainWhy

= ExplainHow
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
    -- ->ExplainHow
* {numQsAnswered>1} [NEXT]
    -- ->END







// Comfort Questions
=== ComfortQuestion0 ===
- N: Comfort Question: How do you feel when I ask you to pick between [UserPrioSecond] and [UserPrioFirst]?
* [Img_Man_0]
    -- N: Cool! Well congrats, deciding between these two (in life) will be easy, then!
* [Img_Man_1]
    -- N: Okay, sounds like you've got an idea of which is more important.
* [Img_Man_2]
    -- N: Aww.
    -- N: What'll happen when these two come into conflict in the future?\\nWhat would it feel like if you already KNEW which was more important?
* [Img_Man_3]
    -- N: Ouchie wowchie!\\n\\nSounds like a conflict you'll try to avoid.
    -- N: But... what if you can't?\\n\\nHow might you feel if you'd already just DECIDED which is more important?
- 
- ->END

=== ComfortQuestion_PostBachelor ===
- N: How did that feel, having to say goodbye to those two priorities?
* [Img_Hand_0]
    -- N: Oho! You're eager to prune the list, huh?
* [Img_Hand_1]
    -- N: That hand looks like "acceptance" to me.
* [Img_Hand_2]
    -- N: Aww.
    -- N: You know you can only focus on so many things at a time, right?\\n\\nFocusing on too many things might be spreading you thin.
* [Img_Hand_3]
    -- N: Aww.
    -- N: You know you can only focus on so many things at a time, right?\\n\\nFocusing on too many things might be spreading you thin.
- ->END

=== ComfortQuestion2 ===
- N: Comfort Question: How do you feel when I ask you to pick between [UserPrioFirst] and [UserPrioSecond]?
* [Img_Baby_0]
    -- N: Wow, you must enjoy whenever these two come into conflict then!
* [Img_Baby_1]
    -- N: Makes you kinda... sleepy?\\n\\nYeah, I can relate.
    -- N: Remember: This game isn't the only time these two will come into conflict.
* [Img_Baby_2]
    -- N: Aww.\\n\\nWell. You care lots about both.
    -- N: Remember: This game isn't the only time these two will come into conflict.
* [Img_Baby_3]
    -- N: Aww.\\n\\nWell. You care lots about both.
    -- N: Remember: This game isn't the only time these two will come into conflict.
- ->END

=== ComfortQuestion3 ===
- N: Comfort Question: How do you feel when I ask you to pick between [UserPrioFirst] and [UserPrioSecond]?
* [Img_Ball_0]
    -- N: Really! Wow! Unfazed!\\n\\nWow. You sound like you <i>know</i> your \#1 priority: [UserPrioFirst]. Just based on what you've told me so far.
* [Img_Ball_1]
    -- N: Okay, not too bad, then!\\n\\nA little sandy, but overall all right.
    -- N: Ask yourself: Why does it make me uncomfortable at all?\\nUseful to know outside of this game.
* [Img_Ball_2]
    -- N: Yeh, I feel ya. Not so thrilled.
    -- N: Hey, ask yourself: WHY does that make me uncomfortable?\\nIt's helpful to know WHY outside of playing this game.
* [Img_Ball_3]
    -- N: Aww. Seen better days, huh?
    -- N: Hey. Ask yourself: WHY does that make me so uncomfortable?\\nHelpful to know WHY outside of playing this game.
- ->END





=== RankTidbit0 ===
- N: [Speech_RankTidbit0]
    * [VERY surprising]
    * [SLIGHTLY surprising]
    * [NOT surprising]
- N: Interesting to note, mm!
- ->END


=== PreTheBachelor ===
- N: All this matchmaking's given me an idea.
* [Oh?]
- N: Yeah, I'm gonna put you on The Bachelorette.\\n\\nYou'll have to ELIMINATE two priorities from the game.
* [Oh.]
- N: That should add some real quality drama, don'tcha think?\\n\\nGood luck!
* [BEGIN]
- 
- ->END

=== PostTheBachelor ===
- N: All right, now THAT's what I call drama!!!
* [...]
- N: ...\\n\\nOh. Sorry.\\n\\nYeah, those priorities are gone from the game.
* [OKAY]
    -- N: Yep.
* [NOT OKAY]
    -- N: Sorry, [UserName], sometimes you gotta cut what doesn't matter to focus on what does.
- N: I mean, the order of your top 4 is what MOSTLY matters.

* [AGREE]
    -- N: That's the spirit.//-- Now you're speakin' my lingo, cuz.
* [DISAGREE]
    -- N: You can only focus on so many things.
    -- N: Brain-power spent on [UserPrioEliminated0] leaves less brain-power to focus on [UserPrioSecond].
    ** [AGREE]
        --- N: Useful to remember when thinking about how you spend your time and energy!
    ** [REBUTTAL]
        --- N: Yeah! I'd rather you have a strong opinion than not care at all.
        --- N: Rock on, [UserName].
- 
- ->END



=== DontCareMuchForLast ===
- N: It looks like you don't care much for [UserPrioLast], huh?
    * [CORRECT]
        -- N: Really?
        -- N: How'd [UserPrioLast] make it on your list at all, then?
        -- ->END
    * [INCORRECT]
    * [VERY INCORRECT]
- N: No? Well. You haven't picked it much.
- N: How's that make you feel?
    * [Img_StuffedAnimal_0]
        -- N: Ah, sounds like you recognize there's too much other important stuff to get distracted by it!
        -- N: You're wise.
        -- ->END
    * [Img_StuffedAnimal_1]
    * [Img_StuffedAnimal_2]
    * [Img_StuffedAnimal_3]
- N: Good.
- N: That means you care.
    * [I DO.]
- N: Also, this is <i>supposed</i> to be tricky.
- N: We don't often shine light on what our priorities actually are.
    * [NEXT]
- N: You're doing great!
- ->END






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


=== ComfortQuestion1 ===
- N: Comfort Question: How do you feel when I ask you to pick between [UserPrioLast] and [UserPrioSecond]?
* [Img_Hand_0]
    -- N: Oh, you're all like "<i>Mmmm, seeeeriously [UserPrioLast] can take a HIKE, I got [UserPrioSecond] ridin' MY shotgun today.</i>"
    -- N: I feel ya, I feel ya!
* [Img_Hand_1]
    -- N: Arright arright, not too bad!
    -- N: Like, you prefer [UserPrioSecond], but you also don't like the idea of sacrificing [UserPrioLast].\\n\\nKeep that in mind outside of this game.
* [Img_Hand_2]
    -- N: Oh, not too confident? ...I actually thought you preferred [UserPrioSecond] rather more than [UserPrioLast].
    -- N: Maybe the decision is easier than you think?
* [Img_Hand_3]
    -- N: Ouch! I wasn't expecting that.
    -- N: It SEEMS like [UserPrioSecond] is more important to you.\\n\\nAsk yourself: Why does that match-up make me uncomfortable?
- ->END


=== Test1 ===
- N: Hi! Testing.
- ->END


=== GameIntro ===
- N: Oh, [UserName]-- So glad you've arrived!
- N: Your priorities are restless.\\nThey don't have a leader.
* [Oh, no!]
    -- N: My thought exactly.\\n\\nIt's anarchy, [UserName].
    ** [Yikes]
    -- N: Yeah.\\nAnd as you know, they're YOUR priorities, so THEY rule YOU.
* [Aren't I their leader?...]
    -- N: Well, no. They're YOUR priorities.\\n\\nTHEY rule YOU.
    ** [Oh yeah]
    -- N: It's a power-struggle out there.\\nThey're all fighting to be \#1.
- 

- N: We need to nominate ONE of them to lead the rest, [UserName].\\n\\nWe need to figure out which priority is \#1.
- N: Let's give it a title, too, something nice and fitting.\\n\\nWhat should we call this top priority?
* [\[UserName\]'s \#1 Priority]
    -- ~topPrioTitle = "<color=\#AD1FDE>[UserName]'s \#1 Priority</color>"
* [\[UserName\]'s Top Dog]
    -- ~topPrioTitle = "<color=\#AD1FDE>[UserName]'s Top Dog</color>"
* [The Most Important]
    -- ~topPrioTitle = "<color=\#AD1FDE>The Most Important</color>"
* [President of \[UserName\]'s World]
    -- ~topPrioTitle = "<color=\#AD1FDE>President of [UserName]'s World</color>"
* [Queen of \[UserName\]'s Life]
    -- ~topPrioTitle = "<color=\#AD1FDE>Queen of [UserName]'s Life</color>"
* [King of \[UserName\]'s Life]
    -- ~topPrioTitle = "<color=\#AD1FDE>King of [UserName]'s Life</color>"
// * [.]
//     -- ~topPrioTitle = "<color=\#AD1FDE></color>"
// * [.]
//     -- ~topPrioTitle = "<color=\#AD1FDE></color>"
- FuncContinue_AssignTopPrioTitle
- N: Ooh! {topPrioTitle}.\\n\\nUnderstated. Yet classy.\\nI love it!
//[TopPrioTitle]

    
// - N: There's good news, though! We've created a position for your \#1 Priority.
// //They want to nominate ONE priority to rule them all.
// - N: This TOP PRIORITY will be your \#1. It'll come before all others.
// - N: There's good news, though!\\n\\nThey've created a position for {numberOneTitle}.
- N: So. {topPrioTitle} will be your \#1 priority.\\nIt will come before all others.
* [Makes sense.]
    // -- Great.// And, as you know, the position will change from time to time.
* [That doesn't fit my lifestyle...]
    -- N: Don't worry, the position isn't permanent.
    -- N: A priority can be {topPrioTitle} for as short as a week.
    -- N: But the position needs to be filled by SOMEone.

// My goal with this game is to improve the clarity of your life priorities today
// Priorities shift, so don't worry about getting the order perfect.\\n\\n

- N: So. Here's the million-dollar question...\\n\\nDo you know which life priority should be {topPrioTitle}?
* [10,000% YES]
    -- N: Good. ->DoKnowWho
* [Uhh...]
    -- N: What do you mean, 'Uhh?', [UserName]?\\nYou don't know who to nominate?!
    ** [I know who.]
        --- N: Okay. Phew.\\n\\n->DoKnowWho
        --- ->DoKnowWho
    ** [Yeah... not sure...]
        --- ->DontKnowWho
* [Nope. Not at ALL.]
    -- ->DontKnowWho

= DontKnowWho
- N: Whoa okay, okay! <i>Oo</i>kay, no PROBlem, <i>keep it toGETher, Bratt, deep BREATHS</i>, hoooo, okay.
- N: Okay.\\n\\nO. Kay.
- N: Well. In that case, we'll have to run a nomination process.
- N: You know what? Let's make that this whole game.\\nThis whole game will be the nomination process.
* [Sounds good.]
// * [
// - We'll pit priorities against each other, and at the end, see who comes out on top.
- N: We're gonna nominate {topPrioTitle} in the only way that makes sense to me:
- N: Minigames.
- ->Ready

= DoKnowWho
// -- And you'll also get some bonus clarity on the lower priorities, too.
- <>Then the nomination process should go smoothly.
- N: We're gonna nominate {topPrioTitle} in the only way that makes sense to me:
- N: Minigames.
- -> Ready

= Ready
* [BEGIN ROUND 1]
- ->END
*/




/*
=== PreRound2 ===
- N: Hey, I forgot to introduce myself earlier--\\n\\nI'm {BrattTaylor}.
* [Nice to meet you, Bratt]
- N: You too, [UserName]!
- N: We're right on track for nominating {topPrioTitle}.
* [What's the purpose of this game?]
    -- N: Glad you asked! My goal is to help you get clarity on your life priorities.
    ** [Got it.]
    ** [Why?]
    -- N: The more clarity about what matters to you, the easier it is to make decisions when your desires conflict.
* [Who are you?]
    -- N: I'm just your friendly neighborhood {BrattTaylor}.\\n\\nI'm here to give you some clarity on your life priorities!
- 

* [What if my priorities change a lot?]
    -- N: Knowing what matters to you today will be enough for today.
    -- N: Seriously. If you know what's most important to you today, it's <i>easy</i> to make most decisions.
    ** [...]
    -- N: Are you wondering 'What if things change'?
    ** [Yeah...]
        --- N: There're too many variables to predict what will matter to you in the future.
        --- N: Plus. What's \#1 to you today will <i>probably</i> still be \#1 tomorrow.\\n\\nOur priorities don't shift as much as we might think.
        // --- N: So, the outcome of this game will be 
    ** [Nah.]
* [Skip additional explanations]
    -- N: Say no more.
    -- ->StartRound2

- N: Okay, enough from me.\\nWe have a priority to nominate for {topPrioTitle}!
- N: Ready for Round 2?
    * [Yep]
        -- N: Hot. You are so hot.
        -- ->StartRound2
    * [Not quite...]
        -- N: What will make you ready?
        ** [Clicking this button]
            --- N: Perfect.
            --- ->StartRound2
        ** [Nothing]
            --- N: ...Oh. Well. Okay.
            --- N: We're gonna start Round 2 eventually.
                *** [Never]
                    ---- N: I do love your rebel spirit.\\n\\nAnyway.
                    ---- ->StartRound2
                *** [Ok let's go]
                    ---- N: Yay!
                    ---- ->StartRound2

= StartRound2
- N: Without further ado, let's open Round 2!
* [BEGIN ROUND 2]
- -> END
*/


/*
// Comfort Questions
=== ComfortQuestion0 ===
- N: Comfort Question: How do you feel when I ask you to pick between [UserPrioSecond] and [UserPrioFirst]?
* [Img_TomHanks_0]
    -- N: Very comfortable! Well congrats, deciding between these two (in life) will be easy, then!
    // -- N: Useful to know in advance, hm?
* [Img_TomHanks_1]
    -- N: Ah, sounds like you've already got an idea of which is more important.
    // -- N: Helpful to think about, hm?
* [Img_TomHanks_2]
    -- N: Fair enough. But what'll happen when these two come into conflict in the future? And what would it feel like if you already KNEW which was more important?
    // -- N: What if you didn't overcomplicate it? Hm?
    //Ah, so when these conflict in your actual life, it must be uncomfortable.
* [Img_TomHanks_3]
    -- N: Ah. Sounds like a conflict you'll try to avoid.
    -- N: But what if you can't?\\n\\nHow might you feel if you'd already just DECIDED which is more important?
    // -- N: What if you didn't overcomplicate it? Hm?//You must already know the pain from when these came into conflict in the past.
- 
// * [HM]
- ->END

=== ComfortQuestion1 ===
- N: Comfort Question: How do you feel when I ask you to pick between [UserPrioLast] and [UserPrioSecond]?
* [Img_MerylStreep_0]
    -- N: Oh, you're all like "<i>Mmmm, seeeeriously [UserPrioLast] can take a HIKE, I got [UserPrioSecond] ridin' MY shotgun today.</i>"
    -- N: I feel ya, I feel ya!
* [Img_MerylStreep_1]
    -- N: Arright arright, so kiiinda confident!
    -- N: Like, you prefer [UserPrioSecond], but you also don't like the idea of sacrificing [UserPrioLast].\\n\\nKeep that in mind outside of this game.
* [Img_MerylStreep_2]
    -- N: Oh, kinda sad? Huh. ...I actually thought you preferred [UserPrioSecond] rather more than [UserPrioLast].
    -- N: Maybe the decision is easier than you think?
* [Img_MerylStreep_3]
    -- N: Oh, really! Tough choice? I wasn't expecting that.
    -- N: It SEEMS like [UserPrioSecond] is more important to you.\\n\\nAsk yourself: Why does that match-up make me uncomfortable?
- ->END

=== ComfortQuestion2 ===
- N: Comfort Question: How do you feel when I ask you to pick between [UserPrioFirst] and [UserPrioSecond]?
* [Img_Baby_0]
    -- N: Wow, you must enjoy whenever these two come into conflict then!
* [Img_Baby_1]
    -- N: Aww, so not thrilled.
    -- N: Remember: This game isn't the only time these two will come into conflict.
* [Img_Baby_2]
    -- N: Aww.\\n\\nWell. You care lots about both.
    -- N: Remember: This game isn't the only time these two will come into conflict.
* [Img_Baby_3]
    -- N: Aww.\\n\\nWell. You care lots about both.
    -- N: Remember: This game isn't the only time these two will come into conflict.
- ->END

=== ComfortQuestion3 ===
- N: Comfort Question: How do you feel when I ask you to pick between [UserPrioFirst] and [UserPrioSecond]?
* [Img_PicnicTable_0]
    -- N: Really! Wow! Unfazed!\\n\\nWow. You sound like you <i>know</i> your \#1 priority: [UserPrioFirst]. Just based on what you've told me so far.
* [Img_PicnicTable_1]
    -- N: All right, not too bad, then! I mean, you're not <i>happy</i>, but not terrible either.
    -- N: Ask yourself: Why does it make me uncomfortable at all?\\nUseful to know outside of this game.
* [Img_PicnicTable_2]
    -- N: Yeh, I feel ya. Not so thrilled.
    -- N: Hey, ask yourself: WHY does that make me uncomfortable?\\nIt's helpful to know WHY outside of playing this game.
* [Img_PicnicTable_3]
    -- N: Ouch. You could use a good repair-person, huh?
    -- N: Hey. Ask yourself: WHY does that make me so uncomfortable?\\nHelpful to know WHY outside of playing this game.
- ->END
*/






