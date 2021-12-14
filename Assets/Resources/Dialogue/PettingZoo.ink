VAR rocketStr = "<color=\#FF0044>goat</color>"
VAR knownUserName = "[UserName]"


- PetZooHost: Welcome to Kittery Point Petting Zoo, hun!\\n\\nI'm Jesse Bessum.
- PetZooHost: Wha's yer name, hun?
* [\[UserName\]]
    -- ~knownUserName = "[UserName]"
// * [Jesus Flagrante]
//     -- ~knownUserName = "Jesus"
// * [Yipyack Gadsmack]
//     -- ~knownUserName = "Yipyack"
// * [Yack Dassarray]
//     -- ~knownUserName = "Yack"
// * [New Coke]
//     -- ~knownUserName = "New Coke"
// * [Heytard Hellbeck]
//     -- ~knownUserName = "Heytard"
// * [Lookatme Mylegsaretoolong]
//     -- ~knownUserName = "Lookatme"
* [Bango Grabbo]
    -- ~knownUserName = "Bango"
* [Banjiman Kaboozie]
    -- ~knownUserName = "Banjiman"
* [Mr. Pixar Nickelodeon]
    -- ~knownUserName = "Mr. Pixar"
* [Saint Tickleus]
    -- ~knownUserName = "Saint Tickleus"
* [Gooseperson \#3]
    -- ~knownUserName = "Gooseperson \#3"
- PetZooHost: Ooh, I like the sounda that, {knownUserName}!
- PetZooHost: You wanna guess what we feed our aminals here?
* [Hay]
    -- PetZooHost: 'Hey' what? You already got my attention!\\n\\nHar har! Tha's a lil' joke, there.
* [Grapes]
    -- PetZooHost: Naw, grapes are the Devil's food.\\nWe only use grapes fer séances.
* [Each other]
    -- PetZooHost: Nah, tha's how I feed my kids.
    -- PetZooHost: Har har, jeeee-ust kiddin'.
* [Priorities]
    -- PetZooHost: Right-o! Dang.\\n\\nLook sharp, {knownUserName}!
- PetZooHost: We feed our animals <i>priorities</i> here.

- FuncHalt_Anim_ShowGoatBody
- PetZooHost: This is our goat, Beastie.\\nShe's a romper!
- FuncHalt_Anim_EmphasizeGoatBody
- PetZooHost: She's peckish as all heck, {knownUserName}.\\n\\nSay, would you mind feedin' her TWO priorities?
- PetZooHost: Pick careful! What you feed Beastie, you ain't takin' back home!
- FuncContinue_HideCharViews
- FuncHalt_ShowContViews
- FuncHalt_ShowContViews
- FuncContinue_HideContViews
- FuncContinue_Anim_GoatReturnToNormal
- PetZooHost: Aww, Beastie says 'Thanks for the snack, {knownUserName}!'
- PetZooHost: Sure looked tasty.\\n\\nHar har!
- PetZooHost: Y'all take care now, {knownUserName}!

* [END]
- FuncHalt_Anim_Ending
- ->DONE





// - PetZooHost: This is an ELIMINATION.

// * [My pleasure]
//     -- 
// * [For real?]
//     --
// - 

// - PetZooHost: Please feed the goat 
// * [Okay]
// * [What goat?]



    
    